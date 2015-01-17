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

        /**
         * Refresh elapsedTimeSec and elapsedTimeMili attributes
         * 
         **/
        public void updateElapsedTime()
        {
            TimeSpan span = this.endDate.Subtract(this.startDate);
            this.elapsedTimeSec = span.Seconds;
            this.elapsedTimeMili = span.Milliseconds;
        }

        /** INUPTS **/
        public DateTime startDate {get;set;}
        public DateTime endDate { get; set; }
        public int elapsedTimeSec { get; set; }
        public int elapsedTimeMili { get; set; }
    }
}
