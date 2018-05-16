using Fleck;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Tu.WebRTC.Model;
using Vedio.Model;
using WebRtc.NET;
using WebSocketSharp;

namespace Tu.WebRTC.RctPoint
{
    public class RtcClient : IDisposable
    {
        /// <summary>
        /// WebRtc.Net lib
        /// </summary>
        public readonly WebRtcNative _WebRtc;
        public readonly CancellationTokenSource _Cancel;
        ScreenCaptureInfo _CaptureInfo = new ScreenCaptureInfo();

        /// <summary>
        /// Client Socket: use to trans peer to peer(WebRTC) connection info
        /// </summary>
        private WebSocket _SocketClient;
        CancellationTokenSource _TurnCancel;

        /// <summary>
        /// RtcClient Constructor
        /// </summary>
        /// <param name="url"></param>
        public RtcClient(string socketUrl)
        {
            try
            {
                _SocketClient = new WebSocket(socketUrl);
                _WebRtc = new WebRtcNative();
                _Cancel = new CancellationTokenSource();
                _TurnCancel = new CancellationTokenSource();
                SocketInitial();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }

        public void Dispose()
        {
            _WebRtc.Dispose();
            _SocketClient.Close();
            _Cancel.Cancel();
        }

        public void SocketConnect()
        {
            try
            {
                _SocketClient.Connect();
                if (_SocketClient.IsAlive) RtcInitial();
            }
            catch (Exception ex)
            {
            }
        }

        public void RtcConnect()
        {

            try
            {
                //if (!_SocketClient.IsAlive) return;
                //RtcInitial();

                Task.Run(() =>
                {
                    var ok = _WebRtc.InitializePeerConnection();
                    if (ok)
                    {
                        _WebRtc.CreateOffer();
                        while (_WebRtc.ProcessMessages(3)) { }
                        _WebRtc.ProcessMessages(3);
                    }
                    else
                    {
                        Debug.WriteLine("InitializePeerConnection failed");
                    }
                });
            }
            catch (Exception ex)
            {
            }
        }

        /// <summary>
        /// Set Native OnRenderRemote Callback Method 
        /// </summary>
        /// <param name="callback">
        /// callback method parameters: callback_method(IntPtr BGR24, uint w, uint h)
        /// </param>
        public void SetRenderRemote(Action<IntPtr, uint, uint> callback)
        {
            _OnRenderRemoteCallback = callback.Invoke;
            //_WebRtc.OnRenderRemote += callback.Invoke;
        }
        private WebRtcNative.OnCallbackRender _OnRenderRemoteCallback;

        /// <summary>
        /// Set Native OnRenderLocal Callback Method
        /// </summary>
        /// <param name="callback">
        /// callback method parameters: callback_method(IntPtr BGR24, uint w, uint h)
        /// </param>
        public void SetRenderLocal(Action<IntPtr, uint, uint> callback)
        {
            _OnRenderLocalCallback = callback.Invoke;
            //_WebRtc.OnRenderLocal += callback.Invoke;
        }
        private WebRtcNative.OnCallbackRender _OnRenderLocalCallback;

        /// <summary>
        /// Initial Client Socket
        /// </summary>
        protected void SocketInitial()
        {
            _SocketClient.OnMessage += (s, e) =>
            {
                string data = e.Data;
                SocketMessage message = SocketMessage.MessageFac(data);

                switch (message.Command)
                {
                    case SocketMessage.OfferSpd:
                        using (var go = new ManualResetEvent(false))
                        {
                            var t = Task.Factory.StartNew(() =>
                            {
                                var ok = _WebRtc.InitializePeerConnection();
                                if (ok)
                                {
                                    go.Set();
                                    while (_WebRtc.ProcessMessages(1000)) { }
                                    _WebRtc.ProcessMessages(1000);
                                }
                            }, _Cancel.Token, TaskCreationOptions.LongRunning, TaskScheduler.Default);

                            if (go.WaitOne(9999)) _WebRtc.OnOfferRequest(message.SDP);
                        }
                        break;
                    case SocketMessage.OnSuccessAnswer:
                        SuccessAnswerMsg answerMsg = message as SuccessAnswerMsg;
                        _WebRtc.OnOfferReply(answerMsg.Type, answerMsg.SDP);
                        break;
                    case SocketMessage.OnIceCandidate:
                        IceCandidateMsg iceMsg = message as IceCandidateMsg;
                        _WebRtc.AddIceCandidate(iceMsg.SdpMid, iceMsg.SdpMlineIndex, iceMsg.SDP);
                        break;
                }
            };
            _SocketClient.OnClose += (s, e) => _Cancel.Cancel();
            _SocketClient.OnError += (s, e) => _Cancel.Cancel();
        }

        /// <summary>
        /// Initial WebRtc Component
        /// </summary>
        protected void RtcInitial()
        {
            try
            {
                WebRtcNative.InitializeSSL();
                ServerUrls.GetStunUrls().ForEach(url =>
                {
                    // Set Stun Servers
                    _WebRtc.AddServerConfig(url, string.Empty, string.Empty);
                });

                // Get audio control
                _WebRtc.SetAudio(true);
                _WebRtc.OpenVideoCaptureDevice("");
                // Set Video Capturer
                {
                    IVedioCaptureInfo cptr = new ScreenCaptureInfo();
                    _WebRtc.SetVideoCapturer(cptr.Width, cptr.Height, cptr.CaptureFps);
                }
                // Relative Connection callback
                {
                    // Be triggered On Success to generate Offer
                    _WebRtc.OnSuccessOffer += (sdp) =>
                    {
                        OfferSpdMsg msg = new OfferSpdMsg
                        {
                            Command = SocketMessage.OfferSpd,
                            SDP = sdp
                        };
                        _SocketClient.Send(JsonConvert.SerializeObject(msg));
                    };

                    // Be triggered On Success to generate ICE
                    _WebRtc.OnIceCandidate += (sdp_mid, sdp_mline_index, sdp) =>
                    {
                        IceCandidateMsg msg = new IceCandidateMsg
                        {
                            Command = SocketMessage.OnIceCandidate,
                            SDP = sdp,
                            SdpMid = sdp_mid,
                            SdpMlineIndex = sdp_mline_index,
                        };
                        _SocketClient.Send(JsonConvert.SerializeObject(msg));
                    };

                    // Be triggered On OfferRequest
                    _WebRtc.OnSuccessAnswer += (sdp) =>
                    {
                        SuccessAnswerMsg msg = new SuccessAnswerMsg
                        {
                            Command = SocketMessage.OnSuccessAnswer,
                            SDP = sdp
                        };
                        _SocketClient.Send(JsonConvert.SerializeObject(msg));
                    };
                }
                // Set Local and Remote callback
                {
                    _WebRtc.OnRenderLocal += _OnRenderLocalCallback;
                    _WebRtc.OnRenderRemote += _OnRenderRemoteCallback;
                }
                // Set Other callback
                {
                    _WebRtc.OnSuccessAnswer += (sdp) => { Debug.WriteLine($"Success Get SDP:{sdp}"); };
                    _WebRtc.OnFailure += (failure) => { Debug.WriteLine($"Failure Msg:{failure}"); };
                    _WebRtc.OnError += (error) => { Debug.WriteLine($"Error Msg:{error}"); };
                    _WebRtc.OnDataMessage += (dmsg) => { Debug.WriteLine($"OnDataMessage: {dmsg}"); };
                    _WebRtc.OnDataBinaryMessage += (dmsg) => { Debug.WriteLine($"OnDataBinaryMessage: {dmsg.Length}"); };
                }
            }
            catch (Exception ex) { }
        }


    }
}
