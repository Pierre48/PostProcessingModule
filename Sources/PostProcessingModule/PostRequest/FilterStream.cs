using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Handlers;

namespace LegacyJS
{
    /// <summary>
    /// This class is an implementation of the filter pattern on a stream 
    /// It allows to update JS, or html
    /// This class can handle the with page result, when axd stream is updated.
    /// </summary>
    internal class FilterStream : Stream
    {
        private readonly Encoding _encoding;
        private readonly Stream _filter;
        private List<Change> _changes;

        /// <summary>
        /// Initializes a new instance of the <see cref="FilterStream"/> class.
        /// </summary>
        /// <param name="filter">The filter.</param>
        /// <param name="encoding">The encoding.</param>
        public FilterStream(Stream filter, Encoding encoding)
        {
            _encoding = encoding;
            _filter = filter;
            _changes = new List<Change>();
        }

        #region Wrapped method (Processing is done by the original stream

        public override bool CanRead
        {
            get { return _filter.CanRead; }
        }

        public override bool CanSeek
        {
            get { return _filter.CanSeek; }
        }

        public override bool CanWrite
        {
            get { return _filter.CanWrite; }
        }

        public override long Length
        {
            get { return _filter.Length; }
        }

        public override long Position
        {
            get { return _filter.Position; }

            set { _filter.Position = value; }
        }

        public override void Flush()
        {
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            return _filter.Read(buffer, offset, count);
        }

        public override long Seek(long offset, SeekOrigin origin)
        {
            return _filter.Seek(offset, origin);
        }

        public override void SetLength(long value)
        {
            _filter.SetLength(value);
        }
        #endregion

        /// <summary>
        /// Allows the further writes on axd stream.
        /// force the value _ignoringFurtherWrites to false, even if 
        /// The code is considered legacy by Microsoft and won't be fixed unless they consider it a critical issue.
        /// 
        /// The workaround is to implement a custom http module to correct the code before setting _ignoringFurtherWrites to false and  sending it to the browser.
        /// </summary>
        private void AllowFurtherWrites()
        {
            if (!HttpContext.Current.Request.FilePath.EndsWith(".axd", StringComparison.InvariantCultureIgnoreCase))
                return;

            // We get the HttpWritter
            var httpWriterField = HttpContext.Current.Response.GetType()
                .GetField("_httpWriter", BindingFlags.NonPublic | BindingFlags.Instance);

            if (httpWriterField != null)
            {
                var httpWriter = httpWriterField.GetValue(HttpContext.Current.Response);

                if (httpWriter != null)
                {
                    // we get _ignoringFurtherWrites property
                    var ignoringFurtherWritesField = httpWriter.GetType()
                        .GetField("_ignoringFurtherWrites", BindingFlags.NonPublic | BindingFlags.Instance);

                    if (ignoringFurtherWritesField != null)
                    {
                        ignoringFurtherWritesField.SetValue(httpWriter, false);
                    }
                }
            }
        }

        /// <summary>
        /// Writes the specified buffer.
        /// </summary>
        /// <param name="buffer">The buffer.</param>
        /// <param name="offset">The offset.</param>
        /// <param name="count">The count.</param>
        public override void Write(byte[] buffer, int offset, int count)
        {
            AllowFurtherWrites();

            if (count < 1) return;
            if (offset < 0) return;

            var toBeWritten = new byte[count];
            Array.Copy(buffer, offset, toBeWritten, 0, count);

            var str = _encoding.GetString(toBeWritten);

            if (_changes != null)
            {
                foreach (var change in _changes)
                {
                    str = Regex.Replace(str, change.OldString, change.NewString, RegexOptions.IgnoreCase);
                }
            }

            var bytes = _encoding.GetBytes(str);

            _filter.Write(bytes, offset, bytes.Length);
        }

        internal void AddChanges(List<Change> changes)
        {
            _changes.AddRange(changes);
        }
    }
}