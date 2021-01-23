using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using static DDNS.Startup;

namespace DDNS.DAL
{

    public class traefikDAL
    {
        private static string traefikhostName = EnvironmentHelper.Arguments["traefikhostName"];

        //TODO: Adding blacklist of domain!
        // private static List<string> traefikBlackList = (EnvironmentHelper.Arguments["traefikBlackList"] as List<string>);
        private static List<string> traefikBlackList = new List<string>();
        public async Task<List<string>> ProcessRepositories()
        {

            using (var httpClientHandler = new HttpClientHandler())
            {
                httpClientHandler.ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => { return true; };
                using (var client = new HttpClient(httpClientHandler))
                {
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Add("User-Agent", ".NET Foundation Repository Reporter");

                    var msg = await client.GetStringAsync(traefikhostName);

                    List<string> services = new List<string>();
                    JToken myJObject = null;
                    try
                    {
                        myJObject = JArray.Parse(msg);
                    }
                    catch
                    {
                        myJObject = JObject.Parse(msg)["kubernetes"]["frontends"];
                    }
                    foreach (JToken item in myJObject)
                    {
                        string servName = string.Empty;
                        try
                        {
                            servName = item.Value<string>("rule").Replace("Host(`", "").Replace("`)", "");

                        }
                        catch
                        {
                            servName = (item as JProperty).Name;
                        }
                        var a = Uri.CheckHostName(servName) != UriHostNameType.Unknown;
                        var b = !traefikBlackList.Exists(x => x.ToLower() == servName.ToLower());
                        if (a && b)
                        {
                            services.Add(servName);
                        }
                    }

                    return services;
                }
            }
        }
    }
}