using ASPDotNetCore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static ASPDotNetCore.WSPacket;

namespace Project_Chat
{
    class ChatApplicationContext : ApplicationContext
    {
        static ChatApplicationContext s_instance = null;

        #region Constructor
        public ChatApplicationContext(Form mainForm) : base(mainForm)
        {
            MainForm = mainForm;
            init();
        }
        #endregion Constructor

        #region Static
        static public void Recv_Rpy_MemberCheck(Rpy_MemberCheck rpy)
        {
            if (s_instance != null)
                s_instance.recv_Rpy_MemberCheck(rpy);
        }

        static public void Recv_Rpy_SignUp(Rpy_SignUp rpy)
        {
            if (s_instance != null)
                s_instance.recv_Rpy_SignUp(rpy);
        }

        static public void Recv_Rpy_SignIn(Rpy_SignIn rpy)
        {
            if (s_instance != null)
                s_instance.recv_Rpy_SignIn(rpy);
        }

        static public void Recv_Rpy_Login(Rpy_Login rpy)
        {
            if (s_instance != null)
                s_instance.recv_Rpy_Login(rpy);
        }

        static public void Recv_Rpy_Chat(Rpy_Chat rpy)
        {
            if (s_instance != null)
                s_instance.recv_Rpy_Chat(rpy);
        }
        #endregion Static

        void init()
        {
            if (s_instance != null)
                return;

            s_instance = this;

            WindowManager.Create();

            WindowManager.AddListener_ChangeMainForm(changeMainForm);
        }

        void changeMainForm(Form form)
        {
            Form prevForm = MainForm;
            MainForm = form;
            MainForm.Show();

            if (prevForm != null)
                prevForm.Close();
        }

        void recv_Rpy_MemberCheck(Rpy_MemberCheck rpy)
        {
            WinFormLogin form = MainForm as WinFormLogin;
            if(form != null)
            {
                form.Recv_Rpy_MemberCheck(rpy);
            }
        }

        void recv_Rpy_SignUp(Rpy_SignUp rpy)
        {
            WinFormLogin form = MainForm as WinFormLogin;
            if (form != null)
            {
                form.Recv_Rpy_SignUp(rpy);
            }
        }

        void recv_Rpy_SignIn(Rpy_SignIn rpy)
        {
            WinFormLogin form = MainForm as WinFormLogin;
            if (form != null)
            {
                form.Recv_Rpy_SignIn(rpy);
            }
        }

        void recv_Rpy_Login(Rpy_Login rpy)
        {
            WinFormLogin form = MainForm as WinFormLogin;
            if(form != null)
            {
                form.Recv_Rpy_Login(rpy);
            }
        }

        void recv_Rpy_Chat(Rpy_Chat rpy)
        {
            WinFormChat form = MainForm as WinFormChat;
            if (form != null)
            {
                form.Recv_Rpy_Chat(rpy);
            }
        }
    }
}
