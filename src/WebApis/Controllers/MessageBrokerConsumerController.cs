using System;
using System.Threading;
using System.Threading.Tasks;
using Autofac.Features.Indexed;
using Infrastructure.Background.Abstarct;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Serilog;

namespace WebApiSwc.Controllers
{
    [Produces("application/json")]
    [Route("api/MessageBrokerConsumer")]
    public class MessageBrokerConsumerController : Controller
    {
        #region fields
        private readonly ISimpleBackground _backgroundMessageBrokConsum;   //TODO: заменгить на ConsumerMessageBroker4InputData<TIn> и вызывать методы бекграунда чеерз него.
        private readonly ILogger _logger;
        #endregion



        #region ctor
        public MessageBrokerConsumerController(  
            IConfiguration config,
            IIndex<string, ISimpleBackground> background,
            ILogger logger)

        {
            _logger = logger;
            var backgroundName = config["MessageBrokerConsumer4InData:Name"]; //TODO: работу с _backgroundMessageBrokConsum вынести в отдельный сервис
            _backgroundMessageBrokConsum = background[backgroundName];
        }
        #endregion



        #region Api
        // GET api/InputData/GetMessageBrokerConsumerBgState
        [HttpGet("GetMessageBrokerConsumerBgState")]
        public async Task<IActionResult> GetMessageBrokerConsumerBgState()
        {
            var bgState = _backgroundMessageBrokConsum.IsStarted ? "Started" : "Stoped";
            await Task.CompletedTask;
            return new JsonResult(bgState);
        }


        /// <summary>
        /// Запустить бекграунд слушателя выходных сообшений от messageBroker
        /// </summary>
        // GET api/InputData/StartMessageBrokerConsumerBg
        [HttpPut("StartMessageBrokerConsumerBg")]
        public async Task<IActionResult> StartMessageBrokerConsumerBg()
        {
            try
            {
                if (_backgroundMessageBrokConsum.IsStarted)
                {
                    ModelState.AddModelError("StartMessageBrokerConsumerBg", "Listener already started !!!");
                    return BadRequest(ModelState);
                }

                await _backgroundMessageBrokConsum.StartAsync(CancellationToken.None);
                return Ok("Listener starting");
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "{Type}", "Ошибка в InputDataController/StartMessageBrokerConsumerBg");
                throw;
            }
        }


        /// <summary>
        /// Запустить бекграунд слушателя выходных сообшений от messageBroker
        /// </summary>
        [HttpPut("StopMessageBrokerConsumerBg")]
        public async Task<IActionResult> StopMessageBrokerConsumerBg()
        {
            try
            {
                if (!_backgroundMessageBrokConsum.IsStarted)
                {
                    ModelState.AddModelError("StopMessageBrokerConsumerBg", "Listener already staopped !!!");
                    return BadRequest(ModelState);
                }

                await _backgroundMessageBrokConsum.StopAsync(CancellationToken.None);
                return Ok("Listener Stoping");
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "{Type}", "Ошибка в InputDataController/StopMessageBrokerConsumerBg");
                throw;
            }
        }
        #endregion
    }
}