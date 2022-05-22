using Grpc.Core;
using System.Threading.Tasks;
using TelegramBot.Proto;
using TelegramBot.Model;
using System.Collections.Generic;
using Google.Protobuf.WellKnownTypes;
using TelegramBot.Repository;
using Microsoft.Extensions.Logging;

namespace TelegramBot.Services
{
    public class TelegramEventService: Proto.TelegramEventService.TelegramEventServiceBase
    {
        private readonly IUsersRepository _usersRepository;
        private readonly ILogger<TelegramEventService> _logger;

        public TelegramEventService(IUsersRepository usersRepository, ILogger<TelegramEventService> logger)
        {
            _logger = logger;
            _usersRepository = usersRepository;
        }
        public override Task<ReminderOperationResponse> AddReminder(Reminder request, ServerCallContext context)
        {
            var reminder = new EventReminder(
                request.Id,
                request.Name,
                request.Description,
                request.DateTime.ToDateTime(),
                request.RepeatPeriod);
                _usersRepository.AddEventReminder(request.UserId, reminder);
            _logger.LogTrace($"Add new reminder for User {request.UserId}");
            return Task.FromResult(new ReminderOperationResponse { UserId = request.UserId, Result = true });
        }

        public override Task<ReminderOperationResponse> ChangeReminder(Reminder request, ServerCallContext context)
        {
            var reminder = new EventReminder(
                request.Id,
                request.Name,
                request.Description,
                request.DateTime.ToDateTime(),
                request.RepeatPeriod);
            _usersRepository.ChangeEventReminder(request.UserId, request.Id, reminder);
            _logger.LogTrace($"Change reminder {request.Id} of User {request.UserId}");
            return Task.FromResult(new ReminderOperationResponse { UserId = request.UserId, Result = true });
        }

        public override Task<ReminderOperationResponse> RemoveReminder(Reminder request, ServerCallContext context)
        {
            _usersRepository.RemoveEventReminder(request.UserId, request.Id);
            _logger.LogTrace($"Remove reminder {request.Id} of User {request.UserId}");
            return Task.FromResult(new ReminderOperationResponse { UserId = request.UserId, Result = true });
        }

        public override Task<UserResponse> GetReminders(UserRequest request, ServerCallContext context)
        {
            var user = _usersRepository.FindUser(request.UserId);
            var reminders = new List<Reminder>();
            foreach(var eventReminder in user.EventReminders)
            {
                reminders.Add(new Reminder
                {
                    Id = eventReminder.Id,
                    Name = eventReminder.Name,
                    Description = eventReminder.Description,
                    DateTime = Timestamp.FromDateTime(eventReminder.Time),
                    RepeatPeriod = eventReminder.RepeatPeriod,
                    UserId = request.UserId
                });
            }
            var response = new UserResponse();
            response.Reminders.AddRange(reminders);
            _logger.LogTrace($"Send reminders to User {request.UserId}");
            return Task.FromResult(response);
        }
    }
}
