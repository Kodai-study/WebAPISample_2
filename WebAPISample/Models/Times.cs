using static WebAPISample.Models.Result;

namespace WebAPISample.Models
{
    /// <summary>
    /// サイクルタイムを、工程ごとの時刻で表示させるときの表示データ
    /// </summary>
    public class Times
    {

        public const int COLUM_NUMBER = 5;

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
        /// <param name="startTime"> 開始時刻。これだけ日付を含むDateTime </param>
        /// <param name="times"> タイムスタンプの配列。時刻のみのTimeSpan </param>
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
        /// サイクルID。行われた検査ごとに作られる
        /// </summary>
        public int workId { get; set; }
        /// <summary>
        /// 搬送コンベアに乗せられた(センサに触れた)時間。
        /// ワークを代表する検査時間はこの時間が適用される
        /// </summary>
        public DateTime supply { get; set; }
        /// <summary>
        /// ワークが写真撮影場所で位置決めされた時
        /// </summary>
        public DateTime? Functional_in { get; set; }

        public DateTime? Visal_in { get; set; }


        /// <summary>
        /// 写真撮影の1枚目が行われた時
        /// </summary>
        public DateTime? Assembly_in { get; set; }
        /// <summary>
        /// 写真撮影の最後の1枚が行われた時
        /// </summary>
        public DateTime? Assembly { get; set; }

        /// <summary>
        ///  (使うかわからない)
        ///  タイムスタンプのデータを扱いやすい配列にする
        /// </summary>
        /// <returns> TimeSpanの配列。日付どうしよう </returns>
        public DateTime?[] getTimeArray()
        {

            ///  FIXME hello
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