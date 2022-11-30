using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Primitives;
using WebAPISample.Models;
using System.Text;
using System.Drawing.Text;

namespace WebAPISample.Controllers
{
    /// <summary>
    /// 結果を表すJSONを作成するクラス。
    /// </summary>
    [Route("api/result")]
    [ApiController]
    public class ResultControll : ControllerBase
    {
        //TODO 返すデータに、検査項目を追加する
        [HttpGet]
        public List<CheckResult> Get(string? resultsort, [FromQuery] TimeParams time)
        {
            
            bool isWhere = false;
            var targetIndexes = new List<int>();   //対象になるワークのID
            StringValues val = new StringValues("*");
            this.Response.Headers.Add("Access-Control-Allow-Origin", val);
            StringBuilder sql = new StringBuilder
                ("SELECT ID FROM View_RDC");

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

            if(time.isParams)
            {
                if (isWhere)
                    sql.Append(" AND ");
                else
                    sql.Append(" WHERE ");

                sql.Append("Carry_in ");
                sql.Append(time.CreateSQL());
            }

            var rList = new List<CheckResult>();
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

            foreach (var e in targetIndexes)
            {
                var errList = new List<string>();
                var errCommand = new SqlCommand("SELECT result_Code,temp,hum,illum,Carry_in from View_RDC where ID = " + e, Parameters.sqlConnection);
                using (var errReader = errCommand.ExecuteReader())
                {
                    float temp = 0;
                    float hun = 0;
                    float illum = 0;
                    DateTime startTime = DateTime.MinValue;
                    if (errReader.Read())
                    {
                        var checkCode = (string)errReader[0];
                        temp = (float)errReader.GetDouble(1);
                        hun = (float)errReader.GetDouble(2);
                        illum = (float)errReader.GetDouble(3);
                        startTime = errReader.GetDateTime(4);
                        if (checkCode.Equals("OK   "))
                        {
                            rList.Add(new CheckResult(e, temp, hun, illum, startTime, true));
                            Console.WriteLine("ID　:　{0}は合格!!!", e);
                            continue;
                        }
                        errList.Add((string)errReader[0]);
                    }
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
        /*
        //TODO 返すデータに、検査項目を追加する
        [HttpPost]
        public List<CheckResult> Post()
        {
            StringValues val = new StringValues("*");

            var targetIndexes = new List<int>();   //対象になるワークのID

            this.Response.Headers.Add("Access-Control-Allow-Origin", val);
            StringBuilder sql = new StringBuilder
                ("SELECT ID FROM Test_Data");
            var rList = new List<CheckResult>();
            using (var command = new SqlCommand(sql.ToString(), Parameters.sqlConnection))
            using (var reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    int id = (int)reader[0];
                    targetIndexes.Add(id);
                }
            }

            foreach (var e in targetIndexes)
            {
                var errList = new List<string>();
                var errCommand = new SqlCommand("SELECT result_Code from Test_Result where ID = " + e, Parameters.sqlConnection);
                using (var errReader = errCommand.ExecuteReader())
                {
                    if (errReader.Read())
                    {
                        var checkCode = (string)errReader[0];
                        if (checkCode.Equals("OK   "))
                        {
                            rList.Add(new CheckResult(e, 11f, 12f, 12f, true));
                            Console.WriteLine("ID　:　{0}は合格!!!", e);
                            continue;
                        }
                        errList.Add((string)errReader[0]);
                    }
                    while (errReader.Read())
                    {
                        errList.Add((string)errReader[0]);
                        Console.WriteLine("ID :{0}  , st :{1}", e, errReader[0]);
                    }
                    rList.Add(new CheckResult(e, 11f, 12f, 12f, errList));
                }
            }
            return rList;
        }*/
    }
}