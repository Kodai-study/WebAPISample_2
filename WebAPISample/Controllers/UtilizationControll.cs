using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Primitives;
using System.IO.MemoryMappedFiles;
using System.Text;
using WebAPISample.Data;
using WebAPISample.JSONModels;
using WebAPISample.Modules;
using WebAPISample.Query;

namespace WebAPISample.Controllers
{
    /// <summary>
    ///  ステーション全体の状態を変更した時間と変化した状態の
    ///  情報を返す。
    /// </summary>
    [Route("api/stationUtilization")]
    [ApiController]
    public class UtilizationControll : ControllerBase
    {
        public const String STATECODE_START = "Start";
        public const String STATECODE_END = "End";
        public const String STATECODE_PAUSE = "Pause";
        public const String STATECODE_RESTART = "Restart";
        public const String STATECODE_EMERGENCY = "Emergency";
        public const String STATECODE_STANDBY = "Standby";

        /// <summary>
        ///  APIのメインメソッド。
        ///  1日毎の、ロボットの稼働時間に関する情報を取得する
        /// </summary>
        /// <param name="timeParams"></param>
        /// <returns></returns>
        [HttpGet]
        public List<Utilization> Get([FromQuery] TimeRangeParams timeParams, [FromQuery]SortParams sortParams)
        {
            List<Utilization> utilizationList = new();
            StringValues val = new("*");
            this.Response.Headers.Add("Access-Control-Allow-Origin", val);
            StringBuilder sql = new("SELECT state_Code,time FROM StateTimeT");


            if (timeParams)
            {
                sql.Append(" WHERE time ");
                sql.Append(timeParams.CreateSQL());
            }


            using SqlCommand command = new(sql.ToString(), InspectionParameters.sqlConnection);
            using SqlDataReader reader = command.ExecuteReader();
            /* 状態変化の時刻と、変化後の状態のペアのリスト */
            List<Tuple<String, DateTime>> stateChangeTimes = new();

            DateTime currentDate;   //稼働時間を取得する対象の日付
            if (reader.Read())
            {
                currentDate = reader.GetDateTime(1).Date;
                stateChangeTimes.Add(
                    new Tuple<String, DateTime>(reader.GetString(0), reader.GetDateTime(1)));
            }
            else
            {
                Console.WriteLine("ロボットの状態変化テーブルが読み取れなかった");
                return utilizationList;
            }

            while (reader.Read())
            {
                // 状態変化した日付が更新された段階で、その日の稼働状況の情報を計算して作成
                if (reader.GetDateTime(1).Date > currentDate)
                {
                    utilizationList.Add(new Utilization(currentDate, stateChangeTimes));
                    stateChangeTimes = new List<Tuple<String, DateTime>>();
                    currentDate = currentDate.AddDays(1);
                }
                stateChangeTimes.Add(
                    new Tuple<String, DateTime>(reader.GetString(0), reader.GetDateTime(1)));
            }
            utilizationList.Add(new Utilization(currentDate, stateChangeTimes));


            if (sortParams.IsSetAnyParams)
            {
                if (sortParams.sortColum.ToUpper() == "DATE")
                {
                    if (sortParams.sortingMethod.ToUpper() == "ASC")
                        utilizationList.Sort(new UtilizationComparer_DateAsc());
                    else
                        utilizationList.Sort(new UtilizationComparer_DateDesc());
                }
                else if (sortParams.sortColum.ToUpper().Contains("OPERATION"))
                {
                    if (sortParams.sortingMethod.ToUpper() == "ASC")
                        utilizationList.Sort(new UtilizationComparer_OperationTimeAsc());
                    else
                        utilizationList.Sort(new UtilizationComparer_OperationTimeDesc());
                }
                else if(sortParams.sortingMethod == "ASC") {
                    utilizationList.Sort(new UtilizationComparer_DateAsc());
                }
            }else
            {
                utilizationList.Sort(new UtilizationComparer_DateDesc());
            }
            return utilizationList;
        }
    }
}