using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types.Enums;

namespace TelegramBotServer
{
    public class SetWebHookService : IHostedService
    {
        private IConfiguration _config;
        private readonly ILogger<SetWebHookService> _logger;

        public SetWebHookService(IConfiguration config, ILogger<SetWebHookService> logger)
        {
            _config = config;
            _logger = logger;
        }
        public Task StartAsync(CancellationToken cancellationToken)
        {
            ITelegramBotClient bot = new TelegramBotClient($"{_config["BotToken"]}");
            var webhookAddress = $"{_config["WebHookURL"]}/bot/{_config["BotToken"]}";

            return bot.SetWebhookAsync(
                url: webhookAddress,
                allowedUpdates: Array.Empty<UpdateType>(),
                cancellationToken: cancellationToken);
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Set WebHook Service is stopping.");

            return Task.CompletedTask;
        }
    }
}
