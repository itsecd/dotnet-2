using Server.Model;

namespace Client.Models
{
    public static class EventExtensions
    {
        public static string ToFullInfo(this UserEvent userEvent)
        {
            return $"{userEvent.EventName} {userEvent.DateNTime:dd.MM.yyyy-HH:mm} every {userEvent.EventFrequency} days";
        }
    }
}
