using System;

namespace Server.Model
{
    /// <summary>
    /// User event
    /// </summary>
    public class UserEvent
    {
        /// <summary>
        /// UserEventID
        /// </summary>
        public int Id { get; init; }
        /// <summary>
        /// User
        /// </summary>
        public User User { get; set; }
        /// <summary>
        /// User event name
        /// </summary>
        public string EventName { get; set; }
        /// <summary>
        /// Date and time at which the event will occur
        /// </summary>
        public DateTime DateNTime { get; set; }
        /// <summary>
        /// The frequency with which the event occurs
        /// </summary>
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
            return 819357107 ^ Id;
        }
    }
}
