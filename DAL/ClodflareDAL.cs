using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using DDNS.models;
using Newtonsoft.Json.Linq;
using static DDNS.Startup;

namespace DDNS.DAL {
    public class ClodflareDAL {
        private static string email = EnvironmentHelper.Arguments["email"];
        private static string toekn = EnvironmentHelper.Arguments["toeknclodflare"];
        private static string zoneId = "";
        static ClodflareDAL () {

            //TODO: edit the code to use client factory
            using (var httpClientHandler = new HttpClientHandler ()) {
                httpClientHandler.ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => { return true; };
                using (var client = new HttpClient ()) {

                    client.DefaultRequestHeaders.Accept.Clear ();
                    client.DefaultRequestHeaders.Add ("X-Auth-Email", email);
                    client.DefaultRequestHeaders.Add ("X-Auth-Key", toekn);
                    client.DefaultRequestHeaders.Accept
                        .Add (new MediaTypeWithQualityHeaderValue ("application/json"));
                    GetZodeID (client).Wait ();
                }
            }

        }
        private async Task updateClodflare () {

            using (var httpClientHandler = new HttpClientHandler ()) {
                httpClientHandler.ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => { return true; };
                using (var client = new HttpClient ()) {

                    client.DefaultRequestHeaders.Accept.Clear ();
                    client.DefaultRequestHeaders.Add ("X-Auth-Email", email);
                    client.DefaultRequestHeaders.Add ("X-Auth-Key", toekn);
                    client.DefaultRequestHeaders.Accept
                        .Add (new MediaTypeWithQualityHeaderValue ("application/json"));

                    var temp = getDnsList (client).Result;
                    List<string> dnsToCreate = new List<string> ();
                    foreach (var item in services) {
                        if (!temp.Exists (x => x == item)) {
                            dnsToCreate.Add (item);
                        }
                    }
                    if (dnsToCreate.Count > 0) {

                        DnsCreate dnsCreate;
                        foreach (var item in dnsToCreate) {
                            dnsCreate = new DnsCreate () {
                                type = "A",
                                name = item.Split ('.') [0],
                                content = getPublicIp (),
                                ttl = 120,
                                priority = 10,
                                proxied = true
                            };
                            using (var content = new StringContent (Newtonsoft.Json.JsonConvert.SerializeObject (dnsCreate), System.Text.Encoding.UTF8, "application/json")) {

                                var res = await client.PostAsync (dnslisturi, content);
                            }

                            if ("status 200" == "status 200") {
                                dnsList.Add (item);

                            }
                        }
                    }
                }
            }
        }

        private static async Task GetZodeID (HttpClient client) {
            var msg = await client.GetStringAsync ("https://api.cloudflare.com/client/v4/zones");

            zoneId = JObject.Parse (msg).ToObject<ZoneIdData> ().result[0].id;
        }

        private async Task<List<string>> getDnsList (HttpClient client) {
            string dnslisturi = string.Format ("https://api.cloudflare.com/client/v4/zones/{0}/dns_records", zoneId);
            var data = await client.GetStringAsync (dnslisturi);

            var dnsObjlist = JObject.Parse (data).ToObject<DnsListData> ().result;
            List<string> temp = new List<string> ();
            foreach (var item in dnsObjlist) {
                temp.Add (item.name);
            }
            return temp;
        }
        private string getPublicIp () {
            return "87.71.231.222";
        }
    }
}