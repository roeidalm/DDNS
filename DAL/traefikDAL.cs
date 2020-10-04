using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using static DDNS.Startup;

namespace DDNS.DAL {

    public class traefikDAL {
        private static string traefikhostName = EnvironmentHelper.Arguments["traefikhostName"];
        private static async Task<List<string>> ProcessRepositories () {

            using (var httpClientHandler = new HttpClientHandler ()) {
                httpClientHandler.ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => { return true; };
                using (var client = new HttpClient (httpClientHandler)) {
                    client.DefaultRequestHeaders.Accept.Clear ();
                    client.DefaultRequestHeaders.Add ("User-Agent", ".NET Foundation Repository Reporter");
                    string callurl = string.Format ("https://{0}/api", traefikhostName);

                    var msg = await client.GetStringAsync (callurl);

                    var myJObject = JObject.Parse (msg) ["kubernetes"]["frontends"];
                    List<string> services = new List<string> ();
                    foreach (JToken item in myJObject) {
                        string servName = (item as JProperty).Name;
                        if (servName.ToLower () != traefikhostName.ToLower ()) {
                            services.Add (servName);
                        }
                    }

                    return services;
                }
            }
        }
    }
}