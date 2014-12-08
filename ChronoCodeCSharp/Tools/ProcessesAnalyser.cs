using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FocusChanged.Tools
{
    class ProcessesAnalyser
    {
        public ProcessesAnalyser()
        {
            init();

            banProcess();
        }

        private void banProcess()
        {
            foreach (Process p in processes)
            {
                if(!this.processBanned.Contains(p.ProcessName))
                {
                    this.processList.Add(p);
                }
            }
        }

        private void init()
        {
            DataStream dS = new DataStream("./listProcess.txt");
            DataStream dSBannedProcess = new DataStream("./ban.txt");
            processList = new ArrayList();

            processes = Process.GetProcesses();

            processBanned = dSBannedProcess.getBannedProcess();
        }

        public Process[] processes;
        public ArrayList processList;
        public ArrayList processBanned;
    }
}
