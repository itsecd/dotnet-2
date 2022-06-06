namespace ChatServer.Services
{
    public interface IChatService
    {
        bool LoginToIdContainsKey(string client);
        
        string[] LoginToIdArrayKeys();

        void LoginToIdAdd(string login, string connectionId);

        void IdToLoginAdd(string connectionId, string login);

        string ResolveCallerLogin(string connectionId);

        public string ResolveConnectionId(string login);
       
    }
}
