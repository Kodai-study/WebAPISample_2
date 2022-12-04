using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Primitives;
using WebAPISample.Models;
using System;
using System.Text;

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
        public List<Times> Get([FromQuery] TimeParams time)
        {
            StringValues val = new StringValues("*");
            this.Response.Headers.Add("Access-Control-Allow-Origin", val);
            StringBuilder sql = new StringBuilder("SELECT * FROM Test_CycleTime WHERE");
            sql.Append(time.CreateSQL());

            using (var command = new SqlCommand(sql.ToString(), Parameters.sqlConnection))
            using (var reader = command.ExecuteReader())
            {
                /* 時刻リストから、返すデータを作成 */
                List<Times> times = new List<Times>();
                while (reader.Read())
                {
                    var start = (DateTime)reader[1];
                    var spanTimes = new TimeSpan[9];

                    for (int i = 0; i < spanTimes.Length; i++)
                    {
                        spanTimes[i] = (TimeSpan)reader[i + 2];
                    }
                    times.Add(new Times((int)reader[0], start, spanTimes));
                    //times.Add(new Times(spanTimes));
                }
                return times;
            }
        }

        //TODO かかった時間を返すやつも作成
        [HttpGet("{id}")]
        public List<TimeSpan> get(string id)
        {
            return new List<TimeSpan>();
        }
    }
}