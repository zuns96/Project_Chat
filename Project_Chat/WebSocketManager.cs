using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Net.WebSockets;
using Newtonsoft.Json;
using static ASPDotNetCore.WSPacket;

namespace Project_Chat
{
    public class WebSocketManager : Net<WebSocketManager>
    {
        protected override string c_domain_base { get { return "ws://localhost:6038/ws"; } }

        ClientWebSocket m_clientWebSocket = null;

        public WebSocketManager() : base()
        {
            
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

        void connect()
        {
            m_clientWebSocket = new ClientWebSocket();
            ClientWebSocketOptions webSocketOptions = m_clientWebSocket.Options;
            webSocketOptions.KeepAliveInterval = TimeSpan.FromSeconds(120);

            ConnectWebSocketClient();
            while(m_clientWebSocket.State != WebSocketState.Open)
            {
                Log.Write("웹 소켓 연결 중...");
                Thread.Sleep(100);
            }

            ProcessWebSocketClient();
        }

        void disconnect()
        {
            m_clientWebSocket.CloseAsync(WebSocketCloseStatus.Empty, string.Empty, CancellationToken.None).Wait();
            m_clientWebSocket.Dispose();
            m_clientWebSocket = null;
            Log.Write("웹 소켓 연결 중지");
        }

        private void ConnectWebSocketClient()
        {
            Uri serverUri = new Uri(c_domain_base);
            Log.Write("웹 소켓 연결 시작, webScoketServer : {0}", serverUri.AbsoluteUri);
            m_clientWebSocket.ConnectAsync(serverUri, CancellationToken.None).ContinueWith((task) =>
            {
                if(m_clientWebSocket.State == WebSocketState.Open)
                    Log.Write("웹 소켓 연결 완료...");
            });
        }

        private async void ProcessWebSocketClient()
        {
            ArraySegment<byte> buffer = new ArraySegment<byte>(new byte[1024 * 1024 * 10]);
            WebSocketReceiveResult result = null;
            while (m_clientWebSocket.State == WebSocketState.Open)
            {
                try
                {
                    result = await m_clientWebSocket.ReceiveAsync(buffer, CancellationToken.None);
                }
                catch (Exception ex)
                {
                    Log.Write("#### Exception!!! THROWN ####");
                    Log.Write(ex.Message);
                    Log.Write(ex.StackTrace);
                    Log.Write("#### Exception!!! THROWN ####");
                    continue;
                }

                string json = Encoding.UTF8.GetString(buffer.Array, 0, result.Count);

                Packet packet = JsonConvert.DeserializeObject<Packet>(json);
                switch(packet.hd.iRmiID)
                { 
                    case (int)E_RMIID.E_RMIID_RPY_LOGIN:
                        {
                            Rpy_Login rpy = JsonConvert.DeserializeObject<Rpy_Login>(packet.strJson);
                            ChatApplicationContext.Recv_Rpy_Login(rpy);
                        }
                        break;
                    case (int)E_RMIID.E_RMIID_RPY_CHAT:
                        {
                            Rpy_Chat rpy = JsonConvert.DeserializeObject<Rpy_Chat>(packet.strJson);
                            ChatApplicationContext.Recv_Rpy_Chat(rpy);
                        }
                        break;
                }
            }

            Log.Write("웹 소켓 프로세스 종료");

            WindowManager.OpenWindow<WinFormLogin>();
        }

        static public void Send_Req_Login(long lUserNo, string strUserName)
        {
            if (s_instance != null)
                s_instance.send_Req_Login(lUserNo, strUserName);
        }

        void send_Req_Login(long lUserNo, string strUserName)
        {
            Log.Write("Send_Req_Login({0},{1}) 시작 --------->>", lUserNo, strUserName);

            Req_Login req = new Req_Login();
            req.lUserNo = lUserNo;
            req.strUserName = strUserName;
            
            string json = JsonConvert.SerializeObject(req);
            Packet packet = new Packet();
            CommonHeader hd = new CommonHeader();
            hd.iRmiID = (int)E_RMIID.E_RMIID_REQ_LOGIN;
            packet.hd = hd;
            packet.strJson = json;

            Send(packet);
            Log.Write("Send_Req_Login 끝 <<---------");
        }

        static public void Send_Req_Chat(long lUserNo, string strUserName, string strMsg, long lTimeStamp)
        {
            if (s_instance != null)
                s_instance.send_Req_Chat(lUserNo, strUserName, strMsg, lTimeStamp);
        }

        void send_Req_Chat(long lUserNo, string strUserName, string strMsg, long lTimeStamp)
        {
            Log.Write("Send_Req_Login({0},{1},{2},{3}) 시작 --------->>", lUserNo, strUserName, strMsg, lTimeStamp);

            Req_Chat req = new Req_Chat();
            req.lUserNo = lUserNo;
            req.strSender = strUserName;
            req.strMsg = strMsg;
            req.lTimeStamp = lTimeStamp;

            string json = JsonConvert.SerializeObject(req);
            Packet packet = new Packet();
            CommonHeader hd = new CommonHeader();
            hd.iRmiID = (int)E_RMIID.E_RMIID_REQ_CHAT;
            packet.hd = hd;
            packet.strJson = json;

            Send(packet);
            Log.Write("Send_Req_Chat 끝 <<---------");
        }

        void Send(Packet packet)
        {
            string json = JsonConvert.SerializeObject(packet);
            ArraySegment<byte> buffer = new ArraySegment<byte>(Encoding.UTF8.GetBytes(json));
            m_clientWebSocket.SendAsync(buffer, WebSocketMessageType.Binary, true, CancellationToken.None).Wait();
        }
    }
}
