using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Primitives;
using System.Text;
using WebAPISample.Models;

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
        ///  GETメソッドで、検査結果の一覧をJSONで返す
        /// </summary>
        /// <param name="resultsort"> 
        ///  検査全体の結果が合格("OK")
        ///  不合格("NG")だけを選択することができる
        /// </param>
        /// <param name="time"> 検査開始時刻で期間の指定ができる </param>
        /// <returns> CheckResult 検査結果のリスト </returns>
        [HttpGet]
        public List<CheckResult> Get(string? resultsort, [FromQuery] TimeParams time)
        {
            bool isWhere = false;
            var targetIndexes = new List<int>();   //対象になるワークのID
            StringValues val = new StringValues("*");
            this.Response.Headers.Add("Access-Control-Allow-Origin", val);
            StringBuilder sql = new StringBuilder
                ("SELECT ID FROM View_RDC");

            /* 検査結果の合格、不合格で絞るとき */
            if (resultsort != null)
            {
                isWhere = true;
                if (resultsort.Equals("OK"))
                {
                    sql.Append(" WHERE result_Code = 'OK   '");
                }
                else if (resultsort.Equals("NG"))
                {
                    sql.Append(" WHERE not result_Code = 'OK   '");
                }
            }

            /* 絞り込みに範囲指定があるとき */
            if (time.IsSetParams)
            {
                if (isWhere)
                    sql.Append(" AND ");
                else
                    sql.Append(" WHERE ");

                sql.Append("Carry_in ");
                sql.Append(time.CreateSQL());
            }

            var rList = new List<CheckResult>();
            //HACK 取ってくるときの設計を変える。出力を全部見て、IDが変わったら書き換え

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
                    ("SELECT result_Code,temp,hum,illum,Carry_in FROM View_RDC where ID = " +
                    e, Parameters.sqlConnection);

                using (var errReader = errCommand.ExecuteReader())
                {
                    float? temp = null;
                    float? hun = null;
                    float? illum = null;
                    DateTime startTime = DateTime.MinValue;
                    if (errReader.Read())
                    {
                        /* 1行目のデータから、ワークの検査情報を取ってくる */
                        var checkCode = (string)errReader[0];

                        //温度、湿度、照度にはNULLが入っている可能性がある
                        if (!errReader[1].Equals(DBNull.Value))
                            temp = (float)errReader.GetDouble(1);

                        if (!errReader[2].Equals(DBNull.Value))
                            hun = (float)errReader.GetDouble(2);

                        if (!errReader[3].Equals(DBNull.Value))
                            illum = (float)errReader.GetDouble(3);

                        if (!errReader[4].Equals(DBNull.Value))
                            startTime = errReader.GetDateTime(4);

                        /* 検査が全部オッケーだった時の処理 */
                        if (checkCode.Equals("OK   "))
                        {
                            rList.Add(new CheckResult(e, temp, hun, illum, startTime, true));
                            Console.WriteLine("ID　:　{0}は合格!!!", e);
                            continue;
                        }
                        errList.Add((string)errReader[0]);
                    }
                    /* 検査にエラーがあったら、そのリストからワークの検査結果を作成 */
                    while (errReader.Read())
                    {
                        errList.Add((string)errReader[0]);
                        Console.WriteLine("ID :{0}  , st :{1}", e, errReader[0]);
                    }
                    rList.Add(new CheckResult(e, temp, hun, illum, startTime, errList));
                }
            }
            return rList;
        }
    }
}