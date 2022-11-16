using System.Diagnostics;

namespace WebAPISample.Models
{
    /// <summary>
    /// 作業の期間で表示するときに、その時間を計算してデータを作成する
    /// </summary>
    public class TimeInterval
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="times"></param>
        public TimeInterval(int id, DateTime time, TimeSpan[] times)
        {
            if (times.Length != 9)
            {
                return;
            }
            this.cycleID = id;
            this.startTime = time;
            this.time_carryIn = times[0];
            this.time_waitShoot = times[1];
            this.time_shoot = times[2];
            this.time_stock = times[3];
            this.time_waitArm1 = times[4];
            this.time_recipt = times[5];
            this.time_readRFID = times[6];
            this.time_put = times[7];
            this.time_carryOut = times[8];
        }

        public TimeInterval(int id, DateTime start, TimeSpan[] times)
        {
            if (times.Length != 8)
            {
                return;
            }
            this.cycleID = id;
            this.startTime = start;
            this.time_carryIn = times[0];
            this.time_waitShoot = times[1];
            this.time_shoot = times[2];
            this.time_stock = times[3];
            this.time_waitArm1 = times[4];
            this.time_recipt = times[5];
            this.time_readRFID = times[6];
            this.time_put = times[7];
            this.time_carryOut = times[8];
        }
        public int cycleID { get; set; }
        public DateTime startTime { set; get; }
        public TimeSpan time_carryIn { get; set; }
        public TimeSpan time_waitShoot { get; set; }
        public TimeSpan time_shoot { get; set; }
        public TimeSpan time_stock { get; set; }
        public TimeSpan time_waitArm1 { get; set; }
        public TimeSpan time_recipt { get; set; }
        public TimeSpan time_readRFID { get; set; }
        public TimeSpan time_put { get; set; }
        public TimeSpan time_carryOut { get; set; }
    }
}