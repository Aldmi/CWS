using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WebApiSwc.DTO.JSON.DevicesStateDto;

namespace WebApiSwc.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    public class StatisticController: Controller
    {
        #region Methode

        // GET api/Statistic/MemUsage
        [HttpGet("MemUsage")]
        public IActionResult MemUsage()
        {
            var mem = GC.GetTotalMemory(false) / 1000000.0;
            return new JsonResult("$MemUsage= {mem}Mb");
        }

        #endregion
    }
}