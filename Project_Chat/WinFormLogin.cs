using System;
using System.Windows.Forms;

namespace Project_Chat
{
    public partial class WinFormLogin : Form
    {
        public WinFormLogin()
        {
            InitializeComponent();
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            string strID = textBoxID.Text;
            string strPassword = textBoxPassword.Text;

            Console.WriteLine("ID : " + strID + "\nPassword : " + strPassword);

            if (string.IsNullOrEmpty(strID))
            {
                MessageBox.Show("아이디를 입력해주세요.");
            }
            else if (string.IsNullOrEmpty(strPassword))
            {
                MessageBox.Show("패스워드를 입력해주세요.");
            }
            else
            {
                WinFormChat winFormChat = new WinFormChat(strID, strPassword);
                Program.ApplicartionContext.MainForm = winFormChat;
                winFormChat.Show();
                Close();
            }
        }
    }
}
