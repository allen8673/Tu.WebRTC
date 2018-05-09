using Fleck;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tu.WebRTC.Model;

namespace Tu.WebRTC.RctPoint
{
    public class RtcServer:IDisposable
    {
        private ConcurrentDictionary<Guid, IWebSocketConnection> UserList = new ConcurrentDictionary<Guid, IWebSocketConnection>();
        private WebSocketServer _SocketServer;

        public RtcServer(string location)
        {
            _SocketServer = new WebSocketServer(location);
            _SocketServer.Start(SocketConfig);
        }

        #region Socket Initial Methods
        /// <summary>
        /// websocket config
        /// </summary>
        /// <param name="socket"></param>
        public void SocketConfig(IWebSocketConnection socket)
        {
            socket.OnOpen = () =>
            {
                try
                {
                    OnConnected(socket);
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"OnConnected: {ex}");
                };
            };
            socket.OnMessage = message =>
            {
                try
                {
                    OnReceive(socket, message);
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"OnReceive: {ex}");
                }
            };
            socket.OnClose = () =>
            {
                try
                {
                    OnDisconnect(socket);
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"OnDisconnect: {ex}");
                }
            };
            socket.OnError = (e) =>
            {
                try
                {
                    OnDisconnect(socket);
                    socket.Close();
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"OnError: {ex}");
                }
            };
        }

        /// <summary>
        /// On Connected Action
        /// </summary>
        /// <param name="context">websocket context</param>
        private void OnConnected(IWebSocketConnection context)
        {
            if (UserList.Count < SocketServerInfo.ClientLimit)
            {
                Debug.WriteLine($"OnConnected: {context.ConnectionInfo.Id}, {context.ConnectionInfo.ClientIpAddress}");
                UserList[context.ConnectionInfo.Id] = context;
            }
            else
            {
                Debug.WriteLine($"OverLimit, Closed: {context.ConnectionInfo.Id}, {context.ConnectionInfo.ClientIpAddress}");
                context.Close();
            }
        }

        /// <summary>
        /// On Receive Action
        /// </summary>
        /// <param name="context">websocket context</param>
        /// <param name="json">Receive message(json)</param>
        private void OnReceive(IWebSocketConnection context, string json)
        {
            foreach (var socket in UserList.Where(i => i.Key != context.ConnectionInfo.Id).Select(i => i.Value))
            {
                socket.Send(json);
            }
        }

        /// <summary>
        /// On Disconnect Action
        /// </summary>
        /// <param name="context">websocket context</param>
        private void OnDisconnect(IWebSocketConnection context)
        {
            Debug.WriteLine($"OnDisconnect: {context.ConnectionInfo.Id}, {context.ConnectionInfo.ClientIpAddress}");
            {
                IWebSocketConnection ctx;
                UserList.TryRemove(context.ConnectionInfo.Id, out ctx);
            }
        }
        #endregion

        public void Dispose()
        {
            try
            {
                foreach (IWebSocketConnection i in UserList.Values)
                {
                    i.Close();
                }

                _SocketServer.Dispose();
                UserList.Clear();
            }
            catch { }
        }
    }
}
