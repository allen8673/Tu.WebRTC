using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tu.WebRTC.Model
{
    abstract class SocketMessage
    {
        public const string OnSuccessAnswer = "OnSuccessAnswer";
        public const string OnIceCandidate = "OnIceCandidate";
        public const string OfferSpd = "OfferSdp";

        public string Command { get; set; }
        public string SDP { get; set; }

        public static SocketMessage MessageFac(string json)
        {
            string cmd = JObject.Parse(json)["Command"].ToString();

            switch (cmd)
            {
                case OfferSpd:
                    return JsonConvert.DeserializeObject<OfferSpdMsg>(json);
                case OnSuccessAnswer:
                    return JsonConvert.DeserializeObject<SuccessAnswerMsg>(json);
                case OnIceCandidate:
                    return JsonConvert.DeserializeObject<IceCandidateMsg>(json);
                default:
                    return null;
            }
        }
    }

    class OfferSpdMsg : SocketMessage { }

    class SuccessAnswerMsg : SocketMessage
    {
        public string Type
        {
            get { return "answer"; }
            set { }
        }
    }

    class IceCandidateMsg : SocketMessage
    {
        public string SdpMid { get; set; }
        public int SdpMlineIndex { get; set; }
    }
}
