using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace PostProcessing.UserAgent
{
    internal class HttpWorkerRequestMock : HttpWorkerRequest
    {
        string _userAgent;

        HttpWorkerRequest _wrappedObject = null;
        public HttpWorkerRequestMock(HttpWorkerRequest wrappedObject, string userAgent)
        {
            _wrappedObject = wrappedObject;
            _userAgent = userAgent;
        }

        public override string GetKnownRequestHeader(int index)
        {

            if (index == HttpWorkerRequest.HeaderUserAgent)
            {
                return _userAgent;
            }
            return _wrappedObject.GetKnownRequestHeader(index);
        }

        public override void EndOfRequest()
        {
            _wrappedObject.EndOfRequest();
        }

        public override void FlushResponse(bool finalFlush)
        {
            _wrappedObject.FlushResponse(finalFlush);
        }

        public override string GetHttpVerbName()
        {
            return _wrappedObject.GetHttpVerbName();
        }

        public override string GetHttpVersion()
        {
            return _wrappedObject.GetHttpVersion();
        }

        public override string GetLocalAddress()
        {
            return _wrappedObject.GetLocalAddress();
        }

        public override int GetLocalPort()
        {
            return _wrappedObject.GetLocalPort();
        }

        public override string GetQueryString()
        {
            return _wrappedObject.GetQueryString();
        }

        public override string GetRawUrl()
        {
            return _wrappedObject.GetRawUrl();
        }

        public override string GetRemoteAddress()
        {
            return _wrappedObject.GetRemoteAddress();
        }

        public override int GetRemotePort()
        {
            return _wrappedObject.GetRemotePort();
        }

        public override string GetUriPath()
        {
            return _wrappedObject.GetUriPath();
        }

        public override void SendKnownResponseHeader(int index, string value)
        {
            _wrappedObject.SendKnownResponseHeader(index, value);
        }

        public override void SendResponseFromFile(IntPtr handle, long offset, long length)
        {
             _wrappedObject.SendResponseFromFile(handle, offset, length);
        }

        public override void SendResponseFromFile(string filename, long offset, long length)
        {
             _wrappedObject.SendResponseFromFile(filename, offset, length);
        }

        public override void SendResponseFromMemory(byte[] data, int length)
        {
            _wrappedObject.SendResponseFromMemory(data, length);
        }

        public override void SendStatus(int statusCode, string statusDescription)
        {
            _wrappedObject.SendStatus(statusCode, statusDescription);
        }

        public override void SendUnknownResponseHeader(string name, string value)
        {
            _wrappedObject.SendUnknownResponseHeader(name, value);
        }
    }
}
