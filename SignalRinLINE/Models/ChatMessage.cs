using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SignalRinLINE.Models
{
    public class ChatMessage
    {
        public string LineName { get; set; }
        public string LineID { get; set; } = "A12345678";
        public string LinePic { get; set; } = "https://s3-ap-northeast-1.amazonaws.com/magic-panda-engineer/blog-img/magicpandaengineer.jpg";
        public string Text { get; set; } = "您好啊，我可以幫您什麼呢？";
        public DateTime SendTime { get; set; }
    }
}
   

