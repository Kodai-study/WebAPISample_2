using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.DotNet.Scaffolding.Shared.CodeModifier.CodeChange;
using Microsoft.Extensions.Primitives;
using System.Text;
using WebAPISample.JSONModels;

namespace WebAPISample.Controllers
{

    /// <summary>
    ///  サイクルタイムの一覧を表す。
    ///  各動作が行われた時刻を返す
    /// </summary>
    [Route("api/times")]
    [ApiController]
    public class TimeControll : ControllerBase
    {
        /// <summary>
        ///  各動作タイミングのタイムスタンプを返す。
        ///  GETメソッド URL:/api/times
        /// </summary>
        /// <param name="time"> 
        ///  検査開始時刻(start)の時間で期間指定ができる 
        /// </param>
        /// <see cref="Times"/>
        /// <returns>
        ///  検索された時間データ一覧
        /// </returns>


        [HttpGet]

        public List<TimeInterval> getTimeIntervals([FromQuery] TimeParams timeParams)
        {
            StringValues val = new StringValues("*");
            this.Response.Headers.Add("Access-Control-Allow-Origin", val);
            StringBuilder sql = new StringBuilder("SELECT * FROM SensorTimeT");

            if (timeParams)
            {
                sql.Append(" WHERE supply ");
                sql.Append(timeParams.CreateSQL());
            }
            using (var command = new SqlCommand(sql.ToString(), Parameters.sqlConnection))
            using (var reader = command.ExecuteReader())
            {
                /* 時刻リストから、返すデータを作成 */
                List<TimeInterval> times = new List<TimeInterval>();
                while (reader.Read())
                {
                    var start = (DateTime)reader[1];
                    var spanTimes = new DateTime?[Times.COLUM_NUMBER - 1];

                    for (int i = 0; i < spanTimes.Length; i++)
                    {
                        if (reader[i + 2].Equals(DBNull.Value))
                            spanTimes[i] = null;
                        else
                            spanTimes[i] = reader.GetDateTime(i + 2);
                    }
                    Times timeStump = new Times((int)reader[0], start, spanTimes);
                    times.Add(new TimeInterval(timeStump));
                }
                return times;

            }
        }
    }
}