using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Primitives;
using WebAPISample.Models;

namespace WebAPISample.Controllers
{
    [Route("api/insert")]
    [ApiController]
    public class Statistics : ControllerBase
    {
        
        [HttpGet]
        public void get([FromQuery] int num)
        {
            StringValues val = new StringValues("*");
            this.Response.Headers.Add("Access-Control-Allow-Origin", val);

            var date_sql = "SELECT YMD,scan,OK,ave_temp,ave_hun,ave_illum,ave_cyctime,max_cyctime,min_cyctime FROM View_DOK";
            
            var all_sql = "SELECT SUM(scan) AS al,SUM(OK) AS ok,100.0 * SUM(OK) / SUM(scan) FROM View_DOK";
        }
    }
}