using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAppExamPart2.Models;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;

namespace WebAppExamPart2.DAL
{
    public class RuteRepository : IRuteRepository
    {
        private readonly KundeContext _kundeDb;
        private ILogger<RuteRepository> _kundeLog;

        public RuteRepository(KundeContext kundeDb, ILogger<RuteRepository> kundeLog)
        {
            _kundeDb = kundeDb;
            _kundeLog = kundeLog;
        }
        
        public async Task<bool> LagreRute(Rute innRute)
        {
            try
            {
                var nyRute = new Rute();
                nyRute.Id = innRute.Id;
                nyRute.BoatNavn = innRute.BoatNavn;
                nyRute.RuteFra = innRute.RuteFra;
                nyRute.RuteTil = innRute.RuteTil;
                nyRute.PrisEnvei = innRute.PrisEnvei;
                nyRute.PrisRabattBarn = innRute.PrisRabattBarn;
                nyRute.PrisToVei = innRute.PrisToVei;
                nyRute.PrisStandardLugar = innRute.PrisStandardLugar;
                nyRute.PrisPremiumLugar = innRute.PrisPremiumLugar;
                nyRute.Avgang = innRute.Avgang;
                nyRute.AntallDagerEnVei = innRute.AntallDagerEnVei;
                nyRute.AntallDagerToVei = innRute.AntallDagerToVei;
                
                var sjekStrekning = await _kundeDb.Strekninger.FindAsync(innRute.RuteFra + "-" + innRute.RuteTil);
                if (sjekStrekning == null) {
                    var nyStrekning = new Strekning(){
                        Id = innRute.RuteFra + "-" + innRute.RuteTil,
                        Fra = innRute.RuteFra,
                        Til = innRute.RuteTil
                    };
                    _kundeDb.Strekninger.Add(nyStrekning);
                }

                _kundeDb.Ruter.Add(nyRute);
                await _kundeDb.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<List<Rute>> HentAlleRuter()
        {
            try
            {
                List<Rute> alleRuter = await _kundeDb.Ruter.Select(rute => new Rute
                {
                    Id = rute.Id,
                    BoatNavn = rute.BoatNavn,
                    RuteFra = rute.RuteFra,
                    RuteTil = rute.RuteTil,
                    PrisEnvei = rute.PrisEnvei,
                    PrisToVei = rute.PrisToVei,
                    PrisRabattBarn = rute.PrisRabattBarn,
                    PrisStandardLugar = rute.PrisStandardLugar,
                    PrisPremiumLugar = rute.PrisPremiumLugar,
                    Avgang = rute.Avgang,
                    AntallDagerEnVei = rute.AntallDagerEnVei,
                    AntallDagerToVei = rute.AntallDagerToVei
                }).ToListAsync();

                return alleRuter;
            }
            catch
            {
                return null;
            }
        }

        public async Task<Rute> HentEnRute(int Id)
        {
            try
            {
                Rute hentetRute = await _kundeDb.Ruter.FindAsync(Id); 
                var enRute = new Rute()
                {
                    Id = hentetRute.Id,
                    BoatNavn = hentetRute.BoatNavn,
                    RuteFra = hentetRute.RuteFra,
                    RuteTil = hentetRute.RuteTil,
                    PrisEnvei = hentetRute.PrisEnvei,
                    PrisToVei = hentetRute.PrisToVei,
                    PrisRabattBarn = hentetRute.PrisRabattBarn,
                    PrisStandardLugar = hentetRute.PrisStandardLugar,
                    PrisPremiumLugar = hentetRute.PrisPremiumLugar,
                    Avgang = hentetRute.Avgang,
                    AntallDagerEnVei = hentetRute.AntallDagerEnVei,
                    AntallDagerToVei = hentetRute.AntallDagerToVei
                };
                return enRute;
            }
            catch 
            {
                return null;   
            }
        }


        public async Task<bool> EndreRute(Rute rute)
        {
            try
            {
                Rute enRute = await _kundeDb.Ruter.FindAsync(rute.Id);
                bool slettStrekningOk = await SlettEnStrekning(enRute.RuteFra + "-" + enRute.RuteTil);
                enRute.Id = rute.Id;
                enRute.BoatNavn = rute.BoatNavn;
                enRute.RuteFra = rute.RuteFra;
                enRute.RuteTil = rute.RuteTil;
                enRute.PrisEnvei = rute.PrisEnvei;
                enRute.PrisToVei = rute.PrisToVei;
                enRute.PrisRabattBarn = rute.PrisRabattBarn;
                enRute.PrisStandardLugar = rute.PrisStandardLugar;
                enRute.PrisPremiumLugar = rute.PrisPremiumLugar;
                enRute.Avgang = rute.Avgang;
                enRute.AntallDagerEnVei = rute.AntallDagerEnVei;
                enRute.AntallDagerToVei = rute.AntallDagerToVei;

                var sjekStrekning = await _kundeDb.Strekninger.FindAsync(enRute.RuteFra + "-" + enRute.RuteTil);
                if (sjekStrekning == null) {
                    var nyStrekning = new Strekning(){
                        Id = enRute.RuteFra + "-" + enRute.RuteTil,
                        Fra = enRute.RuteFra,
                        Til = enRute.RuteTil
                    };
                    _kundeDb.Strekninger.Add(nyStrekning);
                };
                await _kundeDb.SaveChangesAsync();
                return true;
            }
            catch 
            {
               return false; 
            }
        }
        
        public async Task<bool> SlettEnRute(int ruteId)
        {
            try
            {
                Rute enRute = await _kundeDb.Ruter.FindAsync(ruteId);
                _kundeDb.Ruter.Remove(enRute);
                bool ok = await SlettEnStrekning(enRute.RuteFra + "-" + enRute.RuteTil);
                await _kundeDb.SaveChangesAsync();
                return true;
            }
            catch 
            {
                return false;   
            }
        }
        public async Task<List<Strekning>> HentAlleStrekninger()
        {
            try
            {
                List<Strekning> alleStrekninger = await _kundeDb.Strekninger.Select(strekning => new Strekning
                {
                    Id = strekning.Id,
                    Fra = strekning.Fra,
                    Til = strekning.Til
                }).ToListAsync();
                return alleStrekninger;
            }
            catch
            {
                return null;
            }
        }

        public async Task<bool> SlettEnStrekning(string strekningId)
        {
            try
            {
                List<Rute> alleRuter = await _kundeDb.Ruter.Select(rute => new Rute
                    {
                        Id = rute.Id,
                        BoatNavn = rute.BoatNavn,
                        RuteFra = rute.RuteFra,
                        RuteTil = rute.RuteTil,
                        PrisEnvei = rute.PrisEnvei,
                        PrisToVei = rute.PrisToVei,
                        PrisRabattBarn = rute.PrisRabattBarn,
                        PrisStandardLugar = rute.PrisStandardLugar,
                        PrisPremiumLugar = rute.PrisPremiumLugar,
                        Avgang = rute.Avgang,
                        AntallDagerEnVei = rute.AntallDagerEnVei,
                        AntallDagerToVei = rute.AntallDagerToVei
                    }).ToListAsync();
                int antall = 0;
                for (int i = 0; i < alleRuter.Count; i++)
                {
                    if ((alleRuter[i].RuteFra + "-" + alleRuter[i].RuteTil) == strekningId) {
                        antall++;
                    }
                }
                if (antall <= 1) {
                    Strekning strekning = await _kundeDb.Strekninger.FindAsync(strekningId);
                    _kundeDb.Strekninger.Remove(strekning);
                    await _kundeDb.SaveChangesAsync();
                }
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}