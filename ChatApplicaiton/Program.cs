using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;

namespace ChatApplicaiton
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(){
			string strProcessName = Process.GetCurrentProcess().ProcessName;

			Process[] processCollection = Process.GetProcessesByName(strProcessName);

			if(processCollection.Length > 1){
				MessageBox.Show("Application: " + strProcessName + " is already running");
			}
			else{
				Application.EnableVisualStyles();
				Application.SetCompatibleTextRenderingDefault(false);
				Application.Run(new Form1());
			}
        }
    }
}
