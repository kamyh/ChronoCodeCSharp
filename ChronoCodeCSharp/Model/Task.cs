using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FocusChanged.Model
{
    [Serializable]
    public class Task
    {
        public Task()
        {
            this.periods = new List<Period>();
            //this.periods.Add(new Period());
            this.ProcessName = "";
        }

        public Task(String ProcessName)
        {
            this.periods = new List<Period>();
            //this.periods.Add(new Period());
            this.ProcessName = ProcessName;
        }

        public void addEntry()
        {
            this.periods.Add(new Period());
        }

        public void closeLastPeriod()
        {
            Period p = ((Period)this.periods[this.periods.Count - 1]);

            p.endDate = DateTime.Now;
            p.updateElapsedTime();
        }

        /** INPUT **/

        private int elapsedTime {get;set;}
        public String ProcessName { get; set; }
        public String name { get; set; }
        public String machineName { get; set; }
        public String id { get; set; }
        public List<Period> periods { get; set; }
        private Boolean isWatching {get;set;}
    }
}
