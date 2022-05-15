using Grpc.Core;
using System.Threading.Tasks;
using TelegramBot.Proto;
using TelegramBot.Model;
using System.Collections.Generic;
using Google.Protobuf.WellKnownTypes;
using TelegramBot.Repository;

namespace TelegramBot.Services
{
    public class TelegramEventService: Proto.TelegramEventService.TelegramEventServiceBase
    {
        private readonly IUsersRepository _usersRepository;

        public TelegramEventService(IUsersRepository usersRepository)
        {
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
            return Task.FromResult(new ReminderOperationResponse { UserId = request.UserId, Result = true });
        }

        public override Task<ReminderOperationResponse> RemoveReminder(Reminder request, ServerCallContext context)
        {
            _usersRepository.RemoveEventReminder(request.UserId, request.Id);
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
            return Task.FromResult(response);
        }
    }
}
