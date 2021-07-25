using static ASPDotNetCore.WSPacket;

namespace Project_Chat.Net
{
    public partial class NetManager_WS
    {
        static public void AddListener_PacketFunc(PacketFunc func)
        {
            if (s_instance != null)
                s_instance.addListener_PacketFunc(func);
        }

        static public void RemoveLisener_PacketFunc(PacketFunc func)
        {
            if (s_instance != null)
                s_instance.removeListener_PacketFunc(func);
        }

        static public void Send_Req_Login(long lUserNo, string strUserName)
        {
            if (s_instance != null)
                s_instance.send_Req_Login(lUserNo, strUserName);
        }

        static public void Recv_Rpy_Login(Rpy_Login rpy)
        {
            if (s_instance != null)
                s_instance.recv_Rpy_Login(rpy);
        }

        static public void Send_Req_Chat(long lUserNo, string strUserName, string strMsg, long lTimeStamp)
        {
            if (s_instance != null)
                s_instance.send_Req_Chat(lUserNo, strUserName, strMsg, lTimeStamp);
        }

        static public void Recv_Rpy_Chat(Rpy_Chat rpy)
        {
            if (s_instance != null)
                s_instance.recv_Rpy_Chat(rpy);
        }
    }
}
