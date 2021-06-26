using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Project_Chat
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            NetManager_ASP.Create();
            ChatApplicationContext chatApplicationContext = new ChatApplicationContext(new WinFormLogin());

            Application.Run(chatApplicationContext);
        }
    }
}
