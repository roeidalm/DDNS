using System.Collections.Generic;
using System.Net.Http;
using DDNS.DAL;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace DDNS.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AdministrationController : ControllerBase
    {
        private static readonly HttpClient client = new HttpClient();
        private readonly ILogger<WeatherForecastController> _logger;

        private static List<string> dnsList = new List<string>();
        private static List<string> services = new List<string>();

        public AdministrationController(ILogger<WeatherForecastController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public string Get()
        {
            updateIps();
            ClodflareDAL clodflareDal = new ClodflareDAL();
            var a = clodflareDal.getDnsList();
            var d = JsonConvert.SerializeObject(a);
            return d;
        }
        void updateIps()
        {
            traefikDAL traefikDal = new traefikDAL();
            ClodflareDAL clodflareDal = new ClodflareDAL();
            string publicip = clodflareDal.GetIP();
            var a = traefikDal.ProcessRepositories().Result;
            var b = clodflareDal.getDnsList();

            Dictionary<string, string> updateList = new Dictionary<string, string>();
            List<string> createList = new List<string>();

            foreach (var item in a)
            {
                if (b.ContainsKey(item))
                {
                    if (b[item].IPAddr != publicip)
                    {
                        updateList.Add(item, b[item].ID);
                    }
                }
                else
                {
                    createList.Add(item);
                }
            }
            clodflareDal.updateARecordes(updateList);
            clodflareDal.createNewARecordes(createList);
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