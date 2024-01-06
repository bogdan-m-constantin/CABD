using eShop.Backend.Domain;
using eShop.Backend.Services;
using Microsoft.AspNetCore.Mvc;

namespace eShop.Backend.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AdreseController(AdreseService service) : ControllerBase
    {
        private readonly AdreseService _service = service;

        [HttpGet("{client}")]
        public ActionResult<List<Adresa>> GetAdrese(int client)
        {
            return Ok(_service.GetAdrese(client));
        }

        [HttpPost("{client}")]
        public ActionResult<Produs> InsertAdresa([FromBody] Adresa adresa, int client)
        {
            return Accepted(_service.InsertAdresa(adresa, client));
        }
        [HttpPut("")]
        public ActionResult<Produs> UpdateAdresa([FromBody] Adresa adresa)
        {
            _service.UpdateAdresa(adresa);
            return Accepted();
        }
    }
}
