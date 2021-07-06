using System;
using System.Threading;
using System.Threading.Tasks;
using System.IO;
using System.Text;
using System.Net;
using Newtonsoft.Json;
using System.Windows.Forms;

namespace Project_Chat
{
    public abstract class Net<T> where T : class, new()
    {
        static protected T s_instance = null;

        protected bool isWait = false;
        protected abstract string c_domain_base { get; }

        static public void Create()
        {
            if (s_instance == null)
                s_instance = new T();
        }

        static public void Release()
        {
            if (s_instance != null)
                s_instance = null;
        }

        protected void WaitForResponse<TPacketType>(Task<WebResponse> asyncTask, Action<TPacketType> recvFunc) where TPacketType : class
        {
            TPacketType rpy = null;

            Log.Write("api Request 시작");
            isWait = true;
            asyncTask.ContinueWith((resultTask) =>
            {
                if(!resultTask.IsFaulted)
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

            while(isWait)
            {
                Log.Write("api Respone 대기중");
                Thread.Sleep(10);
            }

            Log.Write("api Respone 처리 시작");
            Form mainForm = ChatApplicationContext.mainForm;
            mainForm.Invoke(recvFunc, rpy);
            Log.Write("api Respone 처리 끝");
        }
    }
}
