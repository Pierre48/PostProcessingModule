using PostProcessing.PostRequest;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Web;

namespace PostProcessing
{
    /// <summary>
    /// The handler that will allow to change http response
    /// </summary>
    public class LegacyHandler : IHttpModule
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

            _context.PostRequestHandlerExecute += PostRequestHandlerExecute;
        }

        private void PostRequestHandlerExecute(object sender, EventArgs e)
        {
            if (Request.FilePath.EndsWith("PostProcessingConfiguration.xml", StringComparison.InvariantCultureIgnoreCase))
                return;

            if (Response.StatusCode != 200)
                // Change are only applied when the status is 200.
                // It avoids to apply several time the same change
                return;

            Configuration.Intialize(Request);
            var application = (HttpApplication)sender;

            foreach (var rule in Configuration.Current.Rules)
            {
                if (rule.Check(Request, Response))
                {
                    FilterStream filter = application.Response.Filter as FilterStream;
                    if (filter == null)
                    {
                        filter = new FilterStream(Response.Filter, Response.ContentEncoding);
                        application.Response.Filter = filter;
                    }

                    filter.AddChanges(rule.Changes);

                    application.Response.Filter = filter;
                }
            }

        }

    }
}
