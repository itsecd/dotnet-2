using System;

namespace Server.Model
{
    public class UserEvent
    {
        public int Id { get; set; }
        public User User { get; set; }
        public string EventName { get; set; }
        public DateTime DateNTime { get; set; }
        public Frequency EventFrequency { get; set; }
        public enum Frequency
        {
            everyDay = 1,
            every2Days = 2,
            every3Days = 3,
            every4Days = 4,
            every5Days = 5,
            every6Days = 6,
            every7Days = 7,

        };

        public override bool Equals(object obj)
        {
            if (obj is UserEvent userEvent)
            {
                return userEvent.User.Equals(User) &&
                    userEvent.EventName == EventName &&
                    userEvent.DateNTime == DateNTime;
            }
            return false;
        }
    }
}
