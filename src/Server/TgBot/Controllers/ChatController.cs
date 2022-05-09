using Microsoft.AspNetCore.Mvc;
using Server.Model;
using Telegram.Bot.Types;
using TgBot.Services;

namespace TgBot.Controllers;

public class ChatController : ControllerBase
{
    [HttpPost("bot/{any}")]
    public async Task<IActionResult> Post([FromServices] HandleUpdateService handleUpdateService,
                                          [FromBody] Update update)
    {
        await handleUpdateService.EchoAsync(update);
        return Ok();
    }

    [HttpPost("send/{any}")]
    public async Task<IActionResult> PostEvent([FromServices] HandleNotifyService handleNotifyService,
                                               [FromBody] UserEvent userEvent)
    {
        await handleNotifyService.EchoAsync(userEvent);
        return Ok();
    }


}
