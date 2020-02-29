using System;
using Microsoft.AspNetCore.Mvc;

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
            return new JsonResult($"MemUsage= {mem}Mb");
        }

        #endregion
    }
}