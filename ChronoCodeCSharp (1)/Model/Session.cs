using System;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Xml.Serialization;
using System.IO;
using System.Collections.Generic;

namespace FocusChanged.Model
{
    [Serializable]
    public class Session
    {
        public Session()
        {
            this.CurrentTask = null;
            this.previousTask = null;
            this.isRunning = false;
            this.ListTasks = new List<Task>();
            this.focusWatcher = null;
        }

        public Session(WatchDog.FocusWatcher focusWatcher)
        {
            this.CurrentTask = null;
            this.previousTask = null;
            this.isRunning = false;
            this.ListTasks = new List<Task>();
            this.focusWatcher = focusWatcher;
        }

        public void addFocusWatcher(WatchDog.FocusWatcher focusWatcher)
        {
            this.focusWatcher = focusWatcher;
        }

        private Boolean isExistingTask(String ProcessName)
        {
            for (int i = 0; i < this.ListTasks.Count; i++)
            {
                if (((Task)this.ListTasks[i]).ProcessName == ProcessName)
                {
                    return true;
                }
            }
            return false;
        }

        private Task getTaskByName(String ProcessName)
        {
            for (int i = 0; i < this.ListTasks.Count; i++)
            {
                if (((Task)this.ListTasks[i]).ProcessName == ProcessName)
                {
                    return ((Task)this.ListTasks[i]);
                }
            }
            return null;
        }

        public void update(string ProcessName,String name,String id,String MachineName)
        {
            Task t;

            if (isExistingTask(ProcessName))
            {
                t = getTaskByName(ProcessName);

                if(this.previousTask != t)
                {
                    t.addEntry();
                    Debug.WriteLine("ADD ENTRY TO TASK " + t.name);
                }
            }
            else
            {
                t = new Task(ProcessName);
                Debug.WriteLine("NEW TASK " + ProcessName);
                t.addEntry();
                this.ListTasks.Add(t);

            }
            
            if(this.previousTask != null)
            {
                this.previousTask.closeLastPeriod();
                Debug.WriteLine("CLOSE PERIOD");
            }

            this.previousTask = t;
        }

        public int getTotElapsedTime()
        {
            int totElapsedTime = 0;

            foreach (Task t in this.ListTasks)
            {
                foreach (Period p in t.periods)
                {
                    totElapsedTime += p.elapsedTimeSec;
                }
            }

            return totElapsedTime;
        }

        public void saveWatchedTasks(ArrayList arrayListWatchedProcess)
        {
            this.arrayListWatchedProcess = new ArrayList();
            this.arrayListWatchedProcess = arrayListWatchedProcess;
        }

        static public void serializeToXML(Session session, string path)
        {
            try
            {
                XmlSerializer serializer = new XmlSerializer(typeof(Session));
                TextWriter textWriter = new StreamWriter(path);
                serializer.Serialize(textWriter, session);
                textWriter.Close();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());
            }
        }

        internal void setPreviousTaskNull()
        {
            if (this.previousTask != null)
            {
                this.previousTask.closeLastPeriod();
                Debug.WriteLine("CLOSE PERIOD");
            }

            this.previousTask = null;
        }

        /** INPUTS **/
        private Task previousTask { get; set; }
        private Task CurrentTask { get; set; }

        
        public List<Task> ListTasks { get; set; }
        public Boolean isRunning;
        public ArrayList watchedTasks { get; set; }
        public ArrayList arrayListWatchedProcess { get; set; }

        [XmlIgnore]
        public WatchDog.FocusWatcher focusWatcher { get; set; }



    }
}
