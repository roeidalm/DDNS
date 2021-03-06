using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using DDNS.models;
using Newtonsoft.Json.Linq;
using static DDNS.Startup;

namespace DDNS.DAL
{

    public class ClodflareDAL : IClodflareDAL
    {
        private static string email = EnvironmentHelper.Arguments["email"];
        private static string toekn = EnvironmentHelper.Arguments["toeknclodflare"];
        private static string zoneId = "";
        private static Dictionary<string, DnsRecord> dnsList;
        static ClodflareDAL()
        {

            //TODO: edit the code to use client factory

            //this will init the zone ID
            using (var httpClientHandler = new HttpClientHandler())
            {
                httpClientHandler.ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => { return true; };
                using (var client = new HttpClient())
                {

                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Add("X-Auth-Email", email);
                    client.DefaultRequestHeaders.Add("X-Auth-Key", toekn);
                    client.DefaultRequestHeaders.Accept
                        .Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    var msg = client.GetStringAsync("https://api.cloudflare.com/client/v4/zones").GetAwaiter().GetResult();
                    zoneId = JObject.Parse(msg).ToObject<ZoneIdData>().result[0].id;
                }
            }

        }

        public async void createNewARecordes(List<string> dnsNameToCreate)
        {
            using (var client = new HttpClient())
            {

                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Add("X-Auth-Email", email);
                client.DefaultRequestHeaders.Add("X-Auth-Key", toekn);
                client.DefaultRequestHeaders.Accept
                    .Add(new MediaTypeWithQualityHeaderValue("application/json"));

                DnsCreate dnsCreate;
                string publicIP = GetIP();
                string dnsListUri = string.Format("https://api.cloudflare.com/client/v4/zones/{0}/dns_records", zoneId);
                foreach (var item in dnsNameToCreate)
                {
                    dnsCreate = new DnsCreate()
                    {
                        type = "A",
                        name = item.Split('.')[0],
                        content = publicIP,
                        ttl = 120,
                        priority = 10,
                        proxied = true
                    };
                    using (var content = new StringContent(Newtonsoft.Json.JsonConvert.SerializeObject(dnsCreate), System.Text.Encoding.UTF8, "application/json"))
                    {

                        var res = await client.PostAsync(dnsListUri, content);

                        if (res.StatusCode == System.Net.HttpStatusCode.OK)
                        {
                            var a = res.Content.ReadAsStringAsync().Result;
                            dnsList.Add(item, new DnsRecord("id", publicIP));
                        }
                    }
                }
            }
        }
        public async void updateARecordes(Dictionary<string, string> dnsIDToUpdate)
        {
            using (var client = new HttpClient())
            {

                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Add("X-Auth-Email", email);
                client.DefaultRequestHeaders.Add("X-Auth-Key", toekn);
                client.DefaultRequestHeaders.Accept
                    .Add(new MediaTypeWithQualityHeaderValue("application/json"));

                DnsCreate dnsUpdate;
                string publicIP = GetIP();

                foreach (var item in dnsIDToUpdate)
                {
                    dnsUpdate = new DnsCreate()
                    {
                        type = "A",
                        name = item.Key.Split('.')[0],
                        content = publicIP,
                        ttl = 120,
                        priority = 10,
                        proxied = true
                    };

                    using (var content = new StringContent(Newtonsoft.Json.JsonConvert.SerializeObject(dnsUpdate), System.Text.Encoding.UTF8, "application/json"))
                    {
                        string dnsListUri = string.Format("https://api.cloudflare.com/client/v4/zones/{0}/dns_records/{1}", zoneId, item.Value);
                        var res = await client.PutAsync(dnsListUri, content);
                        if (res.StatusCode == System.Net.HttpStatusCode.OK)
                        {
                            var a = res.Content.ReadAsStringAsync().Result;
                        }
                    }
                }
            }
        }

        public async void deleteARecordes(List<string> dnsIDToUpdate)
        {
            using (var client = new HttpClient())
            {

                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Add("X-Auth-Email", email);
                client.DefaultRequestHeaders.Add("X-Auth-Key", toekn);
                client.DefaultRequestHeaders.Accept
                    .Add(new MediaTypeWithQualityHeaderValue("application/json"));

                foreach (var item in dnsIDToUpdate)
                {

                    string dnsListUri = string.Format("https://api.cloudflare.com/client/v4/zones/{0}/dns_records/{1}", zoneId, item);
                    var res = await client.DeleteAsync(dnsListUri);

                }
            }
        }
        public Dictionary<string, DnsRecord> getDnsList()
        {
            if (dnsList == null)
            {
                refreshDnsList();
            }
            return dnsList;

        }
        public async void refreshDnsList()
        {
            using (var httpClientHandler = new HttpClientHandler())
            {
                httpClientHandler.ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => { return true; };
                using (var client = new HttpClient())
                {

                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Add("X-Auth-Email", email);
                    client.DefaultRequestHeaders.Add("X-Auth-Key", toekn);
                    client.DefaultRequestHeaders.Accept
                        .Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    string dnslisturi = string.Format("https://api.cloudflare.com/client/v4/zones/{0}/dns_records", zoneId);
                    var awiter = client.GetStringAsync(dnslisturi).GetAwaiter();
                    var data = awiter.GetResult();
                    var dnsObjlist = JObject.Parse(data).ToObject<DnsListData>().result;
                    Dictionary<string, DnsRecord> temp = new Dictionary<string, DnsRecord>();
                    foreach (var item in dnsObjlist)
                    {
                        temp.Add(item.name, new DnsRecord(item.id, item.content));
                    }
                    dnsList = temp;
                }
            }
        }
        public string GetIP()
        {
            string externalIP = "";
            externalIP = (new System.Net.WebClient()).DownloadString("http://checkip.dyndns.org/");
            externalIP = (new System.Text.RegularExpressions.Regex(@"\d{1,3}\.\d{1,3}\.\d{1,3}\.\d{1,3}")).Matches(externalIP)[0].ToString();
            return externalIP;
        }
    }
}