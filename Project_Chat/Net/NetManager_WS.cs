using System.Windows.Forms;
using Newtonsoft.Json;
using static ASPDotNetCore.WSPacket;

namespace Project_Chat.Net
{
    public partial class NetManager_WS : Singleton<NetManager_WS>
    {
        public delegate void PacketFunc(Packet packet);

        PacketFunc m_packetFunc = null;

        public NetManager_WS() : base()
        {
            
        }

        void addListener_PacketFunc(PacketFunc func)
        {
            m_packetFunc += func;
        }

        void removeListener_PacketFunc(PacketFunc func)
        {
            m_packetFunc -= func;
        }

        void ErrorMessage_Common(byte byRet)
        {
            string msg = string.Empty;
            switch ((ERROR_MSG_COMMON)byRet)
            {
                case ERROR_MSG_COMMON.FAILURE:
                    {
                        msg = "실패 하였습니다.";
                    }
                    break;
            }

            Log.Write(msg);
            MessageBox.Show(msg);
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

            m_packetFunc(packet);
            Log.Write("Send_Req_Login 끝 <<---------");
        }

        void recv_Rpy_Login(Rpy_Login rpy)
        {
            Log.Write("Recv_Rpy_Login 시작 --------->>");
            byte byRet = rpy.byRet;
            if (byRet == (byte)ERROR_MSG_COMMON.SUCCESS)
            {
                ChatApplicationContext.Recv_Rpy_Login(rpy);
            }
            else
            {
                ErrorMessage_Rpy_Login(byRet);
            }
            Log.Write("Recv_Rpy_Login 끝 <<---------");
        }

        void ErrorMessage_Rpy_Login(byte byRet)
        {
            if (byRet >= (byte)ERROR_MSG_COMMON.FAILURE && byRet < (byte)ERROR_MSG_COMMON.MAX)
            {
                ErrorMessage_Common(byRet);
            }
            else
            {
                string msg = string.Empty;
                switch ((ERROR_MSG_LOGIN)byRet)
                {

                }

                Log.Write(msg);
                MessageBox.Show(msg);
            }

            NetWS.Disconnect();
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

            m_packetFunc(packet);
            Log.Write("Send_Req_Chat 끝 <<---------");
        }

        void recv_Rpy_Chat(Rpy_Chat rpy)
        {
            Log.Write("Recv_Rpy_Chat 시작 --------->>");
            byte byRet = rpy.byRet;
            if (byRet == (byte)ERROR_MSG_COMMON.SUCCESS)
            {
                ChatApplicationContext.Recv_Rpy_Chat(rpy);
            }
            else
            {
                ErrorMessage_Rpy_Chat(byRet);
            }
            Log.Write("Recv_Rpy_Chat 끝 <<---------");
        }

        void ErrorMessage_Rpy_Chat(byte byRet)
        {
            if (byRet >= (byte)ERROR_MSG_COMMON.FAILURE && byRet < (byte)ERROR_MSG_COMMON.MAX)
            {
                ErrorMessage_Common(byRet);
            }
            else
            {
                string msg = string.Empty;
                switch ((ERROR_MSG_CHAT)byRet)
                {

                }

                Log.Write(msg);
                MessageBox.Show(msg);
            }
        }
    }
}
