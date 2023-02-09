using Microsoft.AspNetCore.Mvc;
using System.Text;

namespace WebAPISample.Query
{
    /// <summary>
    /// 時間に関するクエリを管理するクラス
    /// </summary>
    public class ResultSearchParams
    {
        [FromQuery(Name = "NG_COLUM")]
        public String ngColum { get; set; }




    }
}