using System.Text;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;
using Newtonsoft.Json;
using Telegram.Bot;
using System.Net;

namespace TgBot.Services;

public class HandleUpdateService
{
    private readonly ITelegramBotClient _botClient;
    private readonly ILogger<HandleUpdateService> _logger;
    private static string? _serverAddress;

    public HandleUpdateService(ITelegramBotClient botClient, ILogger<HandleUpdateService> logger, IConfiguration configuration)
    {
        _botClient = botClient;
        _logger = logger;
        _serverAddress = configuration.GetSection("HostConfiguration").Get<HostConfiguration>().ServerAddress;
    }

    public async Task EchoAsync(Update update)
    {
        var handler = update.Type switch
        {
            UpdateType.Message            => BotOnMessageReceived(update.Message!),
            UpdateType.EditedMessage      => BotOnMessageReceived(update.EditedMessage!),
            _                             => UnknownUpdateHandlerAsync(update)
        };

        try
        {
            await handler;
        }
        catch (Exception exception)
        {
            await HandleErrorAsync(exception);
        }
    }

    private async Task BotOnMessageReceived(Message message)
    {
        _logger.LogInformation("Receive message type: {messageType}", message.Type);
        if (message.Type != MessageType.Text)
            return;

        var action = message.Text!.Split(' ')[0] switch
        {
            "/signUp"   => AddUser(_botClient, message),
            "/getCode"  => SendConfirmationCode(_botClient, message),
            "/enable"   => SetEnableMode(_botClient, message),
            "/disable"  => SetDisableMode(_botClient, message),
            "/signOut"  => DeleteUser(_botClient, message),
            _           => Usage(_botClient, message)
        };
        Message sentMessage = await action;
        _logger.LogInformation("The message was sent with id: {sentMessageId}",sentMessage.MessageId);
    }

    private static async Task<Message> AddUser(ITelegramBotClient bot, Message message)
    {
        await bot.SendChatActionAsync(message.Chat.Id, ChatAction.Typing);
        var user = new Server.Model.User
        {
            Name = message.Chat.Username,
            ChatId = message.Chat.Id,
            Toggle = false
        };
        var client = new HttpClient();
        var response = await client.PostAsync($"{_serverAddress}/api/User", new StringContent(JsonConvert.SerializeObject(user), Encoding.UTF8, "application/json"));
        if (!response.IsSuccessStatusCode)
        {
            if (response.StatusCode == HttpStatusCode.Conflict)
            {
                return await bot.SendTextMessageAsync(chatId: message.Chat.Id,
                                                  text: "Error\nYou are already registered");
            }
        }
        return await bot.SendTextMessageAsync(chatId: message.Chat.Id,
                                                  text: "Success");
    }
    
    private static string GetCode(string userName)
    {
        var key = "12341234123412341234123412341234".ToCharArray();
        var strUserName = userName.ToCharArray();
        string code = "";
        for(int i = 0; i < strUserName.Length; i++)
        {
            code += strUserName[i] ^ key[i];
        }
        return code;
    }

    private static async Task<Message> SendConfirmationCode(ITelegramBotClient bot, Message message)
    {
        return await bot.SendTextMessageAsync(chatId: message.Chat.Id,
                                                          text: GetCode(message.Chat.Username!));
    }

    private static async Task<Message> SetEnableMode(ITelegramBotClient bot, Message message)
    {
        await bot.SendChatActionAsync(message.Chat.Id, ChatAction.Typing);
        var client = new HttpClient();
        var response = await client.GetAsync($"{_serverAddress}/api/User");
        if (response.IsSuccessStatusCode)
        {
            var users = JsonConvert.DeserializeObject<List<Server.Model.User>>(await response.Content.ReadAsStringAsync())!;
            if (users.Exists(user => user.Name.Equals(message.Chat.Username)))
            {
                var user = users.Single(user => user.Name == message.Chat.Username);
                user.Toggle = true;
                var putResponse = await client.PutAsync($"{_serverAddress}/api/User/{user.Id}", new StringContent(JsonConvert.SerializeObject(user), Encoding.UTF8, "application/json"));
                if (putResponse.IsSuccessStatusCode)
                {
                    return await bot.SendTextMessageAsync(chatId: message.Chat.Id,
                                                          text: "Success");
                }
                else
                {
                    return await bot.SendTextMessageAsync(chatId: message.Chat.Id,
                                                          text: $"Error {putResponse.StatusCode}");
                }
            }
            else
            {
                return await bot.SendTextMessageAsync(chatId: message.Chat.Id,
                                                      text: "You are not registered");
            }
        }
        else
        {
            return await bot.SendTextMessageAsync(chatId: message.Chat.Id,
                                                  text: $"Error {response.StatusCode}");
        }
    }

