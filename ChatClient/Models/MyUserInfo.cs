using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatClient.Models
{
    public class MyUserInfo
    {
        public string Name;
        public bool Status;

        public string FormatName => Status ? Name + "   online": Name + "   offline";

    }

   
    

}
