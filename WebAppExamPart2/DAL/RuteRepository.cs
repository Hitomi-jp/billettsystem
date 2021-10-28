using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAppExamPart2.Models;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System.Security.Cryptography;

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
                        StrekningId = innRute.RuteFra + "-" + innRute.RuteTil,
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
                        StrekningId = enRute.RuteFra + "-" + enRute.RuteTil,
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
                    StrekningId = strekning.StrekningId,
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

        public async Task<bool> Slett(int id)
        {
            try
            {
                Kunder enKunde = await _kundeDb.Kunder.FindAsync(id);
                _kundeDb.Kunder.Remove(enKunde);
                await _kundeDb.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> SlettAlle()
        {
            try
            {
                _kundeDb.Kunder.RemoveRange(_kundeDb.Kunder);
                _kundeDb.PostSteder.RemoveRange(_kundeDb.PostSteder);
                _kundeDb.Kreditt.RemoveRange(_kundeDb.Kreditt);
                _kundeDb.Billetter.RemoveRange(_kundeDb.Billetter);
                await _kundeDb.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> Endre(Kunde endreKunde)
        {
            try
            {
                Kunder enKunde = await _kundeDb.Kunder.FindAsync(endreKunde.Id);
                if (enKunde.PostSteder.Postnr != endreKunde.Postnr)
                {
                    var sjekkPoststed = _kundeDb.PostSteder.Find(endreKunde.Postnr);
                    if (sjekkPoststed == null)
                    {
                        var nyPoststedsRad = new PostSteder();
                        nyPoststedsRad.Postnr = endreKunde.Postnr;
                        nyPoststedsRad.Poststed = endreKunde.Poststed;
                        enKunde.PostSteder = nyPoststedsRad;
                    }
                    else
                    {
                        enKunde.PostSteder.Postnr = endreKunde.Postnr;
                    }
                }
                enKunde.Fornavn = endreKunde.Fornavn;
                enKunde.Etternavn = endreKunde.Etternavn;
                enKunde.Telfonnr = endreKunde.Telfonnr;
                enKunde.Epost = endreKunde.Epost;
                enKunde.Adresse = endreKunde.Adresse;

                await _kundeDb.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public static byte[] LagHash(string passord, byte[] salt)
        {
            return KeyDerivation.Pbkdf2(
                                password: passord,
                                salt: salt,
                                prf: KeyDerivationPrf.HMACSHA512,
                                iterationCount: 1000,
                                numBytesRequested: 32);
        }

        public static byte[] LagSalt()
        {
            var csp = new RNGCryptoServiceProvider();
            var salt = new byte[24];
            csp.GetBytes(salt);
            return salt;
        }

        public async Task<bool> LoggInn(Bruker bruker)
        {
            try
            {
                Brukere funnetBruker = await _kundeDb.Brukere.FirstOrDefaultAsync(b => b.Brukernavn == bruker.Brukernavn);
                byte[] hash = LagHash(bruker.Passord, funnetBruker.Salt);
                bool ok = hash.SequenceEqual(funnetBruker.Passord);
                if (ok)
                {
                    return true;

                }
                return false;

            }
            catch (Exception e)
            {
                _kundeLog.LogInformation(e.Message);
                return false;
            }
        }

        public async Task<List<Kunde>> HentAlle()
        {
            try
            {
                List<Kunde> alleKundene = await _kundeDb.Kunder.Select(innKunde => new Kunde
                {
                    Id = innKunde.Id,
                    Fornavn = innKunde.Fornavn,
                    Etternavn = innKunde.Etternavn,
                    Telfonnr = innKunde.Telfonnr,
                    Epost = innKunde.Epost,
                    Adresse = innKunde.Adresse,
                    Postnr = innKunde.PostSteder.Postnr,
                    Poststed = innKunde.PostSteder.Poststed

                }).ToListAsync();

                return alleKundene;
            }
            catch
            {
                return null;
            }
        }

        public async Task<List<Billett>> HentAlleBilletter()
        {
            try
            {
                List<Billett> alleBilletter = await _kundeDb.Billetter.Select(innBillett => new Billett
                {
                    Id = innBillett.Id,
                    KundeId = innBillett.KundeId,
                    DestinationFrom = innBillett.DestinationFrom,
                    DestinationTo = innBillett.DestinationTo,
                    TicketType = innBillett.TicketType,
                    LugarType = innBillett.LugarType,
                    AntallAdult = innBillett.AntallAdult,
                    AntallChild = innBillett.AntallChild,
                    DepartureDato = innBillett.DepartureDato,
                    ReturnDato = innBillett.ReturnDato,
                    Pris = innBillett.Pris,


                }).ToListAsync();

                return alleBilletter;
            }
            catch
            {
                return null;
            }
        }       
    }
}