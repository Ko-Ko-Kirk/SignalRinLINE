using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SignalRinLINE.Models
{
    public class ChatMessage
    {
        public string LineName { get; set; }
        public string LineID { get; set; }
        public string LinePic { get; set; }
        public string Text { get; set; }
        public DateTime SendTime { get; set; }
    }
}
   
}
