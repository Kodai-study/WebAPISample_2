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
    ///  サイクルタイムの一覧を表す。
    ///  各動作が行われた時刻を返す
    /// </summary>
    [Route("api/times/timestump")]
    [ApiController]
    public class TimeStumpControll : ControllerBase
    {
        /// <summary>
        ///  各動作タイミングのタイムスタンプを返す。
        ///  GETメソッド URL:/api/times
        /// </summary>
        /// <param name="timeParams"> 
        ///  検査開始時刻(start)の時間で期間指定ができる 
        /// </param>
        /// <see cref="Times"/>
        /// <returns>
        ///  検索された時間データ一覧
        /// </returns>
        /// 
        [HttpGet]
        public List<Times> getTimeStamps([FromQuery] TimeRangeParams timeParams, [FromQuery] SortParams sortParams)
        {
            StringValues val = new("*");
            this.Response.Headers.Add("Access-Control-Allow-Origin", val);
            StringBuilder sql = new("SELECT * FROM SensorTimeT");

            if (timeParams)
            {
                sql.Append(" WHERE supply ");
                sql.Append(timeParams.CreateSQL());
            }

            sql.Append(sortParams.CreateSQL("Supply", "DESC"));
            using SqlCommand command = new(sql.ToString(), InspectionParameters.sqlConnection);
            using SqlDataReader reader = command.ExecuteReader();
            /* 時刻リストから、返すデータを作成 */
            List<Times> times = new();
            while (reader.Read())
            {
                // 検査開始時刻は、ワークを代表する時刻データで、絶対に必要
                DateTime startTime = reader.GetDateTime(1);

                // 検査開始時刻を除いた時刻データを代入
                DateTime?[] timeStumpsArray = new DateTime?[Times.COLUM_NUMBER - 1];

                for (int i = 0; i < timeStumpsArray.Length; i++)
                {
                    if (reader[i + 2].Equals(DBNull.Value))
                        timeStumpsArray[i] = null;
                    else
                        timeStumpsArray[i] = reader.GetDateTime(i + 2);
                }
                times.Add(new Times(reader.GetInt32(0), startTime, timeStumpsArray));
            }
            return times;
        }
    }
}