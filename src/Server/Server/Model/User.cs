namespace Server.Model
{
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool Toggle { get; set; }

        public override bool Equals(object obj)
        {
            if (obj is User user)
            {
                return user.Name == Name;
            }
            return false;
        }
        public override int GetHashCode()
        {
            int nameHashCode = Name.GetHashCode();
            return nameHashCode ^ Id ^ Toggle.GetHashCode();
        }
    }
}
