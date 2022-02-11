using LibMetaApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

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

        // GET api/<ClientConfigController>/5
        /*[HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<ClientConfigController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<ClientConfigController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<ClientConfigController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }*/
    }
}
