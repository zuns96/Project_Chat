using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ASPDotNetCore.Models;
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
                Console.WriteLine("웹 소켓 연결 중");
                Thread.Sleep(100);
            }

            Task t = new Task(ProcessWebSocketClient);
            t.Start();
        }

        void disconnect()
        {
            m_clientWebSocket.CloseAsync(WebSocketCloseStatus.Empty, string.Empty, CancellationToken.None).Wait();
            m_clientWebSocket.Dispose();
            m_clientWebSocket = null;
        }

        private void ConnectWebSocketClient()
        {
            Uri serverUri = new Uri(c_domain_base);
            m_clientWebSocket.ConnectAsync(serverUri, CancellationToken.None).ContinueWith((task) =>
            {
                if(m_clientWebSocket.State == WebSocketState.Open)
                    Console.WriteLine("웹 소켓 연결 완료");
            });
        }

        private async void ProcessWebSocketClient()
        {
            while(m_clientWebSocket.State == WebSocketState.Open)
            {
                ArraySegment<byte> buffer = new ArraySegment<byte>(new byte[1024 * 1024 * 10]);
                WebSocketReceiveResult result = await m_clientWebSocket.ReceiveAsync(buffer, CancellationToken.None);
                
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

            Console.WriteLine("웹 소켓 프로세스 종료");
        }

        static public void Send_Req_Login(long lUserNo, string strUserName)
        {
            if (s_instance != null)
                s_instance.send_Req_Login(lUserNo, strUserName);
        }

        void send_Req_Login(long lUserNo, string strUserName)
        {
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
        }

        static public void Send_Req_Chat(long lUserNo, string strUserName, string strMsg, long lTimeStamp)
        {
            if (s_instance != null)
                s_instance.send_Req_Chat(lUserNo, strUserName, strMsg, lTimeStamp);
        }

        void send_Req_Chat(long lUserNo, string strUserName, string strMsg, long lTimeStamp)
        {
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
        }

        void Send(Packet packet)
        {
            string json = JsonConvert.SerializeObject(packet);
            ArraySegment<byte> buffer = new ArraySegment<byte>(Encoding.UTF8.GetBytes(json));
            m_clientWebSocket.SendAsync(buffer, WebSocketMessageType.Binary, true, CancellationToken.None).ContinueWith((task) =>
            {

            });
        }
    }
}
