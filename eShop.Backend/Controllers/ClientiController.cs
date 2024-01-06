using eShop.Backend.Domain;
using eShop.Backend.Services;
using Microsoft.AspNetCore.Mvc;

namespace eShop.Backend.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ClientiController(ClientiService service) : ControllerBase
    {
        private readonly ClientiService _service = service;

        [HttpGet("")]
        public ActionResult<List<Client>> GetClienti()
        {
            return Ok(_service.GetClienti());
        }

        [HttpGet("{id}")]
        public ActionResult<Client> GetClientiById(int id)
        {
            return Ok(_service.GetClienti(id).FirstOrDefault());
        }

        [HttpPost()]
        public ActionResult<Client> InsertClient([FromBody] Client client)
        {
            return Accepted(_service.InsertClient(client));
        }
        [HttpPut()]
        public ActionResult<Client> UpdateClient([FromBody] Client client)
        {
            _service.UpdateClient(client);
            return Accepted();
        }
    }
}
