using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Primitives;
using WebAPISample.Models;
using System;

namespace WebAPISample.Controllers
{
    [Route("api/json")]
    [ApiController]
    public class JSONCrtl : ControllerBase
    {
        [HttpGet]
        public List<LogSample> Get()
        {
            StringValues val = new StringValues("*");
            var log_list = new List<LogSample>();
            this.Response.Headers.Add("Access-Control-Allow-Origin", val);


            using (var command = new SqlCommand("select day_time,user_info.name_ID,name from access_log join user_info on access_log.name_ID = user_info.name_ID", hoge.sqlConnection))
            using (var reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    DateTime day = (DateTime)reader[0];
                    int userId = (int)reader[1];
                    string userName = (string)reader[2];
                    log_list.Add(new LogSample(day, userId, userName));
                }
                return log_list;
            }
        }
    }
}
