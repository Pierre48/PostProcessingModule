namespace PostProcessing
{
    using PostProcessing.PostRequest;
    using System;
    using System.Web;

    /// <summary>
    /// The handler that will allow to change http response
    /// </summary>
    public class LegacyHandler : IHttpModule
    {
        private HttpApplication _context;

        /// <summary>
        /// The current request
        /// </summary>
        private HttpRequest Request
        {
            get { return _context?.Request; }
        }

        /// <summary>
        /// the current response
        /// </summary>
        private HttpResponse Response
        {
            get { return _context?.Response; }
        }

        public void Dispose()
        {
            // Nothing to release
        }

        /// <summary>
        /// Initilization of the request
        /// </summary>
        /// <param name="context"></param>
        public void Init(HttpApplication context)
        {
            _context = context;
            _context.PostRequestHandlerExecute += PostRequestHandlerExecute;
        }

        private void PostRequestHandlerExecute(object sender, EventArgs e)
        {
            if (Request.FilePath.EndsWith("PostProcessingConfiguration.xml", StringComparison.InvariantCultureIgnoreCase))
                return;

            //if (Request.ReadEntityBodyMode  == ReadEntityBodyMode.None)

            //    // Change are only applied when the status is 200.
            //    // It avoids to apply several time the same change
            //return;

            Configuration.Intialize(Request);
            var application = (HttpApplication)sender;

            if (Configuration.Current.Rules!=null)
            foreach (var rule in Configuration.Current.Rules)
            {
                if (rule.Check(Request, Response))
                {// We check rules that must be applied
                    FilterStream filter = application.Response.Filter as FilterStream;
                    if (filter == null)
                    {// Change are applied through a filter
                        filter = new FilterStream(Response.Filter, Response.ContentEncoding);
                        application.Response.Filter = filter;
                    }

                    // If a filter is altready applied, then we add the rule to the existing filter
                    filter.AddChanges(rule.Changes);

                    application.Response.Filter = filter;
                }
            }

        }

    }
}
