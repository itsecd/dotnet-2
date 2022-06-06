using System;
using System.Globalization;
namespace ChatClient.Models
{
    public class HistoryOfMessagesModel
    {
        public string User = "";
        public string Message = "";
        public DateTime Date;
        public string FormatNameMessage => User + ">>>  " + Message + "    " + Date.ToString(CultureInfo.InvariantCulture);
    }
}
