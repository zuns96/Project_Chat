using System;
using System.IO;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json;
using static ASPDotNetCore.ASPPacket;

namespace Project_Chat
{
    public class NetASP : Net<NetASP>
    {
        private delegate void RecvResponse(string api, string json);

        private RecvResponse m_delegate = null;

        protected override string c_domain_base { get { return "http://127.0.0.1:12502{0}"; } }

        public NetASP() : base()
        {
            NetManager_ASP.Create();
            NetManager_ASP.AddListener_RequestFunc(request);
            m_delegate = new RecvResponse(do_recv_packet);
        }

        protected override void release()
        {
            NetManager_ASP.Release();
            NetManager_ASP.RemoveListener_RequestFunc(request);
            m_delegate = null;

            base.release();
        }

        private void request(WebRequest webRequest)
        {
            try
            {
                Task<WebResponse> asyncTask = webRequest.GetResponseAsync();
                Task task = new Task(() => waitForResponse(asyncTask));
                task.Start();
            }
            catch (Exception ex)
            {
                Log.Write("#### Exception!!! THROWN ####");
                Log.Write(ex.Message);
                Log.Write(ex.StackTrace);
                Log.Write("#### Exception!!! THROWN ####");
            }
        }

        private void waitForResponse(Task<WebResponse> asyncTask)
        {
            Log.Write("api Request 시작");
            string json = null;
            string api = null;
            bool isWait = true;
            asyncTask.ContinueWith((resultTask) =>
            {
                if (!resultTask.IsFaulted)
                {
                    WebResponse reps = resultTask.Result;
                    Stream stream = reps.GetResponseStream();
                    byte[] buffer = new byte[1024 * 1024];
                    ArraySegment<byte> segment = new ArraySegment<byte>(buffer);
                    int offset = 0;

                    do
                    {
                        offset += stream.Read(segment.Array, segment.Offset, segment.Count);

                    } while (stream.ReadByte() > 0);

                    json = Encoding.UTF8.GetString(segment.Array);
                    api = reps.ResponseUri.LocalPath;

                    Log.Write("api Request 성공");
                }
                else
                {
                    Exception ex = resultTask.Exception;
                    if (ex != null)
                    {
                        Log.Write("#### Exception!!! THROWN ####");
                        Log.Write(ex.Message);
                        Log.Write(ex.StackTrace);
                        Log.Write("#### Exception!!! THROWN ####");
                    }
                    Log.Write("api Request 실패");
                }

                isWait = false;
            });

            Log.Write("api Respone 대기중");
            while (isWait)
            {
                Thread.Sleep(10);
            }

            Log.Write("api Respone 처리 시작");
            Form mainForm = ChatApplicationContext.mainForm;
            mainForm.Invoke(m_delegate, api, json);
            Log.Write("api Respone 처리 끝");
        }
        
        private void do_recv_packet(string api, string json)
        {
            switch (api)
            {
                case APIDefine.API_MEMBER_CHECK:
                    {
                        Rpy_MemberCheck rpy = JsonConvert.DeserializeObject<Rpy_MemberCheck>(json);
                        do_recv_rpy_memberCheck(rpy);
                    }
                    break;
                case APIDefine.API_SIGNUP:
                    {
                        Rpy_SignUp rpy = JsonConvert.DeserializeObject<Rpy_SignUp>(json);
                        do_recv_rpy_signUp(rpy);
                    }
                    break;
                case APIDefine.API_SIGNIN:
                    {
                        Rpy_SignIn rpy = JsonConvert.DeserializeObject<Rpy_SignIn>(json);
                        do_recv_rpy_signIn(rpy);
                    }
                    break;
                default:
                    {

                    }
                    break;
            }
        }

        private void do_recv_rpy_memberCheck(Rpy_MemberCheck rpy)
        {
            Log.Write("do_recv_rpy_memberCheck -------->>");

            Log.Write("byRet({0})",
                rpy.byRet);

            NetManager_ASP.Recv_Rpy_MemberCheck(rpy);

            Log.Write("do_recv_rpy_memberCheck <<--------");
        }

        private void do_recv_rpy_signUp(Rpy_SignUp rpy)
        {
            Log.Write("do_recv_rpy_signUp -------->>");

            Log.Write("byRet({0}),lUserNo({1}),strUserName({2})",
                rpy.byRet,
                rpy.lUserNo,
                rpy.strUserName);

            NetManager_ASP.Recv_Rpy_SignUp(rpy);

            Log.Write("do_recv_rpy_signUp <<--------");
        }

        private void do_recv_rpy_signIn(Rpy_SignIn rpy)
        {
            Log.Write("do_recv_rpy_signIn -------->>");

            Log.Write("byRet({0}),lUserNo({1}),strUserName({2})",
                rpy.byRet,
                rpy.lUserNo,
                rpy.strUserName);

            NetManager_ASP.Recv_Rpy_SignIn(rpy);

            Log.Write("do_recv_rpy_signIn <<--------");
        }
    }
}
