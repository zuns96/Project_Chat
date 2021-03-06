using System;
using System.Windows.Forms;
using Project_Chat.Net;
using static ASPDotNetCore.WSPacket;

namespace Project_Chat
{
    public partial class WinFormChat : Form
    {
        public WinFormChat()
        {
            InitializeComponent();
        }

        private void btnSend_Click(object sender, EventArgs e)
        {
            string msg = textMsg.Text;
            if (string.IsNullOrEmpty(msg))
            {
                return;
            }

            UserData_Account userDataAccount = UserData_Account.Instance;
            long lUserNo = userDataAccount.lUserNo;
            string strUserName = userDataAccount.strUserName;
            long lTimeStamp = TimeManager.TimeStamp;
            NetManager_WS.Send_Req_Chat(lUserNo, strUserName, msg, lTimeStamp);

            textMsg.Text = string.Empty;
        }

        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            UserData_Account.Release();
            NetWS.Disconnect();
            WindowManager.OpenWindow<WinFormLogin>();

            base.OnFormClosed(e);
        }

        public void Recv_Rpy_Chat(Rpy_Chat rpy)
        {
            string msg = string.Format("{0} : {1}", rpy.strSender, rpy.strMsg);

            listChatBox.Items.Add(msg);
        }
    }
}
