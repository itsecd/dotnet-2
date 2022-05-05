using Microsoft.AspNetCore.Mvc;
using Server.Model;
using Telegram.Bot.Types;
using TgBot.Services;

namespace TgBot.Controllers;

public class ChatController : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> Post([FromServices] HandleUpdateService handleUpdateService,
                                          [FromBody] Update update)
    {
        await handleUpdateService.EchoAsync(update);
        return Ok();
    }

    [HttpPost("{name}")]
    public async Task<IActionResult> PostEvent(string name, [FromServices] HandleNotifyService handleNotifyService,
                                               [FromBody] UserEvent userEvent)
    {
        Console.WriteLine(name);
        await handleNotifyService.EchoAsync(userEvent);
        return Ok();
    }
}
