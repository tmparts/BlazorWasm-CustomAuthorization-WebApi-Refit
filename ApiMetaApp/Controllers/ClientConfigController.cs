////////////////////////////////////////////////
// © https://github.com/badhitman - @fakegov 
////////////////////////////////////////////////

using LibMetaApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace ApiMetaApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClientConfigController : ControllerBase
    {
        readonly IOptions<ServerConfigModel> _config;
        
        public ClientConfigController(IOptions<ServerConfigModel> set_config)
        {
            _config = set_config;
        }

        // GET: api/<ClientConfigController>
        [HttpGet]
        public ClientConfigModel Get()
        {
            return (ClientConfigModel)_config.Value;
        }
    }
}
