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
        string m_strID;
        string m_strPassword;

        public string strID { get { return m_strID; } }
        public string strPassword { get { return m_strPassword; } }
    
        public UserData_Account(string strID, string strPassword)
        {
            m_strID = strID;
            m_strPassword = strPassword;
        }
    }
}
