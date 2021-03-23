using BettingLine.Service.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BettingLine.Service.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OutcomesController : ControllerBase
    {
        private readonly IOutcomeRepository _outcomeRepository;
        private readonly ILogger<OutcomesController> _logger;

        public OutcomesController(IOutcomeRepository outcomeRepository, ILogger<OutcomesController> logger)
        {
            _outcomeRepository = outcomeRepository;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var outcomes = await _outcomeRepository.GetOutcomesAsync();
            return Ok(outcomes);
        }

        [HttpPost("Subscibe")]
        public IActionResult Subscibe([FromBody] string serviceName)
        {
            return BadSubscibeRequest();
        }


        [HttpPost("Unsubscibe")]
        public IActionResult Unsubscibe([FromBody] string serviceName)
        {
            return BadSubscibeRequest();
        }

        private IActionResult BadSubscibeRequest()
        {
            var message = "Use rabbitmq to receive changes";
            _logger.LogInformation(message);
            ModelState.AddModelError("Error", message);
            return BadRequest(ModelState);
        }


    }
}
