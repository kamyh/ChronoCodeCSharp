using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FocusChanged.Model
{
    [Serializable]
    public class Period
    {
        public Period()
        {
            this.elapsedTimeSec = 0;
            this.startDate = DateTime.Now;
        }

        public void updateElapsedTime()
        {
            TimeSpan span = this.endDate.Subtract(this.startDate);
            this.elapsedTimeSec = span.Seconds;
            this.elapsedTimeMili = span.Milliseconds;
            Debug.WriteLine("---> " + this.elapsedTimeSec);
            Debug.WriteLine("---> " + this.elapsedTimeMili);
        }



        /** INUPTS **/

        public DateTime startDate {get;set;}
        public DateTime endDate { get; set; }
        public int elapsedTimeSec { get; set; }
        public int elapsedTimeMili { get; set; }
    }
}
