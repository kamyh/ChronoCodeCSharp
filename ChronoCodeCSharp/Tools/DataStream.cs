using FocusChanged.Model;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;


namespace FocusChanged.Tools
{
    class DataStream
    {

        public DataStream(String filename)
        {
            this.filename = filename;
        }

        public void toLogs(ArrayList tasks)
        {
            TextWriter tw = new StreamWriter(this.filename);
            String line = "";
            int totElapsedTime = 0;

            foreach (Task t in tasks)
            {
                foreach (Period p in t.periods)
                {
                    totElapsedTime += p.elapsedTimeSec;
                }

                line += t.ProcessName;
                line += " : " + totElapsedTime;

                tw.WriteLine(line);
                line = "";
            }

            tw.Close();
        }

        public void processListToTXT(ArrayList processlist)
        {
            TextWriter tw = new StreamWriter(this.filename);

            foreach (Process p in processlist)
            {
                tw.WriteLine("Process: {0} ID: {1}", p.ProcessName, p.Id);
            }

            tw.Close();
        }

        public ArrayList getBannedProcess()
        {
            TextReader tR = new StreamReader(this.filename);
            ArrayList bannedProcess = new ArrayList();

            while (tR.Peek() >= 0)
            {
                bannedProcess.Add(tR.ReadLine());
            }

            tR.Close();

            return bannedProcess;
        }

        public void addBan(String ban)
        {
            using (StreamWriter sw = File.AppendText(this.filename))
            {
                sw.WriteLine(ban);

                sw.Close();
            }	
        }

        public void updateBanList(ArrayList processlist)
        {
            TextWriter tw = new StreamWriter(this.filename);

            foreach (String p in processlist)
            {
                tw.WriteLine(p);
            }

            tw.Close();
        }

        public void resetFile()
        {
            TextWriter tw = new StreamWriter(this.filename);

            tw.Close();
        }

        /** INPUTS **/
        private String filename { get; set; }




        
    }

}
