using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Runtime.Serialization;
using System.Text;
using System.Web;
using System.Xml.Serialization;

namespace PostProcessing.PostRequest
{
    internal class Configuration
    {


        static Configuration _config;


        internal static Configuration Current
        {
            get
            {
                if (_config == null) throw new Exception("Not initialized");
                return _config;
            }
        }

        public List<Rule> Rules { get; private set; }

        private static bool IntializeFrom(string path)
        {
            try
            {
                if (!path.EndsWith(@"/")) path = path + "/";

                var request = HttpWebRequest.Create(@"http://localhost" + path + "PostProcessingConfiguration.xml");
                request.Headers.Add("Cache-Control", "private, max-age=0, no-cache");
                var response = request.GetResponse();
                XmlSerializer serializer = new XmlSerializer(typeof(List<Rule>));

                _config.Rules = serializer.Deserialize(response.GetResponseStream()) as List<Rule>;
                return true;
            }
            catch (WebException)
            {
                _config.Rules = new List<Rule>();
                return false;
            }
        }

        internal static void Intialize(HttpRequest request)
        {
            _config = new Configuration();

            if (!IntializeFrom(request.ApplicationPath))
                IntializeFrom(@"/");
        }
    }
}
