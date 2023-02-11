using static WebAPISample.JSONModels.Result;

namespace WebAPISample.JSONModels
{
    /// <summary>
    /// サイクルタイムを、ステーションへの搬入、搬出時間で表したモデル
    /// </summary>
    public class Times
    {
        /// <summary>
        ///  取得する時刻データの数。検査開始+各ステーションの搬入、搬出時間
        /// </summary>
        public const int COLUM_NUMBER = 5;


        /// <summary>
        /// サイクルID。行われた検査ごとに作られる
        /// </summary>
        public int workId { get; set; }

        /// <summary>
        /// 供給ロボットがワークを保持して、検査組み立てが始まった時刻。
        /// </summary>
        public DateTime supply { get; set; }

        /// <summary>
        ///  外観検査ステーションへの搬入時刻。
        ///  搬入コンベアの搬入部の線さに触れた時間
        /// </summary>
        public DateTime? Visal_in { get; set; }

        /// <summary>
        ///  機能検査ステーションへの搬入時刻。
        ///  外観検査工程の終了時刻と同じ。
        /// </summary>
        public DateTime? Functional_in { get; set; }

        /// <summary>
        ///  組み立てステーションへの搬入時刻。
        ///  機能検査工程の終了時刻と同じ。
        /// </summary>
        public DateTime? Assembly_in { get; set; }

        /// <summary>
        ///  組み立て工程、全ての工程の終了時刻
        /// </summary>
        public DateTime? Assembly { get; set; }


        public Times(int id, DateTime?[] times)
        {
            if (times.Length != COLUM_NUMBER || times[0] == null)
            {
                return;
            }

            this.supply = (DateTime)times[0];
            this.Visal_in = times[1];
            this.Functional_in = times[2];
            this.Assembly_in = times[3];
            this.Assembly = times[4];
        }

        /// <summary>
        ///  各情報から、APIで返すデータを作成する
        /// </summary>
        /// <param name="id"> ワークの(検査)ID </param>
        /// <param name="startTime"> 開始時刻。ワークを代表する時刻で、NULLは許容されない </param>
        /// <param name="times"> 各タイムスタンプを表す配列。 </param>
        public Times(int workId, DateTime startTime, DateTime?[] times)
        {
            if (times.Length != COLUM_NUMBER - 1)
            {
                return;
            }
            this.workId = workId;
            this.supply = startTime;
            this.Functional_in = times[0];
            this.Visal_in = times[1];
            this.Assembly_in = times[2];
            this.Assembly = times[3];
        }



        /// <summary>
        ///  タイムスタンプのデータを、工程が行われる順番に
        ///  配列に入れて返す。
        /// </summary>
        /// <returns> 各工程のタイムスタンプの配列を返す。 </returns>
        public DateTime?[] getTimeArray()
        {
            var timearray = new DateTime?[COLUM_NUMBER];

            timearray[0] = this.supply;
            timearray[1] = this.Functional_in;
            timearray[2] = this.Visal_in;
            timearray[3] = this.Assembly_in;
            timearray[4] = this.Assembly;
            return timearray;
        }
    }
}