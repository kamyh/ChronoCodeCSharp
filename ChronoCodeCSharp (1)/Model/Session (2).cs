using System;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace FocusChanged.Model
{
    class Session
    {

        public Session(WatchDog.FocusWatcher focusWatcher)
        {
            this.CurrentTask = null;
            this.previousTask = null;
            this.ListTasks = new ArrayList();
            this.focusWatcher = focusWatcher;

        }


        private Boolean isExistingTask(String name)
        {
            foreach(Task taskToAnalyse in this.ListTasks)
            {
                if(taskToAnalyse.name == name)
                {
                    return true;
                }
            }
            return false;
        }

        private Task getTaskByName(String name)
        {
            foreach (Task taskToAnalyse in this.ListTasks)
            {
                if (taskToAnalyse.name == name)
                {
                    return taskToAnalyse;
                }
            }
            return null;
        }
        public void update(string ProcessName, string name, string id, string MachineName)
        {
            Debug.WriteLine("--> "+name);

            Task t;
            
            if(isExistingTask(name))
            {
                t = getTaskByName(name);
            }
            else
            {
                t = new Task(name);
                t.addEntry();
                this.ListTasks.Add(t);
            }
            
            if(this.previousTask != null)
            {
                this.previousTask.closeLastPeriod();
            }

            this.previousTask = t;
            Debug.WriteLine("Task Name: "+t.name);
        }

        /** INPUTS **/
        public Task previousTask { get; set; }
        public Task CurrentTask { get; set; }
        public ArrayList ListTasks { get; set; }
        private WatchDog.FocusWatcher focusWatcher;

    }
}
