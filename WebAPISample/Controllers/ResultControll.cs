using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Primitives;
using System.Text;
using WebAPISample.JSONModels;
using WebAPISample.Query;

namespace WebAPISample.Controllers
{
    /// <summary>
    /// 結果を表すJSONを作成するクラス。
    /// </summary>
    [Route("api/result")]
    [ApiController]
    public class ResultControll : ControllerBase
    {

        /// <summary>
        ///  ワークごとの検査結果、検査時の情報などを返すAPI
        /// </summary>
        /// <param name="resultSearchParams"> 
        ///  クエリパラメータ。
        ///  ワーク自体の検査結果や、検査項目ごとに不合格になったワークを選んで表示することができる。
        /// </param>
        ///   クエリパラメータ。
        ///   期間を指定して、検査開始時刻が範囲内のワークのみを選んで表示することができる
        /// <param name="time"></param>
        /// <returns></returns>

        [HttpGet]
        public List<CheckResult> Get([FromQuery] ResultSearchParams resultSearchParams, [FromQuery] TimeRangeParams time)
        {
            bool isWhere = false;
            var targetIndexes = new List<int>();   //対象になるワークのID
            StringValues val = new StringValues("*");
            this.Response.Headers.Add("Access-Control-Allow-Origin", val);
            StringBuilder sql = new StringBuilder
                ("SELECT ID FROM ALL_resultView");


            /* 検査結果の合格、不合格で絞るとき */
            if (resultSearchParams.IsSetParams)
            {
                isWhere = true;
                sql.Append(" WHERE ");
                sql.Append(resultSearchParams.CreateSQL());
            }

            /* 絞り込みに範囲指定があるとき */
            if (time.IsSetParams)
            {
                if (isWhere)
                    sql.Append(" AND ");
                else
                    sql.Append(" WHERE ");

                sql.Append("supply ");
                sql.Append(time.CreateSQL());
            }

            var rList = new List<CheckResult>();

            /* IDを取ってくる */
            using (var command = new SqlCommand(sql.ToString(), Parameters.sqlConnection))
            using (var reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    int id = (int)reader[0];
                    if (targetIndexes.IndexOf(id) < 0)
                        targetIndexes.Add(id);
                }
            }

            /* エラー項目から、検査結果のリストを返す */
            foreach (var e in targetIndexes)
            {
                var errList = new List<string>();
                var errCommand = new SqlCommand
                    ("SELECT ID,Supply,Assembly_end,Volt,Freq,result_Code FROM ALL_resultView where ID = " + e
                    , Parameters.sqlConnection);

                using (var errReader = errCommand.ExecuteReader())
                {
                    DateTime startTime = DateTime.MinValue;
                    DateTime? endTime = null;
                    if (errReader.Read())
                    {
                        /* 1行目のデータから、ワークの検査情報を取ってくる */
                        var checkCode = errReader.GetString(5);


                        if (!errReader[1].Equals(DBNull.Value))
                            startTime = errReader.GetDateTime(1);
                        if (!errReader[2].Equals(DBNull.Value))
                            endTime = errReader.GetDateTime(2);

                        /* 検査が全部オッケーだった時、全て検査 */
                        if (checkCode.Equals("OK  "))
                        {
                            rList.Add(new CheckResult(e, startTime, endTime, true));
                            continue;
                        }
                        errList.Add(errReader.GetString(5));
                    }
                    /* 検査にエラーがあったら、そのリストからワークの検査結果を作成 */
                    while (errReader.Read())
                    {
                        errList.Add(errReader.GetString(5));
                    }
                    rList.Add(new CheckResult(e, startTime, endTime, errList));
                }
            }
            return rList;
        }
    }
}