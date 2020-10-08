using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using DDNS.DAL;
using DDNS.models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using static DDNS.Startup;

namespace DDNS.Controllers {
    [ApiController]
    [Route ("[controller]")]
    public class AdministrationController : ControllerBase {
        private static readonly HttpClient client = new HttpClient ();
        private readonly ILogger<WeatherForecastController> _logger;

        private static List<string> dnsList = new List<string> ();
        private static List<string> services = new List<string> ();

        public AdministrationController (ILogger<WeatherForecastController> logger) {
            _logger = logger;
        }

        [HttpGet]
        public IEnumerable<string> Get () {
            ClodflareDAL dal = new ClodflareDAL ();
            var f = dal.GetIP ();
            return new List<string> () { "value 1", "value 2" };
        }
        // void tenmp () {
        //     using (var httpClientHandler = new HttpClientHandler ()) {
        //         httpClientHandler.ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => { return true; };
        //         using (var client = new HttpClient ()) {

        //             client.DefaultRequestHeaders.Accept.Clear ();
        //             client.DefaultRequestHeaders.Add ("X-Auth-Email", email);
        //             client.DefaultRequestHeaders.Add ("X-Auth-Key", toekn);
        //             client.DefaultRequestHeaders.Accept
        //                 .Add (new MediaTypeWithQualityHeaderValue ("application/json"));

        //             var temp = getDnsList (client).Result;
        //             List<string> dnsToCreate = new List<string> ();
        //             foreach (var item in services) {
        //                 if (!temp.Exists (x => x == item)) {
        //                     dnsToCreate.Add (item);
        //                 }
        //             }
        //         }
        //     }
        // }
    }
}