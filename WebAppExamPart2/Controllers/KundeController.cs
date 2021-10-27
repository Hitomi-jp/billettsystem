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
        public async Task<ActionResult<int>> Lagre(Kunde innKunde) //Kunde/Lagre
        {
            /*  if (string.IsNullOrEmpty(HttpContext.Session.GetString(_loggetInn)))
              {
                  return Unauthorized();
              }*/

            if (ModelState.IsValid)
            {
                int kundeId = await _kundeDB.Lagre(innKunde);
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

        [HttpGet]
        public async Task<ActionResult<Kunde>> HentAlle()
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString(_loggetInn)))
            {
                return Unauthorized();
            }
            List<Kunde> alleKunder = await _kundeDB.HentAlle();
            return Ok(alleKunder);
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

        [HttpGet("{id}")]
        public async Task<ActionResult<Billett>> HentEnBillett(int kundeId)
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString(_loggetInn)))
            {
                return Unauthorized();
            }

            Billett billett = await _kundeDB.HentEnBillett(kundeId);
            if (billett == null)
            {
                _kundeLog.LogInformation("Kunne ikke finne kunden");
                return NotFound("Kunne ikke finne kunden");
            }
            return Ok(billett);
        }

        [HttpGet]
        [Route("hentAlleDestinasjon")]
        public async Task<ActionResult<Destinasjon>> HentAlleDestinasjon()
        {
            // if (string.IsNullOrEmpty(HttpContext.Session.GetString(_loggetInn)))
            // {
            //     return Unauthorized();
            // }

            List<Destinasjon> alleDestinasjon = await _kundeDB.HentAlleDestinasjon();
            return Ok(alleDestinasjon);
        }

        [HttpGet]
        [Route("hentGyldigDestinasjoner")]
        public IEnumerable HentGyldigDestinasjoner(int destinasjonId)
        {
            IEnumerable destinasjoner = _kundeDB.HentGyldigDestinasjoner(destinasjonId);
            return destinasjoner;
        }

        [HttpPut]
        public async Task<ActionResult> Endre(Kunde endreKunde)
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString(_loggetInn)))
            {
                return Unauthorized();
            }

            if (ModelState.IsValid)
            {
                bool returnOk = await _kundeDB.Endre(endreKunde);
                if (!returnOk)
                {
                    _kundeLog.LogInformation("Kunne ikke endre kunden");
                    return NotFound("Kunne ikke endre kunden");
                }
                return Ok("Kunde ble endret");
            }
            _kundeLog.LogInformation("Feil i inputValidering");
            return BadRequest("Feil i inputvalidering på server");
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> HentEn(int id)
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString(_loggetInn)))
            {
                return Unauthorized();
            }

            if (ModelState.IsValid)
            {
                Kunde en = await _kundeDB.HentEn(id);
                if (en == null)
                {
                    _kundeLog.LogInformation("Kunne ikke finne kunden");
                    return NotFound("Kunne ikke finne kunden");
                }
                return Ok(en);
            }
            _kundeLog.LogInformation("Feil i inputValidering");
            return BadRequest("Feil i inputValidering på server");
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Slett(int id)
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString(_loggetInn)))
            {
                return Unauthorized();
            }

            bool returnOk = await _kundeDB.Slett(id);
            if (!returnOk)
            {
                _kundeLog.LogInformation("Kunne ikke slette kunden");
                return NotFound("Kunne ikke slette kunden");
            }
            return Ok("Kunde ble slettet");
        }

        [HttpDelete]
        public async Task<ActionResult> SlettAlle()
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString(_loggetInn)))
            {
                return Unauthorized();
            }

            bool returnOk = await _kundeDB.SlettAlle();
            if (!returnOk)
            {
                _kundeLog.LogInformation("Kunne ikke slette alle");
                return NotFound("Kunne ikke slette alle");
            }
            return Ok("Alle ble slettet");
        }

        [HttpPost]
        [Route("lagreKreditt")]
        public async Task<ActionResult> LagreKreditt(Kreditt kredittInfo)
        {
            // if (string.IsNullOrEmpty(HttpContext.Session.GetString(_loggetInn)))
            // {
            //     return Unauthorized();
            // }

            bool returnOk = await _kundeDB.LagreKreditt(kredittInfo);
            if (!returnOk)
            {
                _kundeLog.LogInformation("Kunne ikke lagre kredittinfo");
                return BadRequest("Kunne ikke lagre kredittinfo");
            }
            return Ok();
        }

        [HttpPost]
        [Route("lagreBillett")]
        public async Task<ActionResult> LagreBillett(Billett billett)
        {
            // if (string.IsNullOrEmpty(HttpContext.Session.GetString(_loggetInn)))
            // {
            //     return Unauthorized();
            // }

            bool returnOk = await _kundeDB.LagreBillett(billett);
            if (!returnOk)
            {
                _kundeLog.LogInformation("Kunne ikke lagre Billett");
                return BadRequest("Kunne ikke lagre Billett");
            }
            return Ok();
        }

        [HttpPost]
        [Route("loggInn")]
        public async Task<ActionResult> LoggInn(Bruker bruker)
        {
            Console.WriteLine(bruker);
            if (ModelState.IsValid)
            {
                bool returnOK = await _kundeDB.LoggInn(bruker);
                if (!returnOK)
                {
                    _kundeLog.LogInformation("Innloggingen feilet for bruker" + bruker.Brukernavn);
                    HttpContext.Session.SetString(_loggetInn, "");
                    return Ok(false);
                }
                HttpContext.Session.SetString(_loggetInn, "LoggetInn");
                return Ok(true);
            }
            _kundeLog.LogInformation("Feil i inputvalidering");
            return BadRequest("Feil i inputvalidering på server");
        }

        [HttpGet]
        [Route("api/kunde/loggut")]
        public void LoggUt()
        {
            HttpContext.Session.SetString(_loggetInn, "");
        }
    }
}