using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebServer.Extensions;
using Serilog.Events;

namespace WebServer.Controllers
{
    [Produces("application/json")]
    [Route("api/LoggerApp")]
    public class LoggerAppController : Controller
    {

        // GET api/LoggerApp
        [HttpGet()]
        public IActionResult MinLogEventLevel()
        {
            return Ok(SerilogExtensions.GetMinimumLevel.ToString());
        }




        // PUT api/LoggerApp/SetMinLogEventLevel
        [HttpPut("SetMinLogEventLevel")]
        public IActionResult SetMinLogEventLevel([FromBody] string eventLevel)
        {
            if (Enum.TryParse<LogEventLevel>(eventLevel, out var res))
            {
                SerilogExtensions.ChangeLogEventLevel(res);
                return Ok($"Минимальный уровень логирования изменен на: {res.ToString()}");
            }

            return BadRequest($"Неверный уровень логирования {eventLevel}");
        }
    }
}