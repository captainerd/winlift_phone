using System.Diagnostics;

namespace CallerIdentity
{

    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Process thisProc = Process.GetCurrentProcess();
            if (Process.GetProcessesByName(thisProc.ProcessName).Length > 1)
            {
            //    MessageBox.Show("Application running");
            //    Application.Exit();
                return;
            }

          
                ApplicationConfiguration.Initialize();
                Application.Run(new Form1());
         
          
            // To customize application configuration such as set high DPI settings or default font,
            // see https://aka.ms/applicationconfiguration.
         
        }
    }
}