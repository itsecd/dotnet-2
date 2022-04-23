using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using TelegramBotServer.Services;

namespace TelegramBotServer.Controllers
{
    public class WebhookController : ControllerBase
    {

        [HttpPost]
        public async Task<IActionResult> Post([FromServices] CommandHandlerService commandHandler, [FromBody] Update update)
        {
            await commandHandler.ProcessUpdate(update);
            return Ok();
        }
    }
}
