using System;
using System.Text;
using System.Threading;
using System.Net.WebSockets;
using Newtonsoft.Json;
using static ASPDotNetCore.WSPacket;


namespace Project_Chat
{
    public class NetWS : Net<NetWS>
    {
        protected override string c_domain_base { get { return "ws://127.0.0.1:12502/ws"; } }

        ClientWebSocket m_clientWebSocket = null;

        public NetWS() : base()
        {
            NetManager_WS.Create();
            NetManager_WS.AddListener_PacketFunc(send);
        }

        protected override void release()
        {
            NetManager_WS.RemoveLisener_PacketFunc(send);
            NetManager_WS.Release();

            base.release();
        }

        static public void Connect()
        {
            if (s_instance != null)
                s_instance.connect();
        }

        static public void Disconnect()
        {
            if (s_instance != null)
                s_instance.disconnect();
        }

        static public void Send(Packet packet)
        {
            if (s_instance != null)
                s_instance.send(packet);
        }

        void connect()
        {
            m_clientWebSocket = new ClientWebSocket();
            ClientWebSocketOptions webSocketOptions = m_clientWebSocket.Options;
            webSocketOptions.KeepAliveInterval = TimeSpan.FromSeconds(120);

            ConnectWebSocketClient();
            while (m_clientWebSocket.State == WebSocketState.Connecting)
            {
                Log.Write("웹 소켓 연결 중...");
                Thread.Sleep(100);
            }

            ProcessWebSocketClient();
        }

        private void disconnect()
        {
            if (m_clientWebSocket != null && m_clientWebSocket.State == WebSocketState.Open)
            {
                m_clientWebSocket.CloseAsync(WebSocketCloseStatus.Empty, string.Empty, CancellationToken.None).Wait();

                Log.Write("웹 소켓 연결 중지");
            }
        }

        private void ConnectWebSocketClient()
        {
            Uri serverUri = new Uri(c_domain_base);
            Log.Write("웹 소켓 연결 시작, webScoketServer : {0}", serverUri.AbsoluteUri);
            m_clientWebSocket.ConnectAsync(serverUri, CancellationToken.None).ContinueWith((task) =>
            {
                if (m_clientWebSocket.State == WebSocketState.Open)
                    Log.Write("웹 소켓 연결 완료...");
            });
        }

        private async void ProcessWebSocketClient()
        {
            ArraySegment<byte> buffer = new ArraySegment<byte>(new byte[1024 * 1024]);
            WebSocketReceiveResult result = null;
            WebSocketState state = WebSocketState.None;
            while ((state = m_clientWebSocket.State) == WebSocketState.Open)
            {
                try
                {
                    result = await m_clientWebSocket.ReceiveAsync(buffer, CancellationToken.None);
                }
                catch (WebSocketException ex)
                {
                    Log.Write("#### Exception!!! THROWN ####");
                    Log.Write(ex.Message);
                    Log.Write(ex.StackTrace);
                    Log.Write("#### Exception!!! THROWN ####");
                    continue;
                }
                catch (Exception ex)
                {
                    Log.Write("#### Exception!!! THROWN ####");
                    Log.Write(ex.Message);
                    Log.Write(ex.StackTrace);
                    Log.Write("#### Exception!!! THROWN ####");
                    continue;
                }

                if (state == WebSocketState.Open)
                {
                    string json = Encoding.UTF8.GetString(buffer.Array, 0, result.Count);

                    Packet packet = JsonConvert.DeserializeObject<Packet>(json);
                    switch (packet.hd.iRmiID)
                    {
                        case (int)E_RMIID.E_RMIID_RPY_LOGIN:
                            {
                                Rpy_Login rpy = JsonConvert.DeserializeObject<Rpy_Login>(packet.strJson);
                                do_recv_rpy_login(rpy);
                            }
                            break;
                        case (int)E_RMIID.E_RMIID_RPY_CHAT:
                            {
                                Rpy_Chat rpy = JsonConvert.DeserializeObject<Rpy_Chat>(packet.strJson);
                                do_recv_rpy_chat(rpy);
                            }
                            break;
                    }
                }
            }

            Log.Write("웹 소켓 프로세스 종료");

            if (m_clientWebSocket != null)
            {
                m_clientWebSocket.Dispose();
                m_clientWebSocket = null;
            }

            WindowManager.OpenWindow<WinFormLogin>();
        }

        void send(Packet packet)
        {
            string json = JsonConvert.SerializeObject(packet);
            ArraySegment<byte> buffer = new ArraySegment<byte>(Encoding.UTF8.GetBytes(json));
            m_clientWebSocket.SendAsync(buffer, WebSocketMessageType.Binary, true, CancellationToken.None).Wait();
        }

        void do_recv_rpy_login(Rpy_Login rpy)
        {
            Log.Write("do_recv_rpy_login -------->>");

            Log.Write("byRet({0}),lUserNo({1}),strUserName({2})", 
                rpy.byRet,
                rpy.lUserNo,
                rpy.strUserName);

            NetManager_WS.Recv_Rpy_Login(rpy);

            Log.Write("do_recv_rpy_login <<--------");
        }

        void do_recv_rpy_chat(Rpy_Chat rpy)
        {
            Log.Write("do_recv_rpy_chat -------->>");

            Log.Write("byRet({0}),lUserNo({1}),strSender({2}),strMsg({3}),lTimeStamp({4})",
                rpy.byRet,
                rpy.lUserNo,
                rpy.strSender,
                rpy.strMsg,
                rpy.lTimeStamp);

            NetManager_WS.Recv_Rpy_Chat(rpy);

            Log.Write("do_recv_rpy_chat <<--------");
        }
    }
}
