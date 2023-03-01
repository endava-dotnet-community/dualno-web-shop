using Core.Abstractions.Services;
using Microsoft.AspNetCore.Mvc;

namespace WebShop.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class InfoController : WebShopBaseController
    {
        private readonly ILogger<InfoController> _logger;

        public InfoController(ILogger<InfoController> logger, IUsersService usersService) 
            : base(usersService)
        {
            _logger = logger;
        }

        [HttpGet]
        public string Version()
        {
            return "Web shop is working! ver 1.0";
        }
    }
}