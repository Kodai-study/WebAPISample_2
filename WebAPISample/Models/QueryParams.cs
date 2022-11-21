using Microsoft.AspNetCore.Mvc;
using System.Text;

namespace WebAPISample.Models
{
    public class TimeParams
    {
        [FromQuery(Name = "startTime")]
        public DateTime startTime { get; set; }

        [FromQuery(Name ="endTime")]
        public DateTime endTime { get; set; }

        public bool isParams
        {
            get
            {
                return startTime != DateTime.MinValue || endTime != DateTime.MinValue;
            }
        }

        public string CreateSQL()
        {
            var sql = new StringBuilder("");
            if (startTime != DateTime.MinValue && endTime != DateTime.MinValue)
            {
                sql.Append(" between '");
                sql.Append(startTime);
                sql.Append("' AND '");
                sql.Append(endTime);
                sql.Append("'");
            }
            else if (startTime != DateTime.MinValue)
            {
                sql.Append(" > '");
                sql.Append(startTime);
                sql.Append("'");
            }
            else if (endTime != DateTime.MinValue)
            {
                sql.Append(" < '");
                sql.Append(endTime);
                sql.Append("'");
            }
            return sql.ToString();
        }
    }
}