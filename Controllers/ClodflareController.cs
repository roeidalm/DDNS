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
    public class ClodflareController : ControllerBase {
        private static readonly HttpClient client = new HttpClient ();
        private readonly ILogger<WeatherForecastController> _logger;

        private static List<string> dnsList = new List<string> ();
        private static List<string> services = new List<string> ();

        public ClodflareController (ILogger<WeatherForecastController> logger) {
            _logger = logger;
        }

        [HttpGet]
        public IEnumerable<string> Get () {
            ClodflareDAL dal = new ClodflareDAL ();
            var a = dal.getDnsList ();

            return a.Keys.ToList ();
        }

    }
}