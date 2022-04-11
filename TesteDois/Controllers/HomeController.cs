using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using TesteDois.Models;
using System.Net;
using Newtonsoft.Json;

namespace TesteDois.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            string apikeys = "fed547adcc56e6160550d8f8dfb433c9aa2e12ae2bde93816b2e110297adc912";

            var clientIpClient = new HttpClient();

            string clienteIp = "https://ipv4.icanhazip.com/";
            var responseClienteIp = clientIpClient.GetStringAsync(new Uri(clienteIp)).Result;

            var ipinfo = new HttpClient();
            var u = $"https://api.ipinfodb.com/v3/ip-city/?key={apikeys}&ip={responseClienteIp}&format=json";
            var resp = ipinfo.GetStringAsync(new Uri(u)).Result;

            IpData i = JsonConvert.DeserializeObject<IpData>(resp);

            var lat = i.latitude;
            var longi = i.longitude;

            var url = $"https://api.openweathermap.org/data/2.5/weather?lat={lat}&lon={longi}&units=metric&appid=7a095dea56af62a3afb5b2c732b0808c";

            var client = new HttpClient();

            client.Timeout = TimeSpan.FromMilliseconds(2000);
            var response = client.GetStringAsync(new Uri(url)).Result;
            ViewBag.result = response;
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}