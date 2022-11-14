using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Primitives;
using WebAPISample.Models;
using System;
using System.Text;

namespace WebAPISample.Controllers
{
    [Route("api/times")]
    [ApiController]
    public class TimeControll : ControllerBase
    {
        [HttpGet]
        public List<Times> Get(DateTime startTime,DateTime endTime)
        {
            StringValues val = new StringValues("*");

            this.Response.Headers.Add("Access-Control-Allow-Origin", val);
            StringBuilder sql = new StringBuilder("SELECT * FROM Cycle_content ");
            
            if(startTime != DateTime.MinValue && endTime != DateTime.MinValue)
            {
                sql.Append("WHERE Carry_in between '");
                sql.Append(startTime);
                sql.Append("' AND '");
                sql.Append(endTime);
                sql.Append("'");
            } else if(startTime != DateTime.MinValue)
            {
                sql.Append("WHERE Carry_in > '");
                sql.Append(startTime);
                sql.Append("'");
            } else if(endTime != DateTime.MinValue)
            {
                sql.Append("WHERE Carry_in < '");
                sql.Append(endTime);
                sql.Append("'");
            }

            using (var command = new SqlCommand(sql.ToString(), Parameters.sqlConnection))
            using (var reader = command.ExecuteReader())
            {
                List<Times> times = new List<Times>();
                while (reader.Read())
                {
                    var start = (DateTime)reader[1];

                    var spanTimes = new TimeSpan[9];
                    spanTimes[0] = start.TimeOfDay;
                    
                    for(int i = 0; i < spanTimes.Length; i++)
                    {
                        spanTimes[i] = (TimeSpan)reader[i + 2];
                    }
                    times.Add(new Times((int)reader[0],start,spanTimes));
                    //times.Add(new Times(spanTimes));
                }
                return times;
            }
        }
    }
}
