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

    }
}