using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Primitives;
using WebAPISample.Models;
using System.Text;

namespace WebAPISample.Controllers
{
    [Route("api/result")]
    [ApiController]
    public class ResultControll : ControllerBase
    {
        [HttpGet]
        public List<CheckResult> Get()
        {
            StringValues val = new StringValues("*");
            var errIndexes = new List<int>();

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
                    errIndexes.Add(id);
                }
            }

            foreach (var e in errIndexes)
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
                            Console.WriteLine("ID　:　{0}は合格!!!",e);
                            continue;
                        }
                        errList.Add((string)errReader[0]);
                    }
                    while (errReader.Read())
                    {
                        errList.Add((string)errReader[0]);
                        Console.WriteLine("ID :{0}  , st :{1}", e, errReader[0]);
                    }
                    rList.Add(new CheckResult(e,11f,12f,12f,errList));
                }
            }
            return rList;
        }
    }
}