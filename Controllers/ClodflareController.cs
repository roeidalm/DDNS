using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using DDNS.DAL;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace DDNS.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ClodflareController : ControllerBase
    {
        private static readonly HttpClient client = new HttpClient();
        private readonly ILogger<WeatherForecastController> _logger;

        private static List<string> dnsList = new List<string>();
        private static List<string> services = new List<string>();

        public ClodflareController(ILogger<WeatherForecastController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public string Get()
        {
            ClodflareDAL dal = new ClodflareDAL();
            var a = dal.getDnsList();
            var d = JsonConvert.SerializeObject(a);
            return d;
        }

    }
}