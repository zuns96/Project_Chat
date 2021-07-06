namespace Project_Chat
{
    public partial class NetManager_ASP
    {
        static public void Send_Req_MemberCheck(string strID)
        {
            if (s_instance != null)
                s_instance.send_Req_MemberCheck(strID);
        }

        static public void Send_Req_SignUp(string strID, string strPassword)
        {
            if (s_instance != null)
                s_instance.send_Req_SignUp(strID, strPassword);
        }

        static public void Send_Req_SignIn(string strID, string strPassword)
        {
            if (s_instance != null)
                s_instance.send_Req_SignIn(strID, strPassword);
        }
    }
}
