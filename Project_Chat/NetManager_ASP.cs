using ASPDotNetCore.Models;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Newtonsoft.Json;

namespace Project_Chat
{
    public partial class NetManager_ASP : Net<NetManager_ASP>
    {
        protected override string c_domain_base { get { return "http://zuns96.iptime.org:12502/api/{0}"; } }

        #region API
        void send_Req_Member_Check(string strID)
        {
            string api = string.Format(c_domain_base, "auth/memberCheck");

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
            
            // Post 데이터 세팅 -->>
            Stream requestStrream = webRequest.GetRequestStream();
            requestStrream.Write(buffer, 0, buffer.Length);
            requestStrream.Close();
            // Post 데이터 세팅 <<--
            // Request Data 만들기 <<--

            Task<WebResponse> asyncTask = webRequest.GetResponseAsync();
            WaitForResponse<Rpy_MemberCheck>(asyncTask, recv_Rpy_MemberCheck);
        }

        void recv_Rpy_MemberCheck(Rpy_MemberCheck rpy)
        {
            ChatApplicationContext.Recv_Rpy_MemberCheck(rpy);
        }

        void send_Req_SignUp(string strID, string strPassword)
        {
            string api = string.Format(c_domain_base, "auth/signUp");

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

            // Post 데이터 세팅 -->>
            Stream requestStrream = webRequest.GetRequestStream();
            requestStrream.Write(buffer, 0, buffer.Length);
            requestStrream.Close();
            // Post 데이터 세팅 <<--
            // Request Data 만들기 <<--

            Task<WebResponse> asyncTask = webRequest.GetResponseAsync();
            WaitForResponse<Rpy_SignUp>(asyncTask, recv_Rpy_SignUp);
        }

        void recv_Rpy_SignUp(Rpy_SignUp rpy)
        {
            if (rpy.byRet == 1)
            {
                ChatApplicationContext.Recv_Rpy_SignUp(rpy);
            }
        }

        void send_Req_SignIn(string strID, string strPassword)
        {
            string api = string.Format(c_domain_base, "auth/signIn");

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

            // Post 데이터 세팅 -->>
            Stream requestStrream = webRequest.GetRequestStream();
            requestStrream.Write(buffer, 0, buffer.Length);
            requestStrream.Close();
            // Post 데이터 세팅 <<--
            // Request Data 만들기 <<--

            Task<WebResponse> asyncTask = webRequest.GetResponseAsync();
            WaitForResponse<Rpy_SignIn>(asyncTask, recv_Rpy_SignIn);
        }

        void recv_Rpy_SignIn(Rpy_SignIn rpy)
        {
            if (rpy.byRet == 1)
            {
                ChatApplicationContext.Recv_Rpy_SignIn(rpy);
            }
        }
        #endregion API
    }
}
