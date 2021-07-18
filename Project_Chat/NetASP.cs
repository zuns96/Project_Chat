using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json;

namespace Project_Chat
{
    public class NetASP : Net<NetASP>
    {
        protected override string c_domain_base => throw new NotImplementedException();

        public NetASP() : base()
        {

        }

        private void Request<TPacketType>(WebRequest webRequest, Action<TPacketType> recvFunc) where TPacketType : class
        {
            try
            {
                Task<WebResponse> asyncTask = webRequest.GetResponseAsync();
                WaitForResponse<TPacketType>(asyncTask, recvFunc);
            }
            catch (Exception ex)
            {
                Log.Write("#### Exception!!! THROWN ####");
                Log.Write(ex.Message);
                Log.Write(ex.StackTrace);
                Log.Write("#### Exception!!! THROWN ####");
            }
        }

        private void WaitForResponse<TPacketType>(Task<WebResponse> asyncTask, Action<TPacketType> recvFunc) where TPacketType : class
        {
            TPacketType rpy = null;

            Log.Write("api Request 시작");
            isWait = true;
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
                        offset += stream.Read(segment.Array, offset, 1024 * 1024);

                    } while (stream.ReadByte() > 0);

                    string json = Encoding.UTF8.GetString(segment.Array);
                    rpy = JsonConvert.DeserializeObject<TPacketType>(json);

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
            mainForm.Invoke(recvFunc, rpy);
            Log.Write("api Respone 처리 끝");
        }
    }
}
