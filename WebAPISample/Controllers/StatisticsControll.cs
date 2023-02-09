using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Primitives;
using System.Text;
using WebAPISample.Modules;

namespace WebAPISample.Modules
{
    [Route("api/statistics")]
    [ApiController]
    public class StatisticsControll : ControllerBase
    {

        [HttpGet]
        public void Get([FromQuery] TimeParams timeSearch)
        {
            StringValues val = new StringValues("*");
            this.Response.Headers.Add("Access-Control-Allow-Origin", val);

            StringBuilder get = new StringBuilder("SELECT YMD,scan,OK,ave_temp,ave_hun,ave_illum,ave_cyctime,max_cyctime,min_cyctime FROM View_DOK");

            List<int> targetIndexes = new List<int>();

            if (timeSearch.IsSetParams)
            {
                get.Append(" WHERE YMD ");
                get.Append(timeSearch.CreateSQL());
            }

            using (var command = new SqlCommand(get.ToString(), Parameters.sqlConnection))
            using (var reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    int id = (int)reader[0];
                    if (targetIndexes.IndexOf(id) < 0)
                        targetIndexes.Add(id);
                }
            }

        }
    }
}