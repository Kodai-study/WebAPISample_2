using Microsoft.AspNetCore.Mvc;
using System.Text;

namespace WebAPISample.Models
{
    /// <summary>
    /// 時間に関するクエリを管理するクラス
    /// </summary>
    public class TimeParams
    {
        /// <summary>
        /// クエリネーム:startTime
        /// 検査開始日付(時刻)の範囲絞り込みで、
        /// この時間以降(ぴったりを含む)の選択になる。
        /// </summary>
        [FromQuery(Name = "startTime")]
        public DateTime startTime { get; set; }
        /// <summary>
        /// クエリネーム:endTime
        /// 検査開始日付(時刻)の範囲絞り込みで、
        /// この時間以前になる。
        /// </summary>
        [FromQuery(Name = "endTime")]
        public DateTime endTime { get; set; }

        /// <summary>
        /// クエリが存在していたらtrue
        /// 存在していなかったらfalse
        /// </summary>
        public bool IsSetParams
        {
            get
            {
                return startTime != DateTime.MinValue || endTime != DateTime.MinValue;
            }
        }

        /// <summary>
        /// このクラスをbool型に変更すると、パラメータが存在するかどうかを
        /// 返してくれる
        /// </summary>
        /// <param name="tm"></param>
        public static implicit operator bool(TimeParams tm)
        {
            return tm.startTime != DateTime.MinValue || tm.endTime != DateTime.MinValue;
        }
        
        /// <summary>
        /// 期間の初めの時間を指定するパラメータがあるかどうか
        /// </summary>
        public bool IsSetStartTime
        {
            get
            {
                return startTime != DateTime.MinValue;
            }
        }

        /// <summary>
        /// 期間の終わりの時間を指定するパラメータがあるかどうか
        /// </summary>
        public bool IsSetEndTime
        {
            get
            {
                return endTime != DateTime.MinValue;
            }
        }

        /// <summary>
        /// 受け取っていた、クエリパラメータの日付の値から、
        /// 範囲を指定するSQL文を作り出す。
        /// </summary>
        /// <returns> 
        ///  DataTimeの範囲を指定するSQL文
        ///  startTime以上、endTime以下 
        /// </returns>
        public string CreateSQL()
        {
            var sql = new StringBuilder("");

            /* 期間の最初と最後が指定されていたとき */
            if (IsSetStartTime && IsSetEndTime)
            {
                if(endTime.TimeOfDay == TimeSpan.Zero)
                {
                    endTime = endTime.AddDays(1);
                }
                Console.WriteLine(endTime);
                sql.Append(" BETWEEN '");
                sql.Append(startTime);
                sql.Append("' AND '");
                sql.Append(endTime);
                sql.Append("'");
            }
            else if (IsSetStartTime)
            {
                sql.Append(" >= '");
                sql.Append(startTime);
                sql.Append("'");
            }
            else if (IsSetEndTime)
            {
                if (endTime.TimeOfDay == TimeSpan.Zero)
                {
                    endTime.AddDays(1);
                }
                sql.Append(" <= '");
                sql.Append(endTime);
                sql.Append("'");
            }
            return sql.ToString();
        }
    }
}