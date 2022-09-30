using Microsoft.AspNetCore.Mvc;
using ServiceBusAPI.Messages.Sender;
using ServiceBusAPI.Models;
using System.ComponentModel.DataAnnotations;
using System.Text.Json;

namespace ServiceBusAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotificationController : ControllerBase
    {
        private readonly IServicebusSenderService _serviceBusSenderService;
        private readonly IConfiguration _configuration;
        private readonly ILogger _logger;
        public NotificationController(
            IServicebusSenderService serviceBusSenderService,
            IConfiguration configuration,
            ILogger<NotificationController> logger)
        {
            _serviceBusSenderService = serviceBusSenderService;
            _configuration = configuration;
            _logger = logger;
        }

        [HttpPost]
        [ProducesResponseType(typeof(Notification), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Notification), StatusCodes.Status409Conflict)]
        public async Task<IActionResult> Create([FromBody][Required] Notification notificationPayload)
        {

            string messagePayload = JsonSerializer.Serialize(notificationPayload);

            // Send this to the bus for the other services
            await _serviceBusSenderService.SendMessage(_configuration["ConnectionStrings:QueueName"], messagePayload);

            return Ok(notificationPayload);
        }
    }
}
