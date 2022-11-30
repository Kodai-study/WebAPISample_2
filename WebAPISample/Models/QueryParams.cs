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
        [FromQuery(Name ="endTime")]
        public DateTime endTime { get; set; }

        /// <summary>
        /// クエリが存在していたらtrue
        /// 存在していなかったらfalse
        /// </summary>
        public bool isParams
        {
            get
            {
                return startTime != DateTime.MinValue || endTime != DateTime.MinValue;
            }
        }

        public explicit operator bool(TimeParams tm)
        {
            return false;
        }



        public string CreateSQL()
        {

            var o = new TimeParams();
            if ((bool)o)
            {

            }

            var sql = new StringBuilder("");
            if (startTime != DateTime.MinValue && endTime != DateTime.MinValue)
            {
                sql.Append(" between '");
                sql.Append(startTime);
                sql.Append("' AND '");
                sql.Append(endTime);
                sql.Append("'");
            }
            else if (startTime != DateTime.MinValue)
            {
                sql.Append(" > '");
                sql.Append(startTime);
                sql.Append("'");
            }
            else if (endTime != DateTime.MinValue)
            {
                sql.Append(" < '");
                sql.Append(endTime);
                sql.Append("'");
            }
            return sql.ToString();
        }
    }
}