////////////////////////////////////////////////
// © https://github.com/badhitman - @fakegov 
////////////////////////////////////////////////

using MetaLib.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Swashbuckle.AspNetCore.Annotations;

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

        /// <summary>
        /// Получить настройки клиента Blazor
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ClientConfigModel Get()
        {
            return (ClientConfigModel)_config.Value;
        }
    }
}
