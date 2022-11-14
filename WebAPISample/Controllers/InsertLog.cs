using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Primitives;
using WebAPISample.Models;

namespace WebAPISample.Controllers
{
    [Route("api/insert")]
    [ApiController]
    public class InsertLog : ControllerBase
    {
        [HttpGet]
        public void get([FromQuery] int num)
        {
            StringValues val = new StringValues("*");
            this.Response.Headers.Add("Access-Control-Allow-Origin", val);

            var sql = string.Format("INSERT INTO access_log (day_time,name_ID) VALUES ('{0}',{1})", DateTime.Now, num);
            using (var command = new SqlCommand(sql, Parameters.sqlConnection))
            {
                command.ExecuteNonQuery();
            }
        }
    }
}