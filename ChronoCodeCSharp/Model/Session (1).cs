using System;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FocusChanged.Model
{
    class Session
    {
        public Session()
        {
            this.CurrentTask = null;
            this.previousTask = null;
            this.ListTasks = new ArrayList();
        }

        private Boolean isExistingTask(String name)
        {
            for (int i = 0; i < this.ListTasks.Count; i++)
            {
                if (((Task)this.ListTasks[i]).name == name)
                {
                    return true;
                }
            }
            return false;
        }

        private Task getTaskByName(String name)
        {
            for (int i = 0; i < this.ListTasks.Count; i++)
            {
                if (((Task)this.ListTasks[i]).name == name)
                {
                    return ((Task)this.ListTasks[i]);
                }
            }
            return null;
        }

        public void update(string name)
        {
            Task t;
            
            if(isExistingTask(name))
            {
                //t = getTaskByName(name);
            }
            else
            {
                //t = new Task(name);
                //t.addEntry();
                //this.ListTasks.Add(t);
            }
            /*
            if(this.previousTask != null)
            {
                this.previousTask.closeLastPeriod();
            }

            this.previousTask = t;*/
        }

        /** INPUTS **/
        private Task previousTask { get; set; }
        private Task CurrentTask { get; set; }
        private ArrayList ListTasks { get; set; }

        
    }
}
