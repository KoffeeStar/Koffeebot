using Koffeebot.Dtos;
using Microsoft.AspNetCore.Mvc;
using Koffeebot.Commands;

namespace Koffeebot.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UpdateController : ControllerBase
{
    private readonly TelegramUpdateProcessor _processor;

    public UpdateController(TelegramUpdateProcessor processor)
    {
        _processor = processor;
    }

    [HttpGet]
    public IActionResult Get()
    {
        return Ok("Hello world!");
    }

    [HttpPost]
    public async Task<IActionResult> Post([FromBody] TelegramUpdate update)
    {
        Console.WriteLine($"[WEBHOOK] Received message: {update?.Message?.Text}");

        if (!ModelState.IsValid)
            return BadRequest();

        await _processor.HandleAsync(update);

        return Ok();
    }
}