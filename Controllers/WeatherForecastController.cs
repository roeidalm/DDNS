﻿using System.Collections.Generic;
using System.Net.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

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

            return new List<string> () { "value 1", "value 2" };
        }

    }
}