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
        public List<DailyResults> Get([FromQuery] TimeParams timeSearch, [FromQuery] RangeKind dateTimeKind)
        {
            StringValues val = new StringValues("*");
            this.Response.Headers.Add("Access-Control-Allow-Origin", val);

            String? conditionalSql = null;

            if (timeSearch.IsSetParams)
            {
                conditionalSql = " WHERE supply " + timeSearch.CreateSQL();
            }

            switch (dateTimeKind)
            {
                case RangeKind.day: return getStatistics_Daily(conditionalSql);
                case RangeKind.week: return getStatistics_Weekly(conditionalSql);
                case RangeKind.month: return getStatistics_Monthly(conditionalSql);
                default: goto case RangeKind.day; 
            }

        }

        private List<DailyResults> getStatistics_Daily(String? conditionalSql)
        {
            List<DailyResults> statisticsDataList = new List<DailyResults>();
            StringBuilder get = new StringBuilder("SELECT YMD,scan,OK,ave_temp,ave_hun,ave_illum,ave_cyctime,max_cyctime,min_cyctime FROM View_DOK");
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
            StringBuilder get = new StringBuilder("SELECT YMD,scan,OK,ave_temp,ave_hun,ave_illum,ave_cyctime,max_cyctime,min_cyctime FROM View_DOK");
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
            StringBuilder get = new StringBuilder("SELECT YMD,scan,OK,ave_temp,ave_hun,ave_illum,ave_cyctime,max_cyctime,min_cyctime FROM View_DOK");
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
            DateOnly firstDateOfRange = DateOnly.FromDateTime(reader.GetDateTime(0));

            DateOnly lastDataOfRange = dateRange switch
            {
                RangeKind.day => firstDateOfRange.AddDays(1),
                RangeKind.week => firstDateOfRange.AddDays(6),
                RangeKind.month => firstDateOfRange.AddMonths(1).AddDays(-1),
                _ => firstDateOfRange
            };

            return new DailyResults(

                            firstDateOfRange,
                            firstDateOfRange.AddMonths(1).AddDays(-1),
                            reader.GetInt32(1),
                            reader.GetInt32(1),
                            reader.GetInt32(1),
                            reader.GetInt32(1),
                            reader.GetInt32(1),
                            reader.GetInt32(1),
                            reader.GetInt32(1),
                            reader.GetInt32(1),
                            reader.GetInt32(1),
                            reader.GetInt32(1),
                            reader.GetInt32(1),
                            reader.GetInt32(1),
                            reader.GetInt32(1)
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