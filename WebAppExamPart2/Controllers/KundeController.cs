using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebAppExamPart2.DAL;
using WebAppExamPart2.Models;

namespace WebAppExamPart2.Controllers
{
    [ApiController]
    [Route("api/[Controller]")]

    public class KundeController : ControllerBase
    {
        private readonly IKundeRepository _kundeDB;

        private ILogger<KundeController> _kundeLog;

        private const string _loggetInn = "loggetInn";

        public KundeController(IKundeRepository kundeDB, ILogger<KundeController> kundeLog)
        {
            _kundeDB = kundeDB;
            _kundeLog = kundeLog;
        }

        [HttpPost]
        public async Task<ActionResult<int>> LagreKunde(Kunde innKunde) //Kunde/Lagre
        {
          
            if (ModelState.IsValid)
            {
                int kundeId = await _kundeDB.LagreKunde(innKunde);
                if (kundeId == 0)
                {
                    _kundeLog.LogInformation("Kunne ikke lagre kunden");
                    return BadRequest("Kunne ikke lagre kunden");
                }
                return kundeId;
            }

            _kundeLog.LogInformation("Feil i inputValidering");
            return BadRequest("Feil i inputvalidering på server");
        }

        [HttpPut]
        public async Task<ActionResult> EndreKunde(Kunde endreKunde)
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString(_loggetInn)))
            {
                return Unauthorized();
            }

            if (ModelState.IsValid)
            {
                bool returnOk = await _kundeDB.EndreEnKunde(endreKunde);
                if (!returnOk)
                {
                    _kundeLog.LogInformation("Kunne ikke endre kunden");
                    return NotFound("Kunne ikke endre kunden");
                }
                return Ok();
            }
            _kundeLog.LogInformation("Feil i inputValidering");
            return BadRequest("Feil i inputvalidering på server");
        }

        [HttpDelete]
        [Route("slettEnKunde")]
        public async Task<ActionResult> SlettEnKunde(int kundeId)
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString(_loggetInn)))
            {
                return Unauthorized();
            }

            bool returnOk = await _kundeDB.SlettEnKunde(kundeId);
            if (!returnOk)
            {
                _kundeLog.LogInformation("Kunne ikke slette kunden");
                return NotFound("Kunne ikke slette kunden");
            }
            return Ok();
        }

      /*  [HttpDelete]
        [Route("slettAlle")]
        public async Task<ActionResult> SlettAlleKunder()
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString(_loggetInn)))
            {
                return Unauthorized();
            }

            bool returnOk = await _kundeDB.SlettAlleKunder();
            if (!returnOk)
            {
                _kundeLog.LogInformation("Kunne ikke slette alle");
                return NotFound("Kunne ikke slette alle");
            }
            return Ok("Alle ble slettet");
        }*/

        [HttpPost]
        [Route("lagreKreditt")]
        public async Task<ActionResult> LagreKreditt(Kreditt kredittInfo)
        { 

            bool returnOk = await _kundeDB.LagreKreditt(kredittInfo);
            if (!returnOk)
            {
                _kundeLog.LogInformation("Kunne ikke lagre kredittinfo");
                return BadRequest("Kunne ikke lagre kredittinfo");
            }
            return Ok();
        }

        [HttpGet]
        [Route("HentAlleKunder")]
        public async Task<ActionResult<Kunde>> HentAlleKunder()
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString(_loggetInn)))
            {
                return Unauthorized();
            }
            List<Kunde> alleKunder = await _kundeDB.HentAlleKunder();
            return Ok(alleKunder);
        }

        [HttpGet]
        [Route("hentEnKunde")]
        public async Task<ActionResult> HentEnKunde(int kundeId)
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString(_loggetInn)))
            {
                return Unauthorized();
            }

                Kunde en = await _kundeDB.HentEnKunde(kundeId);
                if (en == null)
                {
                    _kundeLog.LogInformation("Kunne ikke finne kunden");
                    return NotFound("Kunne ikke finne kunden");
                }
                return Ok(en);
            }
    }
}