using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;
using System.Numerics;

namespace ServiceApplication.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class HomeController : Controller
    {
        private readonly IHttpClientFactory _clientFactory;

        public HomeController(IHttpClientFactory clientFactory)
        {
            _clientFactory = clientFactory;
        }

        [HttpGet("token")]
        public async Task<IActionResult> Index()
        {
            var request = new HttpRequestMessage(HttpMethod.Get, "https://localhost:7248/weatherforecast/");
            var client = _clientFactory.CreateClient();
            HttpResponseMessage response = await client.SendAsync(request);

            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                string token = await response.Content.ReadAsStringAsync();
                HttpContext.Session.SetString("JwtToken", token);
            }

            return Ok();
        }

        [HttpGet("roles")]
        public async Task<List<string>> GetAllRoles()
        {
            var accessToken = HttpContext.Session.GetString("JwtToken");
            List<string> roles = new List<string>();
            var request = new HttpRequestMessage(HttpMethod.Get, "https://localhost:7268/api/AuthO");

            var client = _clientFactory.CreateClient();

            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            HttpResponseMessage response = await client.SendAsync(request, HttpCompletionOption.ResponseHeadersRead);

            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                roles = await response.Content.ReadFromJsonAsync<List<string>>();
            }

            return (roles);
        }
    }
}