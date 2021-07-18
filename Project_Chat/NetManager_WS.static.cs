using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project_Chat
{
    public partial class NetManager_WS
    {
        static public void Send_Req_Login(long lUserNo, string strUserName)
        {
            if (s_instance != null)
                s_instance.send_Req_Login(lUserNo, strUserName);
        }

        static public void Send_Req_Chat(long lUserNo, string strUserName, string strMsg, long lTimeStamp)
        {
            if (s_instance != null)
                s_instance.send_Req_Chat(lUserNo, strUserName, strMsg, lTimeStamp);
        }
    }
}
