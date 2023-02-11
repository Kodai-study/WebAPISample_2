namespace WebAPISample.JSONModels
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
            if (times.Length != Times.COLUM_NUMBER)
            {
                return;
            }
            this.cycleID = id;
            this.startTime = time;
            this.time_supply = times[0];
            this.time_visualStation = times[1];
            this.time_functionalStation = times[2];
            this.time_assemblyStation = times[3];
        }

        public TimeInterval(Times timeStump)
        {
            this.cycleID = timeStump.workId;
            DateTime?[] timeStumps = timeStump.getTimeArray();

            if (timeStumps[0] == null)
                return;

            //供給開始時間は必ずデータが存在する
            this.startTime = timeStump.supply;

            for (int i = 1; i < timeStumps.Length; i++)
            {
                if (timeStumps[i] == null || timeStumps[i - 1] == null)
                    continue;

                insertTime(i - 1, timeStumps[i] - timeStumps[i - 1]);
            }


        }
        /// <summary>
        /// サイクルID。行われた検査ごとに作られる
        /// </summary>
        public int cycleID { get; set; }
        /// <summary>
        ///  検査開始時刻
        ///  搬送コンベアに乗せられた(センサに触れた)時間。
        ///  
        /// </summary>
        public DateTime startTime { set; get; }
        /// <summary>
        ///  コンベアで運ばれている時間
        ///  (コンベアに乗せられてから、ポジショニングされるまでの時間)
        /// </summary>
        public TimeSpan? time_supply { get; set; }
        /// <summary>
        ///  ロボットアーム、撮影を待っている時間
        ///  (ポジショニングから撮影開始まで)
        /// </summary>
        public TimeSpan? time_visualStation { get; set; }
        /// <summary>
        ///  写真撮影が行われている時間
        ///  (1枚目撮影から撮影終了まで)
        /// </summary>
        public TimeSpan? time_functionalStation { get; set; }

        /// <summary>
        ///  時間を指定する
        /// </summary>
        public TimeSpan? time_assemblyStation { get; set; }

        /// <summary>
        ///  時間を個別でセットする。
        ///  PLCからもらった情報を追加する?
        /// </summary>
        /// <param name="index">  </param>
        /// <param name="time"></param>
        private void insertTime(int index, TimeSpan? time)
        {
            switch (index)
            {
                case 0: this.time_supply = time; break;
                case 1: this.time_visualStation = time; break;
                case 2: this.time_functionalStation = time; break;
                case 3: this.time_assemblyStation = time; break;
            }
        }
    }
}