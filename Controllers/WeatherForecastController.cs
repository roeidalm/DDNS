using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using DDNS.models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using static DDNS.Startup;

namespace DDNS.Controllers {
    [ApiController]
    [Route ("[controller]")]
    public class WeatherForecastController : ControllerBase {
        private static string hostName = EnvironmentHelper.Arguments["traefikhostName"];
        private static readonly HttpClient client = new HttpClient ();
        private readonly ILogger<WeatherForecastController> _logger;

        private static List<string> dnsList = new List<string> ();
        private static List<string> services = new List<string> ();

        public WeatherForecastController (ILogger<WeatherForecastController> logger) {
            _logger = logger;
        }

        [HttpGet]
        public IEnumerable<string> Get () {
            ProcessRepositories ().Result.ToArray ();
            var a = updateClodflare ();
            a.Wait ();
            return new List<string> () { "dsad" };
        }

        private static async Task<List<string>> ProcessRepositories () {

            using (var httpClientHandler = new HttpClientHandler ()) {
                httpClientHandler.ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => { return true; };
                using (var client = new HttpClient (httpClientHandler)) {
                    client.DefaultRequestHeaders.Accept.Clear ();
                    client.DefaultRequestHeaders.Add ("User-Agent", ".NET Foundation Repository Reporter");
                    string callurl = string.Format ("https://{0}/api", hostName);

                    var stringTask = client.GetStringAsync (callurl);

                    var msg = await stringTask;
                    var myJObject = JObject.Parse (msg) ["kubernetes"]["frontends"];

                    foreach (JToken item in myJObject) {
                        string servName = (item as JProperty).Name;
                        if (servName.ToLower () != hostName.ToLower ()) {
                            services.Add (servName);
                        }
                    }

                    return services;
                }
            }
        }

    }
}