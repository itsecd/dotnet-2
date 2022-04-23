using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace TelegramBotServer.Controllers
{
    public class WebhookController : ControllerBase
    {

        [HttpPost]
        public async Task<IActionResult> Post([FromServices] ITelegramBotClient bot, [FromBody] Update update)
        {
            var cns = new CancellationToken();

            Message message = await bot.SendTextMessageAsync(
                chatId: update.Message.Chat.Id,
                text: "Hello, World!",
                cancellationToken: cns);
            return Ok();
        }
    }
}
