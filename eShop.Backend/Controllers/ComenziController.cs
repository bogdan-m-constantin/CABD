using eShop.Backend.Domain;
using eShop.Backend.Services;
using Microsoft.AspNetCore.Mvc;

namespace eShop.Backend.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ComenziController(ComenziService service) : ControllerBase
    {
        private readonly ComenziService _service = service;

        [HttpGet("{clientid}/{start}/{end}")]
        public ActionResult<List<Comanda>> GetComenziByClient(int clientid, DateTime start, DateTime end)
        {
            return Ok(_service.GetComenzi(clientid,start,end));
        }

        [HttpGet("{id}")]
        public ActionResult<Comanda> GetComandaById(int id)
        {
            return Ok(_service.GetComenzi(comandaId: id).FirstOrDefault());
        }

        [HttpPost()]
        public ActionResult<Comanda> InsertComanda([FromBody] Comanda comanda)
        {
            return Accepted(_service.InsertComanda(comanda));
        }
       
    }
}
