using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAppExamPart2.DAL;
using WebAppExamPart2.Models;
using System.Collections;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace WebAppExamPart2.Controllers
{
    [ApiController]
    [Route("api/[Controller]")]
    public class RuteController : ControllerBase
    {
        private readonly IRuteRepository _kundeDB;

        private readonly IRuteRepository _ruteRepo;

        private ILogger<RuteController> _ruteLogger;

        private const string _loggetInn = "loggetInn";

        public RuteController(IRuteRepository ruteRepo, ILogger<RuteController> ruteLogger, IRuteRepository kundeDB)
        {
            _ruteRepo = ruteRepo;
            _ruteLogger = ruteLogger;
            _kundeDB = kundeDB;
        }


        [HttpPost]
        public async Task<ActionResult<bool>> LagreRute(Rute innRute)
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString(_loggetInn))) {
                return Unauthorized();
            }
            if (ModelState.IsValid)
            {
                bool isLagret = await _ruteRepo.LagreRute(innRute);
                if (!isLagret)
                {
                    _ruteLogger.LogInformation("Kunne ikke lagre rute");
                    return BadRequest("Kunnde ikke lagre rute");
                }
                return Ok();
            }
            _ruteLogger.LogInformation("Feil i input validering");
            return BadRequest("Feil i inputvalidering på server");
        }


        [HttpGet]
        public async Task<ActionResult<Rute>> HentAlleRuter()
        {
            // if (string.IsNullOrEmpty(HttpContext.Session.GetString(_loggetInn))) {
            //     return Unauthorized();
            // }
            List<Rute> alleRuter = await _ruteRepo.HentAlleRuter();
            return Ok(alleRuter);
        }


        [HttpGet("{ruteId}")]
        public async Task<ActionResult<Rute>> HentEnRute(int ruteId)
        {
            // if (string.IsNullOrEmpty(HttpContext.Session.GetString(_loggetInn))) {
            //     return Unauthorized();
            // }
            Rute enRute = await _ruteRepo.HentEnRute(ruteId);
            if (enRute == null) {
                _ruteLogger.LogInformation("Kunnde ikke finen ruten");
                return NotFound("Kunnde ikke finne ruten");
            }
            return Ok(enRute);
        }


        [HttpPut]
        public async Task<ActionResult<bool>> EndreRute(Rute endreRute)
        {
            // if (string.IsNullOrEmpty(HttpContext.Session.GetString(_loggetInn))) {
            //     return Unauthorized();
            // }
            if (ModelState.IsValid)
            {
                bool endreOk = await _ruteRepo.EndreRute(endreRute);
                if (!endreOk)
                {
                    _ruteLogger.LogInformation("Kunne ikke endre rute");
                    return NotFound("Kunne ikke endre ruten");
                }
                return Ok();
            }
            _ruteLogger.LogInformation("Feil i input validering");
            return BadRequest("Fiel i input validering på server");
        }

        [HttpDelete("{ruteId}")]
        public async Task<ActionResult> SlettEnRute(int ruteId)
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString(_loggetInn))) {
                return Unauthorized();
            }
            bool slettOk = await _ruteRepo.SlettEnRute(ruteId);
            if (!slettOk)
            {
                _ruteLogger.LogInformation("Kunne ikke slette rute");
                return NotFound("Kunne ikke slette ruten");
            }
            return Ok();

        }

        [HttpGet]
        [Route("hentAlleStrekninger")]
        public async Task<ActionResult<Rute>> HentAlleStrekninger()
        {
            // if (string.IsNullOrEmpty(HttpContext.Session.GetString(_loggetInn))) {
            //     return Unauthorized();
            // }
            List<Strekning> alleStrekninger = await _ruteRepo.HentAlleStrekninger();
            return Ok(alleStrekninger);
        }

        [HttpGet]
        [Route("hentAlle")]
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
                _ruteLogger.LogInformation("Kunne ikke slette kunden");
                return NotFound("Kunne ikke slette kunden");
            }
            return Ok("Kunde ble slettet");
        }

        [HttpDelete]
        [Route("slettAlle")]
        public async Task<ActionResult> SlettAlle()
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString(_loggetInn)))
            {
                return Unauthorized();
            }

            bool returnOk = await _kundeDB.SlettAlle();
            if (!returnOk)
            {
                _ruteLogger.LogInformation("Kunne ikke slette alle");
                return NotFound("Kunne ikke slette alle");
            }
            return Ok("Alle ble slettet");
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
                    _ruteLogger.LogInformation("Kunne ikke endre kunden");
                    return NotFound("Kunne ikke endre kunden");
                }
                return Ok("Kunde ble endret");
            }
            _ruteLogger.LogInformation("Feil i inputValidering");
            return BadRequest("Feil i inputvalidering på server");
        }

        [HttpGet]
        [Route("sjekkAdminLoggetInn")]
        public  bool SjekkLoggetInn() {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString(_loggetInn)))
            {
                return false;
            }
            return true;
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
                    _ruteLogger.LogInformation("Innloggingen feilet for bruker" + bruker.Brukernavn);
                    HttpContext.Session.SetString(_loggetInn, "");
                    return Ok(false);
                }
                HttpContext.Session.SetString(_loggetInn, "LoggetInn");
                return Ok(true);
            }
            _ruteLogger.LogInformation("Feil i inputvalidering");
            return BadRequest("Feil i inputvalidering på server");
        }

        [HttpGet]
        [Route("loggut")]
        public bool LoggUt()
        {
            HttpContext.Session.SetString(_loggetInn, "");
            return true;

        }
    }
}