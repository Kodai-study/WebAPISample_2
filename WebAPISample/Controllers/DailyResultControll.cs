using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Primitives;
using System.Text;
using WebAPISample.Data;
using WebAPISample.JSONModels;
using WebAPISample.Query;

namespace WebAPISample.Controllers
{
    /// <summary>
    ///  単位時間ごとの検査数、不合格数などを表示させるAPI
    /// </summary>
    [Route("api/statistics")]
    [ApiController]
    public class DailyResultControll : ControllerBase
    {
        /// <summary>
        ///  URLでアクセスした時に呼び出され、データを作成した返す
        ///  メインの関数
        /// </summary>
        /// <param name="timeSearch">
        ///  表示するデータの期間を指定する。
        ///  範囲の始めの時間と範囲の終わりの時間を指定
        /// </param>
        /// <param name="dateTimeKind">
        ///  単位時間を、1日:DAY 1週間:WEEK 1か月:MONTH
        ///  の中から選ぶことができる
        /// </param>
        /// <returns></returns>
        [HttpGet]
        public List<DailyResults> Get([FromQuery] TimeRangeParams timeSearch, [FromQuery] SortParams sortParams, [FromQuery] String? dateTimeKind = "DAY")
        {
            StringValues val = new("*");
            this.Response.Headers.Add("Access-Control-Allow-Origin", val);

            String? conditionalSql = null;

            if(dateTimeKind != null) {
                dateTimeKind = dateTimeKind.ToUpper();
            }
            else
            {
                dateTimeKind = "DEFAULT";
            }
            

            if (timeSearch.IsSetParams)
            {
                conditionalSql = " WHERE Supply " + timeSearch.CreateSQL();
            }

            switch (dateTimeKind)
            {
                case "DAY": return getStatistics_Daily(conditionalSql,sortParams);
                case "WEEK": return getStatistics_Weekly(conditionalSql, sortParams);
                case "MONTH": return getStatistics_Monthly(conditionalSql, sortParams);
                default: goto case "DAY";
            }

        }

        /// <summary>
        ///  1日毎の検査結果データを作成して返す。
        /// </summary>
        /// <param name="conditionalSql">
        ///  絞り込みのSQL文の文字列。
        /// </param>
        /// <returns></returns>
        private List<DailyResults> getStatistics_Daily(String? conditionalSql, SortParams sortParams)
        {
            List<DailyResults> statisticsDataList = new ();
            StringBuilder get = new StringBuilder("select * from dbo.SCAN()");
            if (conditionalSql != null)
            {
                get.Append(conditionalSql);
            }


            get.Append(getSortSQL(sortParams, "YMD"));

            using (SqlCommand command = new(get.ToString(), InspectionParameters.sqlConnection))
            using (SqlDataReader reader = command.ExecuteReader())
            {
                /* 時刻リストから、返すデータを作成 */
                List<Times> times = new();
                while (reader.Read())
                {
                    DailyResults columData = createResultsModel(reader, RangeKind.day);
                    statisticsDataList.Add(columData);
                }
            }
            return statisticsDataList;
        }


        /// <summary>
        ///  1週間毎の検査結果データを作成して返す。
        /// </summary>
        /// <param name="conditionalSql">
        ///  絞り込みのSQL文の文字列。
        /// </param>
        /// <returns></returns>
        private List<DailyResults> getStatistics_Weekly(String? conditionalSql, SortParams sortParams)
        {
            List<DailyResults> statisticsDataList = new List<DailyResults>();
            StringBuilder get = new("select * from dbo.WEEK()");
            if (conditionalSql != null)
            {
                get.Append(conditionalSql);
            }
           
            get.Append(getSortSQL(sortParams,"W"));
            using (var command = new SqlCommand(get.ToString(), InspectionParameters.sqlConnection))
            using (var reader = command.ExecuteReader())
            {
                /* 時刻リストから、返すデータを作成 */
                List<Times> times = new();
                while (reader.Read())
                {
                    DailyResults columData = createResultsModel(reader, RangeKind.week);
                    statisticsDataList.Add(columData);
                }
            }
            return statisticsDataList;
        }


        /// <summary>
        ///  1か月毎の検査結果データを作成して返す。
        /// </summary>
        /// <param name="conditionalSql">
        ///  絞り込みのSQL文の文字列。
        /// </param>
        /// <returns></returns>
        private List<DailyResults> getStatistics_Monthly(String? conditionalSql, SortParams sortParams)
        {
            List<DailyResults> statisticsDataList = new();
            StringBuilder get = new("select * from dbo.month()");
            if (conditionalSql != null)
            {
                get.Append(conditionalSql);
            }

            get.Append(getSortSQL(sortParams, "M"));
            using (var command = new SqlCommand(get.ToString(), InspectionParameters.sqlConnection))
            using (var reader = command.ExecuteReader())
            {
                /* 時刻リストから、返すデータを作成 */
                List<Times> times = new();
                while (reader.Read())
                {
                    DailyResults columData = createResultsModel(reader, RangeKind.month);
                    statisticsDataList.Add(columData);
                }
            }
            return statisticsDataList;
        }

        
        /// <summary>
        ///  検査結果データを作成する関数。単位時間の種類によって、
        ///  期間の始まり、終わりの日付が変わる。
        /// </summary>
        /// <param name="reader">
        ///  データベースのアクセスオブジェクト
        /// </param>
        /// <param name="dateRange">
        ///  単位時間が、1日、1週間、1か月のどれかを選択する
        /// </param>
        /// <returns> 単位時間当たりの検査結果データオブジェクト </returns>
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
        private String getSortSQL(SortParams sortParams, String dateColumName)
        {
            String defaultSortMethod = "DESC";

            if (sortParams.sortColum.ToUpper() == "DATE")
            {
                sortParams.sortColum = dateColumName;
                return sortParams.CreateSQL(dateColumName, defaultSortMethod);
            }
            
            if (sortParams.sortColum.ToUpper() == "PASSRATE")
                sortParams.sortColum = " IIF(SCAN = 0 OR OK = 0,0,(CONVERT(FLOAT, OK)/SCAN)) ";

            else if (sortParams.sortColum.ToUpper() == "DEFECTRATE")
            {
                sortParams.sortColum = " IIF(SCAN = 0 OR OK = 0,0,(CONVERT(FLOAT, OK)/SCAN)) ";
                defaultSortMethod = "ASC";
                if (sortParams.IsSetSotringMethod)
                {
                    if (sortParams.sortingMethod.ToUpper() == "DESC")
                        sortParams.sortingMethod = "ASC";

                    else if (sortParams.sortingMethod.ToUpper() == "ASC")
                        sortParams.sortingMethod = "DESC";
                }
            }
            return sortParams.CreateSQL(dateColumName, defaultSortMethod) +
                String.Format(",{0} DESC", dateColumName);
        }
    }



    public enum RangeKind
    {
        day,
        week,
        month
    }

}