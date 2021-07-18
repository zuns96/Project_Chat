using ASPDotNetCore;
using System;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.IO;
using System.Windows.Forms;
using Newtonsoft.Json;
using static ASPDotNetCore.ASPPacket;

namespace Project_Chat
{
    public partial class NetManager_ASP : Net<NetManager_ASP>
    {
        protected override string c_domain_base { get { return "http://localhost:6038/api/{0}"; } }


        void ErrorMessage_Common(byte byRet)
        {
            string msg = string.Empty;
            switch((ERROR_MSG_COMMON) byRet)
            {
                case ERROR_MSG_COMMON.FAILURE:
                    {
                        msg = "실패 하였습니다.";
                    }
                    break;
            }

            Log.Write(msg);
            MessageBox.Show(msg);
        }

        #region API
        void send_Req_MemberCheck(string strID)
        {
            Log.Write("Send_Req_MemberCheck 시작 --------->>");
            string api = string.Format(c_domain_base, "auth/memberCheck");
            Log.Write("api:{0}({1})", api, strID);

            // Request Data 만들기 -->>
            Req_MemberCheck req = new Req_MemberCheck() 
            { 
                strID = strID, 
            };
            string json = JsonConvert.SerializeObject(req);
            byte[] buffer = Encoding.UTF8.GetBytes(json);

            WebRequest webRequest = WebRequest.Create(api);
            webRequest.Method = "POST";
            webRequest.Credentials = CredentialCache.DefaultCredentials;
            webRequest.ContentType = "application/json";

            if (!SetPostData(webRequest, buffer))
            {
                Log.Write("포스트 데이터 세팅 실패!");
                return;
            }
            // Request Data 만들기 <<--

            Request<Rpy_MemberCheck>(webRequest, recv_Rpy_MemberCheck);
            Log.Write("Send_Req_MemberCheck 끝 <<---------");
        }

        void recv_Rpy_MemberCheck(Rpy_MemberCheck rpy)
        {
            Log.Write("Recv_Rpy_MemberCheck 시작 --------->>");
            byte byRet = rpy.byRet;
            if (byRet == 1)
            {
                ChatApplicationContext.Recv_Rpy_MemberCheck(rpy);
            }
            else
            {
                ErrorMessage_Rpy_MemberCheck(byRet);
            }
            Log.Write("Recv_Rpy_MemberCheck 끝 <<---------");
        }

        void ErrorMessage_Rpy_MemberCheck(byte byRet)
        {
            if(byRet >= (byte)ERROR_MSG_COMMON.FAILURE && byRet < (byte) ERROR_MSG_COMMON.MAX)
            {
                ErrorMessage_Common(byRet);
            }
            else
            {
                string msg = string.Empty;
                switch((ERROR_MSG_MEMBERCHECK)byRet)
                {

                }

                Log.Write(msg);
                MessageBox.Show(msg);
            }
        }

        void send_Req_SignUp(string strID, string strPassword)
        {
            Log.Write("Send_Req_SignUp 시작 --------->>");
            string api = string.Format(c_domain_base, "auth/signUp");
            Log.Write("api:{0}({1},{2})", api, strID, strPassword);

            // Request Data 만들기 -->>
            Req_SignUp req = new Req_SignUp()
            {
                strID = strID,
                strPassword = strPassword,
            };
            string json = JsonConvert.SerializeObject(req);
            byte[] buffer = Encoding.UTF8.GetBytes(json);

            WebRequest webRequest = WebRequest.Create(api);
            webRequest.Method = "POST";
            webRequest.Credentials = CredentialCache.DefaultCredentials;
            webRequest.ContentType = "application/json";

            if (!SetPostData(webRequest, buffer))
            {
                Log.Write("포스트 데이터 세팅 실패!");
                return;
            }
            // Request Data 만들기 <<--

            Request<Rpy_SignUp>(webRequest, recv_Rpy_SignUp);
            Log.Write("Send_Req_SignUp 끝 <<---------");
        }

        void recv_Rpy_SignUp(Rpy_SignUp rpy)
        {
            Log.Write("Recv_Rpy_SignUp 시작 --------->>");
            byte byRet = rpy.byRet;
            if (byRet == 1)
            {
                ChatApplicationContext.Recv_Rpy_SignUp(rpy);
            }
            else
            {
                ErrorMessage_Rpy_SignUp(byRet);
            }
            Log.Write("Recv_Rpy_SignUp 끝 <<---------");
        }

        void ErrorMessage_Rpy_SignUp(byte byRet)
        {
            if (byRet >= (byte)ERROR_MSG_COMMON.FAILURE && byRet < (byte)ERROR_MSG_COMMON.MAX)
            {
                ErrorMessage_Common(byRet);
            }
            else
            {
                string msg = string.Empty;
                switch ((ERROR_MSG_SIGNUP)byRet)
                {

                }

                Log.Write(msg);
                MessageBox.Show(msg);
            }
        }

        void send_Req_SignIn(string strID, string strPassword)
        {
            Log.Write("Send_Req_SignIn 시작 --------->>");
            string api = string.Format(c_domain_base, "auth/signIn");
            Log.Write("api:{0}({1},{2})", api, strID, strPassword);

            // Request Data 만들기 -->>
            Req_SignIn req = new Req_SignIn()
            {
                strID = strID,
                strPassword = strPassword,
            };
            
            string json = JsonConvert.SerializeObject(req);
            byte[] buffer = Encoding.UTF8.GetBytes(json);

            WebRequest webRequest = WebRequest.Create(api);
            webRequest.Method = "POST";
            webRequest.Credentials = CredentialCache.DefaultCredentials;
            webRequest.ContentType = "application/json";

            if(!SetPostData(webRequest, buffer))
            {
                Log.Write("포스트 데이터 세팅 실패!");
                return;
            }
            // Request Data 만들기 <<--

            Request<Rpy_SignUp>(webRequest, recv_Rpy_SignUp);
            Log.Write("Send_Req_SignIn 끝 <<---------");
        }

        void recv_Rpy_SignIn(Rpy_SignIn rpy)
        {
            Log.Write("Recv_Rpy_SignIn 시작 --------->>");
            byte byRet = rpy.byRet;
            if (byRet == 1)
            {
                ChatApplicationContext.Recv_Rpy_SignIn(rpy);
            }
            else
            {
                ErrorMessage_Rpy_SignIn(byRet);
            }
            Log.Write("Recv_Rpy_SignIn 끝 <<---------");
        }

        void ErrorMessage_Rpy_SignIn(byte byRet)
        {
            if (byRet >= (byte)ERROR_MSG_COMMON.FAILURE && byRet < (byte)ERROR_MSG_COMMON.MAX)
            {
                ErrorMessage_Common(byRet);
            }
            else
            {
                string msg = string.Empty;
                switch ((ERROR_MSG_SIGNIN)byRet)
                {

                }

                Log.Write(msg);
                MessageBox.Show(msg);
            }
        }
        #endregion API

        bool SetPostData(WebRequest request, byte[] postParam)
        {
            try
            {
                // Post 데이터 세팅 -->>
                Stream requestStrream = request.GetRequestStream();
                requestStrream.Write(postParam, 0, postParam.Length);
                requestStrream.Close();
                // Post 데이터 세팅 <<--
            }
            catch (Exception ex)
            {
                Log.Write("#### Exception!!! THROWN ####");
                Log.Write(ex.Message);
                Log.Write("StackTrace :\n" + ex.StackTrace);
                Log.Write("#### Exception!!! THROWN ####");

                ErrorMessage_Common(0);
                return false;
            }

            return true;
        }
    }
}
