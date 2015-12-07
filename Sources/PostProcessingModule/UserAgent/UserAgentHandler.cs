using System;
using System.Collections.Generic;
using System.Reflection;
using System.Web;

namespace PostProcessing
{
    /// <summary>
    /// The handler that will allow to change http response
    /// </summary>
    public class UserAgentHandler : IHttpModule
    {
        
        private HttpApplication _context;

        private HttpRequest Request
        {
            get { return _context?.Request; }
        }

        private HttpResponse Response
        {
            get { return _context?.Response; }
        }

        public void Dispose()
        {
        }

        public void Init(HttpApplication context)
        {
            _context = context;

            _context.PreRequestHandlerExecute += _context_PreRequestHandlerExecute;
        }

        private void _context_PreRequestHandlerExecute(object sender, EventArgs e)
        {
            var field = typeof(HttpRequest)
                .GetField("_wr", BindingFlags.Instance | BindingFlags.NonPublic);

            var currentWorkerRequest = field.GetValue(Request);
        }

    }
}
