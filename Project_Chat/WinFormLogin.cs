using ASPDotNetCore.Models;
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
                NetManager_ASP.Send_Req_Member_Check(strID);
            }
        }

        public void Recv_Rpy_MemberCheck(Rpy_MemberCheck rpy)
        {
            string strID = textBoxID.Text;
            string strPassword = textBoxPassword.Text;

            if (rpy.byRet == 1)
                NetManager_ASP.Send_Req_SignIn(strID, strPassword);
            else
                NetManager_ASP.Send_Req_SignUp(strID, strPassword);
        }

        public void Recv_Rpy_SignUp(Rpy_SignUp rpy)
        {
            UserData_Account.Create(rpy.strUserName);
            WindowManager.OpenWindow<WinFormChat>();
        }

        public void Recv_Rpy_SignIn(Rpy_SignIn rpy)
        {
            UserData_Account.Create(rpy.strUserName);
            WindowManager.OpenWindow<WinFormChat>();
        }
    }
}
