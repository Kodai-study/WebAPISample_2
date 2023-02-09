﻿using Microsoft.AspNetCore.Mvc;
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
        ///  GETメソッドで、検査結果の一覧をJSONで返す
        /// </summary>
        /// <param name="resultsort"> 
        ///  検査全体の結果が合格("OK")
        ///  不合格("NG")だけを選択することができる
        /// </param>
        /// <param name="time"> 検査開始時刻で期間の指定ができる </param>
        /// <returns> CheckResult 検査結果のリスト </returns>
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
                sql.Append(resultSearchParams.getTermsSql());
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

                        /* 検査が全部オッケーだった時の処理 */
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