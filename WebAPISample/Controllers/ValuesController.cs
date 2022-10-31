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
        public LogSample Get()
        {
            StringValues val = new StringValues("*");

            this.Response.Headers.Add("Access-Control-Allow-Origin", val);

            
                using (var command = new SqlCommand("select day,time,user_info.name_ID,name from access_log join user_info on access_log.name_ID = user_info.name_ID", hoge.sqlConnection))
                using (var reader = command.ExecuteReader())
                {
                    reader.Read();
                    DateTime day = (DateTime)reader[0];
                    TimeSpan time = (TimeSpan)reader[1];
                    int userId = (int)reader[2];
                    string userName = (string)reader[3];
                    return new LogSample(day, time, userId, userName);
                }

            

        }
    }
}
