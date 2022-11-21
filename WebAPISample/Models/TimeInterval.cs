using Microsoft.CodeAnalysis.CSharp.Syntax;
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

        public TimeInterval(Times timeStump)
        {
            this.cycleID = timeStump.cycleID;
            this.startTime = timeStump.start;

            foreach (var e in timeStump.getTimeArray())
            {

            }
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

        private void insertTime(int index, TimeSpan time)
        {
            switch (index)
            {
                case 0: this.time_carryIn = time; break;
                case 1: this.time_waitShoot = time; break;
                case 2: this.time_shoot = time; break;
                case 3: this.time_stock = time; break;
                case 4: this.time_waitArm1 = time; break;
                case 5: this.time_recipt = time; break;
                case 6: this.time_readRFID = time; break;
                case 7: this.time_put = time; break;
                case 8: this.time_carryOut = time; break;
            }
        }
    }
}