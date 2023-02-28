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
    [Route("api/totalInspectionData")]
    [ApiController]
    public class TotalInspectionControll : ControllerBase
    {

        [HttpGet]   
        public TotalInspectionDatas Get()
        {
            StringValues val = new("*");
            this.Response.Headers.Add("Access-Control-Allow-Origin", val);
            return new TotalInspectionDatas();
        }
    }

}