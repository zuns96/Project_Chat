using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Project_Chat
{
    public class UserData_Account
    {
        static UserData_Account s_intance = null;

        long m_lUserNo;
        string m_strUserName;

        static public UserData_Account Instance { get { return s_intance; } }

        public long lUserNo { get { return m_lUserNo; } }
        public string strUserName { get { return m_strUserName; } }

        static public void Create(long lUserNo, string strUserName)
        {
            if (s_intance == null)
                s_intance = new UserData_Account(lUserNo, strUserName);
        }

        static public void Release()
        {
            if (s_intance != null)
            {
                s_intance.release();
                s_intance = null;
            }
        }

        UserData_Account(long lUserNo, string strUserName)
        {
            m_lUserNo = lUserNo;
            m_strUserName = strUserName;
        }

        void release()
        {
            m_strUserName = null;
        }
    }
}
