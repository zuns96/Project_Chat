using System;
using System.Windows.Forms;
using Project_Chat.Net;

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
            TimeManager.Create();
            Log.Create();

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            NetWS.Create();
            NetASP.Create();
            ChatApplicationContext chatApplicationContext = new ChatApplicationContext(new WinFormLogin());

            Application.Run(chatApplicationContext);
        }
    }
}
