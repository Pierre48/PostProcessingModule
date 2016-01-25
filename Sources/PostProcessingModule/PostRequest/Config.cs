namespace PostProcessing.PostRequest
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Net;
    using System.Web;
    using System.Xml.Serialization;

    // An internal class that contains the configuration
    internal class Configuration
    {
        static Configuration _config;

        /// <summary>
        ///  Get the current configuration
        /// </summary>
        internal static Configuration Current
        {
            get
            {
                if (_config == null) throw new Exception("Not initialized");
                return _config;
            }
        }

        /// <summary>
        /// Contains the list of configured rules
        /// </summary>
        public List<Rule> Rules { get; private set; }

        /// <summary>
        /// Allow to initialize the configuration from a path
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        private static bool IntializeFrom(string path= @"c:\inetpub\wwwwroot\")
        {
            try
            {
                var filePath = Path.Combine(path, "PostProcessingConfiguration.xml");
                if (!File.Exists(filePath)) return false;
                
                XmlSerializer serializer = new XmlSerializer(typeof(List<Rule>));

                using (var reader = new StreamReader(filePath))
                {
                    _config.Rules = serializer.Deserialize(reader) as List<Rule>;
                }
                return true;
            }
            catch (WebException)
            {
                _config.Rules = new List<Rule>();
                return false;
            }
        }

        /// <summary>
        /// Initialize the configuration for a request
        /// </summary>
        /// <param name="request"></param>
        internal static void Intialize(HttpRequest request)
        {
            _config = new Configuration();

            if (!IntializeFrom(request.PhysicalApplicationPath))
                IntializeFrom();
        }
    }
}
