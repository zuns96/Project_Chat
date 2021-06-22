using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Project_Chat
{
    static class Program
    {
        static ApplicationContext s_applicationContext = null;

        static public ApplicationContext ApplicartionContext { get { return s_applicationContext; } }

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            s_applicationContext = new ApplicationContext(new WinFormLogin());

            Application.Run(s_applicationContext);
        }
    }
}
