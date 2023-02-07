namespace WebAPISample.Models
{
    /// <summary>
    /// 作業の期間で表示するときに、その時間を計算してデータを作成する
    /// </summary>
    public class TimeInterval
    {
        /// <summary>
        ///  開始時刻、タイムスタンプからかかった時間を計算する
        ///  途中取れなかったデータは考える
        /// </summary>
        /// <param name="id"> ワークのID </param>
        /// <param name="times"> タイムスタンプの配列 </param>
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
            this.cycleID = timeStump.workId;
            this.startTime = timeStump.supply;

            foreach (var e in timeStump.getTimeArray())
            {
            }
        }
        /// <summary>
        /// サイクルID。行われた検査ごとに作られる
        /// </summary>
        public int cycleID { get; set; }
        /// <summary>
        ///  検査開始時刻
        ///  搬送コンベアに乗せられた(センサに触れた)時間。
        /// </summary>
        public DateTime startTime { set; get; }
        /// <summary>
        ///  コンベアで運ばれている時間
        ///  (コンベアに乗せられてから、ポジショニングされるまでの時間)
        /// </summary>
        public TimeSpan time_carryIn { get; set; }
        /// <summary>
        ///  ロボットアーム、撮影を待っている時間
        ///  (ポジショニングから撮影開始まで)
        /// </summary>
        public TimeSpan time_waitShoot { get; set; }
        /// <summary>
        ///  写真撮影が行われている時間
        ///  (1枚目撮影から撮影終了まで)
        /// </summary>
        public TimeSpan time_shoot { get; set; }
        /// <summary>
        ///  コンベアの終端まで運ばれている時間
        ///  (撮影終了からコンベア最後の在荷センサ反応まで)
        /// </summary>
        public TimeSpan time_stock { get; set; }
        /// <summary>
        ///  アームが来て、ワークをつかんで運ばれるまで待っている時間
        ///  (搬入コンベア終端の在荷センサが反応している時間)
        /// </summary>
        public TimeSpan time_waitArm1 { get; set; }
        /// <summary>
        ///  ワークが持ち替え部まで運ばれている時間
        ///  (コンベアから離れてから、持ち替え部に置かれるまで)
        /// </summary>
        public TimeSpan time_recipt { get; set; }
        /// <summary>
        ///  ワークがRFID読み取り部に運ばれる時間
        ///  (ワークが持ち替え部から離れてから、RFID読み取り完了まで)
        /// </summary>
        public TimeSpan time_readRFID { get; set; }
        /// <summary>
        ///  ワークが搬出コンベアに運ばれる時間
        ///  (RFID読み取りから、搬出コンベアセンサに触れるまで)
        /// </summary>
        public TimeSpan time_put { get; set; }
        /// <summary>
        ///  搬出コンベアを流れる時間
        ///  (搬出コンベアに置かれてから、コンベア最終地点のセンサに触れるまで)
        /// </summary>
        public TimeSpan time_carryOut { get; set; }

        /// <summary>
        ///  時間を個別でセットする。
        ///  PLCからもらった情報を追加する?
        /// </summary>
        /// <param name="index">  </param>
        /// <param name="time"></param>
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