using System;
using System.Threading;
using System.Threading.Tasks;

namespace Project_Chat
{
    public class TimeManager
    {
        readonly DateTime c_utcBaseTime;

        static TimeManager s_instance = null;

        DateTime m_serverTime;
        DateTime m_serverTimeUTC;

        Task m_timeManagerLoopThread = null;

        static public long TimeStamp { get { return s_instance == null ? (long)(DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc)).TotalSeconds : s_instance.lTimeStamp; } }
        long lTimeStamp { get { return (long)(m_serverTimeUTC - c_utcBaseTime).TotalSeconds; } }

        object m_objLock = null;

        static public void Create()
        {
            if (s_instance == null)
                s_instance = new TimeManager();
        }

        static public string GetNowTimeString(bool isUTC, string format = "yyyy/MM/dd HH:mm:ss.ffff")
        {
            if (s_instance != null)
                s_instance = null;

            return isUTC ? DateTime.UtcNow.ToString(format) : DateTime.Now.ToString(format);
        }

        string getNowTimeString(bool isUTC, string format)
        {
            if(isUTC)
            {
                return m_serverTime.ToString(format);
            }
            else
            {
                return m_serverTimeUTC.ToString(format);
            }
        }

        public TimeManager()
        {
            c_utcBaseTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            m_serverTime = DateTime.Now;
            m_serverTimeUTC = DateTime.UtcNow;

            m_objLock = new object();

            m_timeManagerLoopThread = new Task(Update);
            m_timeManagerLoopThread.Start();
        }

        void Update()
        {
            while (true)
            {
                Thread.Sleep(1000);
                lock (m_objLock)
                {
                    m_serverTimeUTC = m_serverTimeUTC.AddSeconds(1);
                    m_serverTime = m_serverTimeUTC.ToLocalTime();
                }
            }
        }
    }
}
