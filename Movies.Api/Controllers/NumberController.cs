using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Net.Http;
using System.Threading.Tasks;

namespace Movies.Api.Controllers
{
    public class NumberController : BaseController
    {
        private readonly string _baseUrl;

        public NumberController(IConfiguration configuration, ILogger<NumberController> logger, IMapper mapper, IHttpContextAccessor httpContextAccessor)
            : base(logger, mapper, httpContextAccessor)
        {
            _baseUrl = configuration.GetSection("AppSettings:NumberApi:BaseUrl").Value;
        }

        [HttpGet]
        public async Task<ActionResult<int>> GetNumber()
        {
            using var client = new HttpClient();
            var response = await client.GetAsync($"{_baseUrl}randomnumber");
            var value = (await response.Content.ReadAsStringAsync())
                .Replace("[", string.Empty)
                .Replace("]", string.Empty);

            return Ok(int.Parse(value));
        }
    }
}
