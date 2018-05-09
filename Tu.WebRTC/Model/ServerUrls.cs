using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tu.WebRTC.Model
{
    class ServerUrls
    {
        const string stun_1 = "stun:stun.l.google.com:19302";
        const string stun_2 = "stun:stun.l.google.com:19302";
        const string stun_3 = "stun:stun.l.google.com:19302";

        public static List<string> GetStunUrls()
        {
            return new List<string> { stun_1, stun_2, stun_3 };
        }
    }
}
