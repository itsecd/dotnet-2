namespace Server.Model
{
    /// <summary>
    /// User
    /// </summary>
    public class User
    {
        /// <summary>
        /// UserId
        /// </summary>
        public int Id { get; init; }
        /// <summary>
        /// User name
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Chat id in telegram
        /// </summary>
        public long ChatId { get; set; }
        /// <summary>
        /// Shows whether the user has alerts enabled
        /// </summary>
        public bool Toggle { get; set; }

        public override bool Equals(object obj)
        {
            if (obj is User user)
            {
                return user.Name == Name || user.ChatId == ChatId;
            }
            return false;
        }

        public override int GetHashCode()
        {
            return 1274718499 ^ Id;
        }
    }
}
