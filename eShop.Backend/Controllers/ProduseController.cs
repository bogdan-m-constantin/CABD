using eShop.Backend.Domain;
using eShop.Backend.Services;
using Microsoft.AspNetCore.Mvc;

namespace eShop.Backend.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProduseController(ProduseService service) : ControllerBase
    {
        private readonly ProduseService _service = service;

        [HttpGet("")]
        public ActionResult<List<Produs>> GetProduse()
        {
            return Ok(_service.GetProduse());
        }

        [HttpGet("{id}")]
        public ActionResult<Produs> GetProduseById(int id)
        {
            return Ok(_service.GetProduse(id).FirstOrDefault());
        }

        [HttpPost()]
        public ActionResult<Produs> InsertProdus([FromBody] Produs produs)
        {
            return Accepted(_service.InsertProdus(produs));
        }
        [HttpPut()]
        public ActionResult<Produs> UpdateProdus([FromBody] Produs produs)
        {
            _service.UpdateProdus(produs);
            return Accepted();
        }
    }
}
