using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ASPDotNetCore.Models;
using System.Net.WebSockets;

namespace Project_Chat
{
    public class WebSocketManager : Net<WebSocketManager>
    {
        protected override string c_domain_base { get { return "ws://zuns96.iptime.org:12502/ws"; } }

        ClientWebSocket m_clientWebSocket = null;

        public WebSocketManager() : base()
        {
            m_clientWebSocket = new ClientWebSocket();
            ClientWebSocketOptions webSocketOptions = m_clientWebSocket.Options;
            webSocketOptions.KeepAliveInterval = TimeSpan.FromSeconds(120);
        }

        static public void Connect()
        {
            if (s_instance != null)
                s_instance.connect();
        }
        
        void connect()
        {
            ConnectWebSocketClient();
            while(m_clientWebSocket.State != WebSocketState.Open)
            {
                Console.WriteLine("웹 소켓 연결 중");
                Thread.Sleep(100);
            }

            Task t = new Task(ProcessWebSocketClient);
            t.Start();
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
                ArraySegment<byte> buffer = new ArraySegment<byte>(new byte[4096]);
                WebSocketReceiveResult result = await m_clientWebSocket.ReceiveAsync(buffer, CancellationToken.None);
                ChatApplicationContext.Recv_Rpy_Chat(Encoding.UTF8.GetString(buffer.Array, 0, result.Count));
            }

            Console.WriteLine("웹 소켓 프로세스 종료");
        }

        static public void Send_Req_Chat(string msg)
        {
            if (s_instance != null)
                s_instance.send_Req_Chat(msg);
        }

        void send_Req_Chat(string msg)
        {
            ArraySegment<byte> buffer = new ArraySegment<byte>(Encoding.UTF8.GetBytes(msg));
            m_clientWebSocket.SendAsync(buffer, WebSocketMessageType.Text, true, CancellationToken.None).ContinueWith((task)=>
            {
                
            });
        }
    }
}
