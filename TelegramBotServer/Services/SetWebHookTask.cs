using Microsoft.Extensions.Configuration;
using System;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types.Enums;

namespace TelegramBotServer
{
    public class SetWebHookTask : IStartupTask
    {
        IConfiguration _config;

        public SetWebHookTask(IConfiguration config)
        {
            _config = config;
        }
        public Task Execute()
        {
            ITelegramBotClient bot = new TelegramBotClient($"{_config["BotToken"]}");
            var webhookAddress = $"{_config["WebHookURL"]}/bot/{_config["BotToken"]}";

            var cts = new CancellationTokenSource();
            var cancellationToken = cts.Token;

            return bot.SetWebhookAsync(
                url: webhookAddress,
                allowedUpdates: Array.Empty<UpdateType>(),
                cancellationToken: cancellationToken);
        }
    }
}
