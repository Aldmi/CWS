﻿using System;
using Microsoft.AspNetCore.Mvc;
using Serilog.Events;
using WebApiSwc.Extensions;

namespace WebApiSwc.Controllers
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