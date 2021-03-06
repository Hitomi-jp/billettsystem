using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAppExamPart2.DAL;
using WebAppExamPart2.Models;

namespace WebAppExamPart2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BillettController : ControllerBase
    {
        private readonly IBillettRepository _kundeDB;

        private ILogger<BillettController> _kundeLog;

        private const string _loggetInn = "loggetInn";

        public BillettController(IBillettRepository kundeDB, ILogger<BillettController> kundeLog)
        {
            _kundeDB = kundeDB;
            _kundeLog = kundeLog;
        }

        [HttpPost]
        [Route("lagreBillett")]
        public async Task<ActionResult<int>> LagreBillett(Billett billett)
        {
            
            int billettId = await _kundeDB.LagreBillett(billett);
            if (billettId == 0)
            {
                _kundeLog.LogInformation("Kunne ikke lagre Billett");
                return BadRequest("Kunne ikke lagre Billett");
            }
            return billettId;
        }

        [HttpPut]
        public async Task<ActionResult> EndreBillett(Billett endreBillett) {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString(_loggetInn)))
            {
                return Unauthorized();
            }
            if (ModelState.IsValid)
            {
                bool returnOk = await _kundeDB.EndreBillett(endreBillett);
                if (!returnOk)
                {
                    _kundeLog.LogInformation("Kunne ikke endre billett");
                    return NotFound("Kunne ikke endre billett");
                }
                return Ok();
            }
            _kundeLog.LogInformation("Feil i inputValidering");
            return BadRequest("Feil i inputvalidering på server");
        }

        [HttpGet]
        [Route("hentAlleBilletter")]
        public async Task<ActionResult<Billett>> HentAlleBilletter()
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString(_loggetInn)))
            {
                return Unauthorized();
            }
            List<Billett> alleBilletter = await _kundeDB.HentAlleBilletter();
            return Ok(alleBilletter);
        }


        [HttpGet]
        [Route("hentEnBillett")]
        public async Task<ActionResult<Billett>> HentEnBillett(int billettId)
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString(_loggetInn)))
            {
                return Unauthorized();
            }

            Billett billett = await _kundeDB.HentEnBillett(billettId);
            if (billett == null)
            {
                _kundeLog.LogInformation("Kunne ikke finne billett");
                return NotFound("Kunne ikke finne billett");
            }
            return Ok(billett);
        }

        [HttpDelete]
        public async Task<ActionResult> SlettEnBillett(int billettId)
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString(_loggetInn)))
            {
                return Unauthorized();
            }
            bool slettOk = await _kundeDB.SlettEnBillett(billettId);
            if (!slettOk)
            {
                _kundeLog.LogInformation("Kunne ikke slette billett");
                return NotFound("Kunnde ikke slette billett");
            }
            return Ok();
        }



        [HttpGet]
        [Route("hentAlleDestinasjon")]
        public async Task<ActionResult<Destinasjon>> HentAlleDestinasjon()
        {

            List<Destinasjon> alleDestinasjon = await _kundeDB.HentAlleDestinasjon();
            return Ok(alleDestinasjon);
        }
    }
}
