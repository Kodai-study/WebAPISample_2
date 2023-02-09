using Microsoft.AspNetCore.Mvc;
using System.IO.MemoryMappedFiles;
using WebAPISample.Modules;

namespace WebAPISample.Modules
{
    [Route("api/stationStatus")]
    [ApiController]
    public class StationStateControll : ControllerBase
    {

        [HttpGet]
        public StationState Get()
        {
            return new StationState();
        }
    }
}