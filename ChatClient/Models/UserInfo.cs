using ReactiveUI;

namespace ChatClient.Models
{
    public class UserInfo : ReactiveObject
    {
        public string Name { get; set; } = "";
        public bool Status { get; set; }
        public string FormatName => Status ? Name + "   online" : Name + "   offline";
    }
}
