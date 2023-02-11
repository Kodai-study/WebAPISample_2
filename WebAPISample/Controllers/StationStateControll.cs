using Microsoft.AspNetCore.Mvc;
using System.IO.MemoryMappedFiles;
using WebAPISample.JSONModels;

namespace WebAPISample.Controllers
{
    /// <summary>
    ///  ステーションの現在の状態についてのデータを返すAPI。
    /// </summary>
    [Route("api/stationStatus")]
    [ApiController]
    public class StationStateControll : ControllerBase
    {
        /// <summary>
        ///  現在のAPIの状態を返すAPI本体
        /// </summary>
        /// <see cref="StationState"/>
        [HttpGet]
        public StationState Get()
        {
            return new StationState();
        }
    }
}