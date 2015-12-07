using System;
using System.Collections.Generic;
using System.Net.Cache;
using System.Web;

namespace PostProcessing
{
    /// <summary>
    /// A rule allows to specify which kind of request has to be modified, and which changes has to be applied
    /// </summary>
    public class Rule
    {
        /// <summary>
        /// The file path end with
        /// </summary>
        public List<string> FilePathEndWith ;

        /// <summary>
        /// ContentTypes
        /// </summary>
        public List<string> ContentTypes;

        /// <summary>
        /// The changes/
        /// </summary>
        public List<Change> Changes ;

        /// <summary>
        /// Checks the specified request.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <param name="response">The response.</param>
        /// <returns></returns>
        public bool Check(HttpRequest request, HttpResponse response)
        {
            var result = false;
            if (FilePathEndWith != null)
            {
                foreach (var end in FilePathEndWith)
                {
                    result |= end != null && request.FilePath.EndsWith(end, StringComparison.InvariantCultureIgnoreCase);
                }
                if (!result) return result;
            }

            if (ContentTypes != null)
            {
                foreach (var contentType in ContentTypes)
                {
                    result |= contentType != null && string.Equals(request.ContentType, contentType,StringComparison.InvariantCultureIgnoreCase);
                }
                if (!result) return result;
            }

            return true;
        }

        private bool AcceptTypes(string[] acceptTypes, string contentType)
        {
            if (acceptTypes == null) return false;

            foreach (var acceptType in acceptTypes)
            {
                if (string.Equals(acceptType, contentType, StringComparison.CurrentCultureIgnoreCase)) return true;
            }
            return false;
        }
    }
}