using System;
using System.Windows.Forms;

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
            if(string.IsNullOrEmpty(textMsg.Text))
            {
                return;
            }

            string msg = string.Format("{0} : {1}", UserData_Account.Instance.strID, textMsg.Text);

            WebSocketManager.Send_Req_Chat(msg);

            textMsg.Text = string.Empty;
        }

        private void WinFormChat_Deactivate(object sender, EventArgs e)
        {
            UserData_Account.Release();
            WindowManager.OpenWindow<WinFormLogin>();
        }

        public void Recv_Rpy_Chat(string msg)
        {
            Invoke(new Action(() => listChatBox.Items.Add(msg)));
        }
    }
}
