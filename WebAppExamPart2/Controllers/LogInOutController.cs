using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAppExamPart2.DAL;
using WebAppExamPart2.Models;

namespace WebAppExamPart2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LogInOutController : ControllerBase
    {
        private readonly ILogInOutRepository _logInOutRepo;
        private ILogger<LogInOutController> _logInOutLogger;
        private const string _loggetInn = "loggetInn";
        private const string _ikkeLoggetInn = "";

        public LogInOutController(ILogInOutRepository logInOutRepo, ILogger<LogInOutController> logInOutLogger)
        {
            _logInOutRepo = logInOutRepo;
            _logInOutLogger = logInOutLogger;
        }

        [HttpPost]
        [Route("loggInn")]
        public async Task<ActionResult> LoggInn(Bruker bruker)
        {
            if (ModelState.IsValid)
            {
                bool returnOK = await _logInOutRepo.LoggInn(bruker);
                if (!returnOK)
                {
                    _logInOutLogger.LogInformation("Innloggingen feilet for bruker");
                    HttpContext.Session.SetString(_loggetInn, _ikkeLoggetInn);
                    return Ok(false);
                }
                HttpContext.Session.SetString(_loggetInn, _loggetInn);
                return Ok(true);
            }
            _logInOutLogger.LogInformation("Feil i inputvalidering");
            return BadRequest("Feil i inputvalidering på server");
        }

        [HttpGet]
        [Route("loggut")]
        public bool LoggUt()
        {
            HttpContext.Session.SetString(_loggetInn, _ikkeLoggetInn);
            return true;

        }

        [HttpGet]
        [Route("sjekkIsLoggetInn")]
        public bool SjekkIsLoggetInn()
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString(_loggetInn))) {
                return false;
            }
            return true;
        }

    }
}
