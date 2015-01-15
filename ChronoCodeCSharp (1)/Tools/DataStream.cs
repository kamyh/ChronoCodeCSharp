﻿using FocusChanged.Model;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;


namespace FocusChanged.Tools
{
    class DataStream
    {
        public DataStream()
        {

        }

        public DataStream(String filename)
        {
            this.filename = filename;
        }

        public void toLogs(List<Task> tasks)
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

        public void exportCsv(Session session)
        {
            SaveFileDialog file = new SaveFileDialog();

            file.InitialDirectory = "./";
            file.Filter = "csv files (*.csv)|*.csv|All files (*.*)|*.*";
            file.RestoreDirectory = true;

            string filePath;

            if (file.ShowDialog() == DialogResult.OK)
            {
                filePath = file.FileName;

                if (!File.Exists(filePath))
                {
                    File.Create(filePath).Close();
                }
                string delimiter = ";";
                StringBuilder sb = new StringBuilder();

                foreach(Task t in session.ListTasks)
                {
                    foreach(Period p in t.periods)
                    {
                        String[] s = new string[] { t.ProcessName, p.startDate.ToString(), p.endDate.ToString(), p.elapsedTimeSec.ToString() };
                        sb.AppendLine(string.Join(delimiter, s));
                    }
                }

                File.WriteAllText(filePath, sb.ToString());
            }
        }

        /** INPUTS **/
        private String filename { get; set; }




        
    }

}
