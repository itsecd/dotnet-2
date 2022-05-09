using System;

namespace Server.Model
{
    public class UserEvent
    {
        public int Id { get; set; }
        public User User { get; set; }
        public string EventName { get; set; }
        public DateTime DateNTime { get; set; }
        public int EventFrequency { get; set; }

        public override bool Equals(object obj)
        {
            if (obj is UserEvent userEvent)
            {
                return userEvent.User.Equals(User) &&
                    userEvent.EventName == EventName &&
                    userEvent.DateNTime == DateNTime &&
                    userEvent.EventFrequency == EventFrequency;
            }
            return false;
        }

        public override int GetHashCode()
        {
            return Id ^ User.GetHashCode() ^ EventName.GetHashCode() ^ EventFrequency.GetHashCode();
        }
    }
}
