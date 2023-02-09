using Microsoft.AspNetCore.Mvc;
using System.IO.MemoryMappedFiles;
using WebAPISample.Models;

namespace WebAPISample.Controllers
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
