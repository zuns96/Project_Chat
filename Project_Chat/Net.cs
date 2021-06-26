﻿using System;
using System.Threading;
using System.Threading.Tasks;
using System.IO;
using System.Text;
using System.Net;
using Newtonsoft.Json;

namespace Project_Chat
{
    public abstract class Net
    {
        protected bool isWait = false;
        protected abstract string c_domain_base { get; }

        protected void WaitForResponse<T>(Task<WebResponse> asyncTask, Action<T> recvFunc) where T : class
        {
            T rpy = null;
            string json = string.Empty;
            
            isWait = true;
            asyncTask.ContinueWith((resultTask) =>
            {
                if(resultTask.IsCompleted)
                {
                    WebResponse reps = resultTask.Result;
                    Stream stream = reps.GetResponseStream();
                    byte[] buffer = new byte[256];
                    int offset = 0;
                    do
                    {
                        offset += stream.Read(buffer, offset, 256);
                        json += Encoding.UTF8.GetString(buffer);
                    } while (stream.ReadByte() > 0);
                    rpy = JsonConvert.DeserializeObject<T>(json);
                }
                else
                {

                }

                isWait = false;
            });

            while(isWait)
            {
                Thread.Sleep(10);
            }

            recvFunc(rpy);
        }
    }
}