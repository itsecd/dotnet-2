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

    public HandleUpdateService(ITelegramBotClient botClient, ILogger<HandleUpdateService> logger)
    {
        _botClient = botClient;
        _logger = logger;
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
            "/signIn"   => AddUser(_botClient, message),
            "/enable"   => SetEnableMode(_botClient, message),
            "/disable"  => SetDisableMode(_botClient, message),
            "/signOut"  => DeleteUser(_botClient, message),
            "/test"     => TestRequest(_botClient, message),
            _           => Usage(_botClient, message)
        };
        Message sentMessage = await action;
        _logger.LogInformation("The message was sent with id: {sentMessageId}",sentMessage.MessageId);
    }

    static async Task<Message> AddUser(ITelegramBotClient bot, Message message)
    {

        await bot.SendChatActionAsync(message.Chat.Id, ChatAction.Typing);
        Server.Model.User user = new();
        user.Name = message.Chat.Username;
        user.ChatId = message.Chat.Id;
        user.Toggle = false;
        var client = new HttpClient();
        var response = await client.PostAsync($"https://localhost:44349/api/User", new StringContent(JsonConvert.SerializeObject(user), Encoding.UTF8, "application/json"));
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

    static async Task<Message> TestRequest(ITelegramBotClient bot, Message message)
    {
        await bot.SendChatActionAsync(message.Chat.Id, ChatAction.Typing);
        var userEvent = new Server.Model.UserEvent
        {
            Id = 1,
            User = new Server.Model.User()
        };
        userEvent.User.Id = 1;
        userEvent.User.ChatId = message.Chat.Id;
        userEvent.User.Name = message.Chat.Username;
        userEvent.User.Toggle = true;
        userEvent.EventName = "VAM POSILKA";
        userEvent.DateNTime = DateTime.Now;
        var client = new HttpClient();
        var response = await client.PostAsync($"https://localhost:443/send", new StringContent(JsonConvert.SerializeObject(userEvent), Encoding.UTF8, "application/json"));
        return await bot.SendTextMessageAsync(chatId: message.Chat.Id,
                                                  text: "a");
    }

    static async Task<Message> SetEnableMode(ITelegramBotClient bot, Message message)
    {
        await bot.SendChatActionAsync(message.Chat.Id, ChatAction.Typing);
        var client = new HttpClient();
        var response = await client.GetAsync("https://localhost:44349/api/User");
        HttpResponseMessage putResponse;
        if (response.IsSuccessStatusCode)
        {
            List<Server.Model.User> users = JsonConvert.DeserializeObject<List<Server.Model.User>>(await response.Content.ReadAsStringAsync())!;
            if (users.Exists(user => user.Name.Equals(message.Chat.Username)))
            {
                Server.Model.User user = users.Single(user => user.Name == message.Chat.Username);
                user.Toggle = true;
                putResponse = await client.PutAsync($"https://localhost:44349/api/User/{user.Id}", new StringContent(JsonConvert.SerializeObject(user), Encoding.UTF8, "application/json"));
                if (putResponse.IsSuccessStatusCode)
                {
                    string str = $"Success";
                    return await bot.SendTextMessageAsync(chatId: message.Chat.Id,
                                                          text: str);
                }
                else
                {
                    string str = $"Error {putResponse.StatusCode}";
                    return await bot.SendTextMessageAsync(chatId: message.Chat.Id,
                                                          text: str);
                }
            }
            else
            {
                string str = "You are not registered";
                return await bot.SendTextMessageAsync(chatId: message.Chat.Id,
                                                      text: str);
            }
        }
        else
        {
            string str = $"Error {response.StatusCode}";
            return await bot.SendTextMessageAsync(chatId: message.Chat.Id,
                                                  text: str);
        }

    }

    static async Task<Message> SetDisableMode(ITelegramBotClient bot, Message message)
    {
        await bot.SendChatActionAsync(message.Chat.Id, ChatAction.Typing);
        var client = new HttpClient();
        var response = await client.GetAsync("https://localhost:44349/api/User");
        HttpResponseMessage putResponse;
        if (response.IsSuccessStatusCode)
        {
            List<Server.Model.User> users = JsonConvert.DeserializeObject<List<Server.Model.User>>(await response.Content.ReadAsStringAsync())!;
            if (users.Exists(user => user.Name.Equals(message.Chat.Username)))
            {
                Server.Model.User user = users.Single(user => user.Name == message.Chat.Username);
                user.Toggle = false;
                putResponse = await client.PutAsync($"https://localhost:44349/api/User/{user.Id}", new StringContent(JsonConvert.SerializeObject(user), Encoding.UTF8, "application/json"));
                if (putResponse.IsSuccessStatusCode)
                {
                    string str = $"Success";
                    return await bot.SendTextMessageAsync(chatId: message.Chat.Id,
                                                          text: str);
                }
                else
                {
                    string str = $"Error {putResponse.StatusCode}";
                    return await bot.SendTextMessageAsync(chatId: message.Chat.Id,
                                                          text: str);
                }
            }
            else
            {
                string str = "You are not registered";
                return await bot.SendTextMessageAsync(chatId: message.Chat.Id,
                                                      text: str);
            }
        }
        else
        {
            string str = $"Error {response.StatusCode}";
            return await bot.SendTextMessageAsync(chatId: message.Chat.Id,
                                                  text: str);
        }

    }

    static async Task<Message> DeleteUser(ITelegramBotClient bot, Message message)
    {
        await bot.SendChatActionAsync(message.Chat.Id, ChatAction.Typing);
        var client = new HttpClient();
        var response = await client.GetAsync("https://localhost:44349/api/User");
        HttpResponseMessage deleteResponse;
        if (response.IsSuccessStatusCode)
        {
            List<Server.Model.User> users = JsonConvert.DeserializeObject<List<Server.Model.User>>(await response.Content.ReadAsStringAsync())!;
            if (users.Exists(user => user.Name.Equals(message.Chat.Username)))
            {
                Server.Model.User user = users.Single(user => user.Name == message.Chat.Username);
                deleteResponse = await client.DeleteAsync($"https://localhost:44349/api/User/{user.Id}");
                if (deleteResponse.IsSuccessStatusCode)
                {
                    string str = $"Success";
                    return await bot.SendTextMessageAsync(chatId: message.Chat.Id,
                                                          text: str);
                }
                else
                {
                    string str = $"Error {deleteResponse.StatusCode}";
                    return await bot.SendTextMessageAsync(chatId: message.Chat.Id,
                                                          text: str);
                }
            }
            else
            {
                string str = "You are not registered";
                return await bot.SendTextMessageAsync(chatId: message.Chat.Id,
                                                      text: str);
            }
        }
        else
        {
            string str = $"Error {response.StatusCode}";
            return await bot.SendTextMessageAsync(chatId: message.Chat.Id,
                                                  text: str);
        }
    }

    static async Task<Message> Usage(ITelegramBotClient bot, Message message)
    {
        const string usage = "Usage:\n" +
                             "/signIn    -  get a confirmation code to log in\n" +
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

    public Task HandleErrorAsync(Exception exception)
    {
        var errorMessage = exception switch
        {
            ApiRequestException apiRequestException => $"Telegram API Error:\n[{apiRequestException.ErrorCode}]\n{apiRequestException.Message}",
            _ => exception.ToString()
        };

        _logger.LogInformation($"HandleError: {errorMessage}", errorMessage);
        return Task.CompletedTask;
    }
}
