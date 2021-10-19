using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using WebAppExamPart2.DAL;
using WebAppExamPart2.Models;
using System.Collections;

namespace WebAppExamPart2.Controllers
{
    [ApiController]
    [Route("api/[Controller]")]
  
    public class KundeController : ControllerBase 
    {
        private readonly IKundeRepository _kundeDB;

        private ILogger<KundeController> _kundeLog;

        public KundeController(IKundeRepository kundeDB, ILogger<KundeController> kundeLog)
        {
            _kundeDB = kundeDB;
            _kundeLog = kundeLog;
        }
        //[HttpPost]
        public async Task<ActionResult<int>> Lagre(Kunde innKunde) //Kunde/Lagre
        {
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
            List<Kunde> alleKunder = await _kundeDB.HentAlle();
            return Ok(alleKunder);
        } 
        
        [HttpGet]
        [Route ("hentAlleBilletter")]
        public async Task<ActionResult<Billett>> HentAlleBilletter()
        {
            List<Billett> alleBilletter = await _kundeDB.HentAlleBilletter();
            return Ok(alleBilletter);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Billett>> HentEnBillett(int kundeId) {
            Billett billett = await _kundeDB.HentEnBillett(kundeId);
            if (billett == null) {
                _kundeLog.LogInformation("Kunne ikke finne kunden");
                return NotFound("Kunne ikke finne kunden");
            }
            return Ok(billett);
		 }

        //[HttpGet]
        public async Task<ActionResult<Destinasjon>> HentAlleDestinasjon()
        {
            List<Destinasjon> alleDestinasjon = await _kundeDB.HentAlleDestinasjon();
            return Ok(alleDestinasjon);
        }
        public IEnumerable HentGyldigDestinasjoner(int destinasjonId)
        {
            IEnumerable destinasjoner = _kundeDB.HentGyldigDestinasjoner(destinasjonId);
            return destinasjoner;
        }

        [HttpPut]
        public async Task<ActionResult> Endre(Kunde endreKunde)
        {
            if(ModelState.IsValid)
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
            bool returnOk = await _kundeDB.SlettAlle();
               if (!returnOk) { 
                _kundeLog.LogInformation("Kunne ikke slette alle");
                return NotFound("Kunne ikke slette alle");
            }
            return Ok("Alle ble slettet");
        }

        //[HttpPost]
         public async Task<ActionResult> LagreKreditt(Kreditt kredittInfo)
        {
                bool returnOk = await _kundeDB.LagreKreditt(kredittInfo);
                if (!returnOk)
                {
                    _kundeLog.LogInformation("Kunne ikke lagre kredittinfo");
                    return BadRequest("Kunne ikke lagre kredittinfo");
                }
                return Ok("Kredittinfo ble lagret");
        }

        //[HttpPost]
        public async Task<ActionResult> LagreBillett(Billett billett)
        {
            bool returnOk = await _kundeDB.LagreBillett(billett);
            if (!returnOk)
            {
                _kundeLog.LogInformation("Kunne ikke lagre Billett");
                return BadRequest("Kunne ikke lagre Billett");
            }
            return Ok("Billett ble lagret");

        }

        [HttpGet]
         public async Task<ActionResult> LoggInn(Bruker bruker) 
        {
            Console.WriteLine(bruker);
            if (ModelState.IsValid)
            {
                
                bool returnOK = await _kundeDB.LoggInn(bruker);
                if (!returnOK)
                {
                    _kundeLog.LogInformation("Innloggingen feilet for bruker"+bruker.Brukernavn);
                    return Ok(false);
                }
                return Ok(true);
            }
            _kundeLog.LogInformation("Feil i inputvalidering");
            return BadRequest("Feil i inputvalidering på server");
        }

    }
}