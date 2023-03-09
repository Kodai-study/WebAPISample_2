using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.DotNet.Scaffolding.Shared.CodeModifier.CodeChange;
using Microsoft.Extensions.Primitives;
using System.IO.MemoryMappedFiles;
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
    [Route("api/systemOperation")]
    [ApiController]
    public class SystemOperationControll : ControllerBase
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
        public bool Get([FromQuery] String operation)
        {
            MemoryMappedFile share_mem = MemoryMappedFile.OpenExisting("shared_memory");
            MemoryMappedViewAccessor accessor = share_mem.CreateViewAccessor();
            if (!accessor.CanRead)
                return false;

            try
            {
                if (StationState.lastMeomryAddress < 0)
                    new StationState();

                if (operation.ToUpper() == "START")
                    accessor.Write(StationState.lastMeomryAddress + 3, true);

                if (operation.ToUpper() == "STOP")
                    accessor.Write(StationState.lastMeomryAddress + 4, true);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return false;
            }
            return true;
        }
    }
}