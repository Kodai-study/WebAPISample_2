using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Primitives;
using System.Text;
using WebAPISample.Models;
using WebAPISample.Modules;

namespace WebAPISample.Controllers
{
    [Route("api/statistics")]
    [ApiController]
    public class DailyResultControll : ControllerBase
    {

        [HttpGet]
        public List<DailyResults> Get([FromQuery] TimeParams timeSearch, [FromQuery] String dateTimeKind)
        {
            StringValues val = new StringValues("*");
            this.Response.Headers.Add("Access-Control-Allow-Origin", val);

            String? conditionalSql = null;

            dateTimeKind = dateTimeKind.ToUpper();

            if (timeSearch.IsSetParams)
            {
                conditionalSql = " WHERE supply " + timeSearch.CreateSQL();
            }

            switch (dateTimeKind)
            {
                case "DAY": return getStatistics_Daily(conditionalSql);
                case "WEEK": return getStatistics_Weekly(conditionalSql);
                case "MONTH": return getStatistics_Monthly(conditionalSql);
                default: goto case "DAY";
            }

        }

        private List<DailyResults> getStatistics_Daily(String? conditionalSql)
        {
            List<DailyResults> statisticsDataList = new List<DailyResults>();
            StringBuilder get = new StringBuilder("select * from dbo.SCAN()");
            if (conditionalSql != null)
            {
                get.Append(conditionalSql);
            }
            using (var command = new SqlCommand(get.ToString(), Parameters.sqlConnection))
            using (var reader = command.ExecuteReader())
            {
                /* 時刻リストから、返すデータを作成 */
                List<Times> times = new List<Times>();
                while (reader.Read())
                {
                    DailyResults columData = createResultsModel(reader, RangeKind.day);
                    statisticsDataList.Add(columData);
                }
            }
            return statisticsDataList;
        }


        private List<DailyResults> getStatistics_Weekly(String? conditionalSql)
        {
            List<DailyResults> statisticsDataList = new List<DailyResults>();
            StringBuilder get = new StringBuilder("select * from dbo.SCAN()");
            if (conditionalSql != null)
            {
                get.Append(conditionalSql);
            }
            using (var command = new SqlCommand(get.ToString(), Parameters.sqlConnection))
            using (var reader = command.ExecuteReader())
            {
                /* 時刻リストから、返すデータを作成 */
                List<Times> times = new List<Times>();
                while (reader.Read())
                {
                    DailyResults columData = createResultsModel(reader, RangeKind.week);
                    statisticsDataList.Add(columData);
                }
            }
            return statisticsDataList;
        }

        private List<DailyResults> getStatistics_Monthly(String? conditionalSql)
        {
            List<DailyResults> statisticsDataList = new List<DailyResults>();
            StringBuilder get = new StringBuilder("select * from dbo.SCAN()");
            if (conditionalSql != null)
            {
                get.Append(conditionalSql);
            }
            using (var command = new SqlCommand(get.ToString(), Parameters.sqlConnection))
            using (var reader = command.ExecuteReader())
            {
                /* 時刻リストから、返すデータを作成 */
                List<Times> times = new List<Times>();
                while (reader.Read())
                {
                    DailyResults columData = createResultsModel(reader, RangeKind.month);
                    statisticsDataList.Add(columData);
                }
            }
            return statisticsDataList;
        }



        private DailyResults createResultsModel(SqlDataReader reader, RangeKind dateRange)
        {
            DateTime firstDateOfRange = reader.GetDateTime(0).Date;


            DateTime lastDataOfRange = dateRange switch
            {
                RangeKind.day => firstDateOfRange,
                RangeKind.week => firstDateOfRange.AddDays(6),
                RangeKind.month => firstDateOfRange.AddMonths(1).AddDays(-1),
                _ => firstDateOfRange
            };

            return new DailyResults(

                            firstDateOfRange,
                            lastDataOfRange,
                            reader.GetInt32(1),
                            reader.GetInt32(2),
                            reader.GetInt32(3),
                            reader.GetInt32(4),
                            reader.GetInt32(5),
                            reader.GetInt32(6),
                            reader.GetInt32(7),
                            reader.GetInt32(8),
                            reader.GetInt32(9),
                            reader.GetInt32(10),
                            reader.GetInt32(11),
                            reader.GetInt32(13),
                            reader.GetInt32(14)
                );
        }
    }



    public enum RangeKind
    {
        day,
        week,
        month
    }

}