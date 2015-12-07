using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;

namespace LegacyJS.Test
{
    [TestClass]
    public class LegacyJSTest
    {
        [TestMethod] 
        public void InsertedText_Test() 
        {
            var request = (HttpWebRequest)WebRequest.Create(@"http://localhost/TestIISWebApp/sample.js");
            var response = request.GetResponse();

            using (var readStream = new StreamReader(response.GetResponseStream(), Encoding.UTF8))
            {
                var text = readStream.ReadToEnd();
                Assert.IsTrue(text.Contains("inserted text"));
            }
        }
        [TestMethod]
        public void RequestTypeOk_Test()
        {
            var request = (HttpWebRequest)WebRequest.Create(@"http://localhost/TestIISWebApp/sample.js");
            request.Headers.Add("Cache-Control", "private, max-age=0, no-cache");
            request.ContentType = "application/javascript";
            var response = request.GetResponse();

            using (var readStream = new StreamReader(response.GetResponseStream(), Encoding.UTF8))
            {
                var text = readStream.ReadToEnd();
                Assert.IsTrue(text.Contains("That is a js request"));
            }
        }

        [TestMethod]
        public void RequestTypeKo_Test()
        {
            var request = (HttpWebRequest)WebRequest.Create(@"http://localhost/TestIISWebApp/sample.css");
            request.Headers.Add("Cache-Control", "private, max-age=0, no-cache");
            request.Headers.Add("Cache-Control", "no-cache");
            var response = request.GetResponse();

            using (var readStream = new StreamReader(response.GetResponseStream(), Encoding.UTF8))
            {
                var text = readStream.ReadToEnd();
                Assert.IsTrue(!text.Contains("That is a js request"));
            }
        }
    }
}
