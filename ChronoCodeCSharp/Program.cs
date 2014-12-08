using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Automation;
using System.Diagnostics;
using System.Runtime.InteropServices;
using FocusChanged.WatchDog;
using FocusChanged.Tools;

namespace FocusChanged
{
    static class Program
    {

        [STAThread]
        static void Main()
        {
            
            Application.EnableVisualStyles();  
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new WatchDog.FocusWatcher());
             

            //ProcessesAnalyser pA = new ProcessesAnalyser();
        }
    }
}
