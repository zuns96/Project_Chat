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

        string m_strID;

        static public UserData_Account Instance { get { return s_intance; } }

        public string strID { get { return m_strID; } }

        static public void Create(string strID)
        {
            if (s_intance == null)
                s_intance = new UserData_Account(strID);
        }

        static public void Release()
        {
            if (s_intance != null)
            {
                s_intance.release();
                s_intance = null;
            }
        }

        UserData_Account(string strID)
        {
            m_strID = strID;
        }

        void release()
        {
            m_strID = null;
        }
    }
}
