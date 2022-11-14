using System.Diagnostics;

namespace WebAPISample.Models
{
    /*hello*/
    public class Times
    {
        public Times(int id, TimeSpan[] times)
        {
            if (times.Length != 10)
            {
                return;
            }
            this.cycleID = id;
            start = DateTime.Today;
            start += times[0];
            position = times[1];
            shootStart = times[2];
            shootEnd = times[3];
            stock = times[4];
            recipt = times[5];
            readRFID = times[6];
            defrred = times[7];
            carryOut = times[8];
            end = times[9];
        }

        public Times(int id, DateTime start, TimeSpan[] times)
        {
            if (times.Length != 9)
            {
                return;
            }
            this.cycleID = id;
            this.start = start;
            position = times[0];
            shootStart = times[1];
            shootEnd = times[2];
            stock = times[3];
            recipt = times[4];
            readRFID = times[5];
            defrred = times[6];
            carryOut = times[7];
            end = times[8];
        }
        public int cycleID { get; set; }
        public DateTime start { get; set; }
        public TimeSpan position { get; set; }
        public TimeSpan shootStart { get; set; }
        public TimeSpan shootEnd { get; set; }
        public TimeSpan stock { get; set; }
        public TimeSpan recipt { get; set; }
        public TimeSpan readRFID { get; set; }
        public TimeSpan defrred { get; set; }
        public TimeSpan carryOut { get; set; }
        public TimeSpan end { get; set; }
    }
}
