using static ASPDotNetCore.ASPPacket;

namespace Project_Chat.Net
{
    public partial class NetManager_ASP
    {
        static public void AddListener_RequestFunc(RequestFunc func)
        {
            if (s_instance != null)
                s_instance.addListener_RequestFunc(func);
        }

        static public void RemoveListener_RequestFunc(RequestFunc func)
        {
            if (s_instance != null)
                s_instance.removeListener_RequestFunc(func);
        }

        static public void Send_Req_MemberCheck(string strID)
        {
            if (s_instance != null)
                s_instance.send_Req_MemberCheck(strID);
        }

        static public void Recv_Rpy_MemberCheck(Rpy_MemberCheck rpy)
        {
            if (s_instance != null)
                s_instance.recv_Rpy_MemberCheck(rpy);
        }

        static public void Send_Req_SignUp(string strID, string strPassword)
        {
            if (s_instance != null)
                s_instance.send_Req_SignUp(strID, strPassword);
        }

        static public void Recv_Rpy_SignUp(Rpy_SignUp rpy)
        {
            if (s_instance != null)
                s_instance.recv_Rpy_SignUp(rpy);
        }

        static public void Send_Req_SignIn(string strID, string strPassword)
        {
            if (s_instance != null)
                s_instance.send_Req_SignIn(strID, strPassword);
        }

        static public void Recv_Rpy_SignIn(Rpy_SignIn rpy)
        {
            if (s_instance != null)
                s_instance.recv_Rpy_SignIn(rpy);
        }
    }
}
