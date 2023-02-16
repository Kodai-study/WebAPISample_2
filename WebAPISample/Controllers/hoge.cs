using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.DotNet.Scaffolding.Shared.CodeModifier.CodeChange;
using Microsoft.Extensions.Primitives;
using System.Text;
using WebAPISample.Data;
using WebAPISample.JSONModels;
using WebAPISample.Query;

namespace WebAPISample.Controllers
{

    /// <summary>
    ///  工程や、ステーション等の単位で、ワークの検査等に
    ///  かかった時間を表示するAPI
    /// </summary>
    [Route("api/sample1")]
    [ApiController]
    public class Hoge : ControllerBase
    {
        /// <summary>
        ///  APIのメインメソッド。
        ///  GETメソッド URL:/api/times
        /// </summary>
        /// <param name="timeParams"> 
        ///  ワーク搬入の開始時刻の範囲を指定して表示することができる。
        /// </param>
        /// <see cref="Times"/>
        /// <returns>
        ///  検索された時間データ一覧
        /// </returns>
        [HttpGet]   
        public OneParameter Get()
        {
            return new OneParameter();
        }

    }

    public class OneParameter
    {
        public Char Value { get; set; }
        public OneParameter() { Value = '〇'; }
    } 

}