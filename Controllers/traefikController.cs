using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using DDNS.DAL;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace DDNS.Controllers {
    [ApiController]
    [Route ("[controller]")]
    public class traefikController : ControllerBase {
        private static readonly HttpClient client = new HttpClient ();
        private readonly ILogger<WeatherForecastController> _logger;

        private static List<string> dnsList = new List<string> ();
        private static List<string> services = new List<string> ();

        public traefikController (ILogger<WeatherForecastController> logger) {
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