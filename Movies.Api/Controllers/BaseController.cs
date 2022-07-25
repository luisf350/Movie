using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Security.Claims;

namespace Movies.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class BaseController : ControllerBase
    {
        public IHttpContextAccessor HttpContextAccessor { get; }

        protected readonly ILogger Logger;
        protected readonly IMapper Mapper;
        protected readonly Guid UserId;


        public BaseController(ILogger logger, IMapper mapper, IHttpContextAccessor httpContextAccessor)
        {
            Logger = logger;
            Mapper = mapper;
            HttpContextAccessor = httpContextAccessor;
            
            if (Guid.TryParse(HttpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value, out var newUserId))
                UserId = newUserId;
        }
    }
}
