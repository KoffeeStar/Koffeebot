using Koffeebot.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Koffeebot.Repositories.Interfaces;

namespace Koffeebot.Controllers
{
    [ApiController]
    [Route("api/chat")]
    public class ChatTestController : ControllerBase
    {
        private readonly IChatApiClient _chatApiClient;

        public ChatTestController(IChatApiClient chatApiClient)
        {
            _chatApiClient = chatApiClient;
        }

        [HttpPost("test")]
        public async Task<IActionResult> Test([FromBody] ChatTestRequest request)
        {
            var answer = await _chatApiClient.SendMessageAsync(request.Message, new List<Koffeebot.Repositories.Models.OpenApiResponse.Message>()); return Ok(new { answer });
        }
    }

    public record ChatTestRequest(string Message);
}