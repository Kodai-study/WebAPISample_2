using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Primitives;
using System.IO.MemoryMappedFiles;
using System.Text;
using WebAPISample.JSONModels;

namespace WebAPISample.Controllers
{
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


        [HttpGet]
        public List<Utilization> Get([FromQuery] TimeParams timeParams)
        {
            List<Utilization> utilizationList = new List<Utilization>();



            StringValues val = new StringValues("*");
            this.Response.Headers.Add("Access-Control-Allow-Origin", val);
            StringBuilder sql = new StringBuilder("SELECT state_Code,time FROM StateTimeT");

            if (timeParams)
            {
                sql.Append(" WHERE supply ");
                sql.Append(timeParams.CreateSQL());
            }
            using (var command = new SqlCommand(sql.ToString(), Parameters.sqlConnection))
            using (var reader = command.ExecuteReader())
            {
                List<Tuple<String, DateTime>> stateChangeTimes =
                   new List<Tuple<string, DateTime>>();


                DateTime currentDate;
                if (reader.Read())
                {
                    currentDate = reader.GetDateTime(1).Date;
                    stateChangeTimes.Add(
                        new Tuple<string, DateTime>(reader.GetString(0), reader.GetDateTime(1)));
                }
                else
                {
                    Console.WriteLine("ロボットの状態変化テーブルが読み取れなかった");
                    return utilizationList;
                }
                while (reader.Read())
                {
                    if (reader.GetDateTime(1).Date > currentDate)
                    {
                        utilizationList.Add(new Utilization(currentDate, stateChangeTimes));
                        stateChangeTimes = new List<Tuple<string, DateTime>>();
                        currentDate = currentDate.AddDays(1);
                    }
                    stateChangeTimes.Add(
                        new Tuple<string, DateTime>(reader.GetString(0), reader.GetDateTime(1)));
                }
                utilizationList.Add(new Utilization(currentDate, stateChangeTimes));
                return utilizationList;
            }
        }
    }
}