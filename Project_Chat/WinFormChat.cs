using System;
using System.Windows.Forms;

namespace Project_Chat
{
    public partial class WinFormChat : Form
    {
        User m_user = null;

        public WinFormChat()
        {
            InitializeComponent();
        }

        public WinFormChat(string strID, string strPassword) : this()
        {
            m_user = new User(strID, strPassword);
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
    }
}
