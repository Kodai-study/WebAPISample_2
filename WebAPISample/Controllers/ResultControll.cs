using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Primitives;
using WebAPISample.Models;
using System;

namespace WebAPISample.Controllers
{
    [Route("api/result")]
    [ApiController]
    public class ResultControll : ControllerBase
    {
        [HttpGet]
        public List<CheckResult> Get()
        {
            var rList = new List<CheckResult>();
            rList.Add(new CheckResult());
            rList.Add(new CheckResult());
            rList.Add(new CheckResult());
            return rList;
        }
    }
}
