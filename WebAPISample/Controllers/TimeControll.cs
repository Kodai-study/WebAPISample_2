using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Primitives;
using WebAPISample.Models;
using System;



namespace WebAPISample.Controllers
{
    [Route("api/times")]
    [ApiController]
    public class TimeControll : ControllerBase
    {
        [HttpGet("{id}")]        
        public List<Times> Get(int id,string str)
        {
            Console.WriteLine("id={0}, str = {1}",id,str);
            StringValues val = new StringValues("*");

            this.Response.Headers.Add("Access-Control-Allow-Origin", val);            
            using (var command = new SqlCommand("select * from Time_Table", hoge.sqlConnection))
            using (var reader = command.ExecuteReader())
            {
                List<Times> times = new List<Times>();
                while (reader.Read())
                {
                    var spanTimes = new TimeSpan[10];
                    for(int i = 0; i < spanTimes.Length; i++)
                    {
                        spanTimes[i] = (TimeSpan)reader[i + 1];
                    }
                    times.Add(new Times(spanTimes));
                }
                return times;
            }
        }
    }
}
