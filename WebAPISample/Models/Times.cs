using System.Diagnostics;

namespace WebAPISample.Models
{
    /// <summary>
    /// サイクルタイムを、工程ごとの時刻で表示させるときの表示データ
    /// </summary>
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
        /// <summary>
        /// サイクルID。行われた検査ごとに作られる
        /// </summary>
        public int cycleID { get; set; }
        /// <summary>
        /// 搬送コンベアに乗せられた(センサに触れた)時間。
        /// ワークを代表する検査時間はこの時間が適用される
        /// </summary>
        public DateTime start { get; set; }
        /// <summary>
        /// ワークが写真撮影場所で位置決めされた時
        /// </summary>
        public TimeSpan position { get; set; }
        /// <summary>
        /// 写真撮影の1枚目が行われた時
        /// </summary>
        public TimeSpan shootStart { get; set; }
        /// <summary>
        /// 写真撮影の最後の1枚が行われた時
        /// </summary>
        public TimeSpan shootEnd { get; set; }
        /// <summary>
        /// ワークがコンベアの最後の在荷センサに触れたとき
        /// </summary>
        public TimeSpan stock { get; set; }
        /// <summary>
        /// ワークがロボットの2つ目の腕でつかまれた時
        /// (受け渡し部分の在荷センサが反応しなくなった瞬間)
        /// </summary>
        public TimeSpan recipt { get; set; }
        /// <summary>
        /// RFIDでICタグのデータが読み取れた瞬間
        /// </summary>
        public TimeSpan readRFID { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public TimeSpan defrred { get; set; }
        public TimeSpan carryOut { get; set; }
        public TimeSpan end { get; set; }

        public TimeSpan[] getTimeArray()
        {
            var timearray = new TimeSpan[10];
            timearray[0] = position;
            timearray[1] = shootStart;
            timearray[2] = shootEnd;
            timearray[3] = stock;
            timearray[4] = recipt;
            timearray[5] = readRFID;
            timearray[6] = defrred;
            timearray[7] = carryOut;
            timearray[8] = end;
            return timearray;
        }

    }
}