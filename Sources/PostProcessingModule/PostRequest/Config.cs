using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

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

        private static void IntializeFromWwwRoot()
        {
            var request = HttpWebRequest.Create(@"http://localhost/PostProcessingConfiguration.xml");
            request.Headers.Add("Cache-Control", "private, max-age=0, no-cache");
            var response = request.GetResponse();

            var serializer = new DataContractSerializer(typeof(List<Rule>));
            _config.Rules = serializer.ReadObject(response.GetResponseStream()) as List<Rule>;
        }
        internal static void Intialize()
        {
            _config = new Configuration();
            IntializeFromWwwRoot();
        }
    }
}
