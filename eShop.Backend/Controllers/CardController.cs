using eShop.Backend.Domain;
using eShop.Backend.Services;
using Microsoft.AspNetCore.Mvc;

namespace eShop.Backend.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CarduriController(CarduriService service) : ControllerBase
    {
        private readonly CarduriService _service = service;

        [HttpGet("client/{clientId}")]
        public ActionResult<List<Card>> GetCarduriClient(int clientId)
        {
            return Ok(_service.GetCarduri(clientId));
        }

        [HttpGet("detail/{clientid}/{serie}")]
        public ActionResult<Client> GetCardClient(int clientid, string serie)
        {
            return Ok(_service.GetCarduri(clientid, serie).FirstOrDefault());
        }
        [HttpGet("serie/{serie}")]
        public ActionResult<Client> GetCardClient(string serie)
        {
            return Ok(_service.GetCarduri(serie: serie).FirstOrDefault());
        }

        [HttpGet("istoric/{serie}")]
        public ActionResult<List<EvolutieCard>> GetIstoricCard(string serie)
        {
            return Ok(_service.GetEvolutieCard(serie: serie));
        }

        [HttpGet("interval-max/{serie}")]
        public ActionResult<IstoricCard> IntervalMax(string serie)
        {
            return Ok(_service.GetIntervalMaximCard(serie: serie));
        }

        [HttpGet("stare/{serie}/{timestamp}")]
        public ActionResult<Card> GetStareLaMoment(string serie,DateTime timestamp)
        {
            return Ok(_service.GetCardLaMoment(serie: serie,timestamp));
        }

        [HttpPost("{clientId}")]
        public ActionResult<Client> InsertCard([FromBody] Card card, int clientId)
        {
            return Accepted(_service.InsertCard(card,clientId));
        }
        [HttpPut()]
        public ActionResult<Client> UpdateCard([FromBody] Card card)
        {
            _service.UpdateCard(card);
            return Accepted();
        }
    }
}
