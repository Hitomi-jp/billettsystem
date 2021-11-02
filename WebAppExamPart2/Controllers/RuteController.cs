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
      
        private readonly IRuteRepository _ruteRepo;

        private ILogger<RuteController> _ruteLogger;

        private const string _loggetInn = "loggetInn";

        public RuteController(IRuteRepository ruteRepo, ILogger<RuteController> ruteLogger)
        {
            _ruteRepo = ruteRepo;
            _ruteLogger = ruteLogger;
            
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
        public async Task<ActionResult<Rute>> HentAlleRuter()// Rute rute is deleted
        {
           
            List<Rute> alleRuter = await _ruteRepo.HentAlleRuter();
            return Ok(alleRuter);
        }


        [HttpGet("{ruteId}")]
        public async Task<ActionResult<Rute>> HentEnRute(int ruteId)
        {
            
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
            if (string.IsNullOrEmpty(HttpContext.Session.GetString(_loggetInn))) {
                 return Unauthorized();
            }
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
            List<Strekning> alleStrekninger = await _ruteRepo.HentAlleStrekninger();
            return Ok(alleStrekninger);
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
    }
}