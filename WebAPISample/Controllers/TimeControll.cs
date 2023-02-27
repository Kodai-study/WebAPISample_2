using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.DotNet.Scaffolding.Shared.CodeModifier.CodeChange;
using Microsoft.Extensions.Primitives;
using System.Text;
using WebAPISample.Data;
using WebAPISample.JSONModels;
using WebAPISample.Query;

namespace WebAPISample.Controllers
{

    /// <summary>
    ///  工程や、ステーション等の単位で、ワークの検査等に
    ///  かかった時間を表示するAPI
    /// </summary>
    [Route("api/times")]
    [ApiController]
    public class TimeControll : ControllerBase
    {
        /// <summary>
        ///  APIのメインメソッド。
        ///  GETメソッド URL:/api/times
        /// </summary>
        /// <param name="timeParams"> 
        ///  ワーク搬入の開始時刻の範囲を指定して表示することができる。
        /// </param>
        /// <see cref="Times"/>
        /// <returns>
        ///  検索された時間データ一覧
        /// </returns>
        [HttpGet]
        public List<TimeInterval> getTimeIntervals([FromQuery] TimeRangeParams timeParams, [FromQuery]SortParams sortParams)
        {
            StringValues val = new("*");
            this.Response.Headers.Add("Access-Control-Allow-Origin", val);
            StringBuilder sql = new("SELECT * FROM SensorTimeT ");

            /* 時間の範囲指定による絞り込み */
            if (timeParams)
            {
                sql.Append(" WHERE supply ");
                sql.Append(timeParams.CreateSQL());
            }

            sql.Append(sortParams.CreateSQL("Supply", "DESC"));
            using var command = new SqlCommand(sql.ToString(), InspectionParameters.sqlConnection);
            using var reader = command.ExecuteReader();
            /* 時刻リストから、返すデータを作成 */
            List<TimeInterval> times = new();
            while (reader.Read())
            {
                var start = (DateTime)reader[1];        //検査開始時刻
                                                        //開始時刻以外のタイムスタンプ
                var spanTimes = new DateTime?[Times.COLUM_NUMBER - 1];

                for (int i = 0; i < spanTimes.Length; i++)
                {
                    if (reader[i + 2].Equals(DBNull.Value))
                        spanTimes[i] = null;
                    else
                        spanTimes[i] = reader.GetDateTime(i + 2);
                }
                //タイムスタンプのデータから、各工程にかかった時間のデータを作成
                Times timeStump = new((int)reader[0], start, spanTimes);
                times.Add(new TimeInterval(timeStump));
            }
            return times;
        }
    }
}