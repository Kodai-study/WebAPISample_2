using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Primitives;
using WebAPISample.Models;

namespace WebAPISample.Modules
{
    public class Statistics
    {
        public DateOnly YMD { get; set; }
        public int Scan { get; set; }
        public int OK { get; set; }
        public float ave_temp { get; set; }
        public float ave_hum { get; set; }
        public float ave_illum { get; set; }
        public TimeSpan ave_cycletime { get; set; }
        public TimeSpan min_cycletime { get; set; }
        public TimeSpan max_cycletime { get; set; }

        public Statistics(DateOnly YMD, int Scan, int OK, float ave_temp, float ave_hum,
           float ave_illum, TimeSpan ave_cycletime, int ave_cycletime_sec)
        {
            this.YMD = YMD;
            this.OK = OK;
            this.ave_temp = ave_temp;
            this.ave_hum = ave_hum;
            this.ave_illum = ave_illum;
            this.ave_cycletime = ave_cycletime;
        }
    }
}