    private static async Task<Message> SetDisableMode(ITelegramBotClient bot, Message message)
    {
        await bot.SendChatActionAsync(message.Chat.Id, ChatAction.Typing);
        var client = new HttpClient();
        var response = await client.GetAsync($"{_serverAddress}/api/User");
        if (response.IsSuccessStatusCode)
        {
            var users = JsonConvert.DeserializeObject<List<Server.Model.User>>(await response.Content.ReadAsStringAsync())!;
            if (users.Exists(user => user.Name.Equals(message.Chat.Username)))
            {
                var user = users.Single(user => user.Name == message.Chat.Username);
                user.Toggle = false;
                var putResponse = await client.PutAsync($"{_serverAddress}/api/User/{user.Id}", new StringContent(JsonConvert.SerializeObject(user), Encoding.UTF8, "application/json"));
                if (putResponse.IsSuccessStatusCode)
                {
                    return await bot.SendTextMessageAsync(chatId: message.Chat.Id,
                                                          text: "Success");
                }
                else
                {
                    return await bot.SendTextMessageAsync(chatId: message.Chat.Id,
                                                          text: $"Error {putResponse.StatusCode}");
                }
            }
            else
            {
                return await bot.SendTextMessageAsync(chatId: message.Chat.Id,
                                                      text: "You are not registered");
            }
        }
        else
        {
            return await bot.SendTextMessageAsync(chatId: message.Chat.Id,
                                                  text: $"Error {response.StatusCode}");
        }
    }

    private static async Task<Message> DeleteUser(ITelegramBotClient bot, Message message)
    {
        await bot.SendChatActionAsync(message.Chat.Id, ChatAction.Typing);
        var client = new HttpClient();
        var response = await client.GetAsync($"{_serverAddress}/api/User");
        if (response.IsSuccessStatusCode)
        {
            var users = JsonConvert.DeserializeObject<List<Server.Model.User>>(await response.Content.ReadAsStringAsync())!;
            if (users.Exists(user => user.Name.Equals(message.Chat.Username)))
            {
                var user = users.Single(user => user.Name == message.Chat.Username);
                var deleteResponse = await client.DeleteAsync($"{_serverAddress}/api/User/{user.Id}");
                if (deleteResponse.IsSuccessStatusCode)
                {
                    return await bot.SendTextMessageAsync(chatId: message.Chat.Id,
                                                          text: "Success");
                }
                else
                {
                    return await bot.SendTextMessageAsync(chatId: message.Chat.Id,
                                                          text: $"Error {deleteResponse.StatusCode}");
                }
            }
            else
            {
                return await bot.SendTextMessageAsync(chatId: message.Chat.Id,
                                                      text: "You are not registered");
            }
        }
        else
        {
            return await bot.SendTextMessageAsync(chatId: message.Chat.Id,
                                                  text: $"Error {response.StatusCode}");
        }
    }

    private static async Task<Message> Usage(ITelegramBotClient bot, Message message)
    {
        const string usage = "Usage:\n" +
                             "/signUp    -  sign up\n" +
                             "/getCode   -  get a confirmation code to log in to the app\n" +
                             "/enable    -  enable alerts\n" +
                             "/disable   -  disable alerts\n" +
                             "/signOut   -  delete user";
        return await bot.SendTextMessageAsync(chatId: message.Chat.Id,
                                              text: usage,
                                              replyMarkup: new ReplyKeyboardRemove());
    }

    private Task UnknownUpdateHandlerAsync(Update update)
    {
        _logger.LogInformation("Unknown update type: {updateType}", update.Type);
        return Task.CompletedTask;
    }

    private Task HandleErrorAsync(Exception exception)
    {
        var errorMessage = exception switch
        {
            ApiRequestException apiRequestException => $"Telegram API Error:\n[{apiRequestException.ErrorCode}]\n{apiRequestException.Message}",
            _ => exception.ToString()
        };

        _logger.LogInformation($"HandleError: {errorMessage}");
        return Task.CompletedTask;
    }
}
