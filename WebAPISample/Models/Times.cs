namespace WebAPISample.Models
{
    /*hello*/
    public class Times
    {
        public Times(TimeSpan[] times)
        {
            if(times.Length != 10)
            {
                return;
            }
            
            start = times[0];
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

        public TimeSpan start { get; set; }
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
