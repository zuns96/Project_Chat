using System;
using System.Windows.Forms;

namespace Project_Chat
{
    public partial class WinFormChat : Form
    {
        UserData_Account m_user = null;

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

            listChatBox.Items.Add(string.Format("{0} : {1}", m_user.strID, textMsg.Text));

            textMsg.Text = string.Empty;
        }

        private void WinFormChat_Deactivate(object sender, EventArgs e)
        {
            UserData_Account.Release();
            WindowManager.OpenWindow<WinFormLogin>();
        }
    }
}
