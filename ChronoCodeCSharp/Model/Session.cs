using System;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Xml.Serialization;

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
            this.ListTasks = new ArrayList();
            this.focusWatcher = null;
        }

        public Session(WatchDog.FocusWatcher focusWatcher)
        {
            this.CurrentTask = null;
            this.previousTask = null;
            this.isRunning = false;
            this.ListTasks = new ArrayList();
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
                }
            }
            else
            {
                t = new Task(ProcessName);
                Debug.WriteLine("New Task Created: " + ProcessName);
                t.addEntry();
                this.ListTasks.Add(t);

            }
            
            if(this.previousTask != null)
            {
                this.previousTask.closeLastPeriod();
                Debug.WriteLine("Close Period !");
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

        /** INPUTS **/
        private Task previousTask { get; set; }
        private Task CurrentTask { get; set; }
        public ArrayList ListTasks { get; set; }
        public Boolean isRunning;
        public ArrayList watchedTasks { get; set; }
        public ArrayList arrayListWatchedProcess { get; set; }

        [XmlIgnore]
        public WatchDog.FocusWatcher focusWatcher { get; set; }

    }
}
