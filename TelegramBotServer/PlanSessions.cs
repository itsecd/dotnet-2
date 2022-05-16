using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TelegramBotServer
{
    public class PlanSessions
    {
        public Dictionary<long, PlanSession> sessions { get; set; } = new Dictionary<long, PlanSession>();

    }
}
