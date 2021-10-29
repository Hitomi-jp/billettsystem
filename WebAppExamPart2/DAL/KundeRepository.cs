using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;
using WebAppExamPart2.Models;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace WebAppExamPart2.DAL
{
    public class KundeRepository : IKundeRepository
    {
        private readonly KundeContext _kundeDB;

        private ILogger<KundeRepository> _kundeLog;

   
        public KundeRepository(KundeContext kundeDb, ILogger<KundeRepository> kundeLog)
        {
            _kundeDB = kundeDb;
            _kundeLog = kundeLog;

        }

        public async Task<int> LagreKunde(Kunde innKunde)
        {
            try
            {
                var nyKundeRad = new Kunder();
                nyKundeRad.Fornavn = innKunde.Fornavn;
                nyKundeRad.Etternavn = innKunde.Etternavn;
                nyKundeRad.Telfonnr = innKunde.Telfonnr;
                nyKundeRad.Epost = innKunde.Epost;
                nyKundeRad.Adresse = innKunde.Adresse;

                var sjekkPoststed = await _kundeDB.PostSteder.FindAsync(innKunde.Postnr); // await and FindAsync check later
                if (sjekkPoststed == null)
                {
                    var nyPoststedRad = new PostSteder();
                    nyPoststedRad.Postnr = innKunde.Postnr;
                    nyPoststedRad.Poststed = innKunde.Poststed;
                    nyKundeRad.PostSteder = nyPoststedRad;
                }
                else
                {
                    nyKundeRad.PostSteder = sjekkPoststed;
                }
                _kundeDB.Kunder.Add(nyKundeRad);
                await _kundeDB.SaveChangesAsync();
                var kundeId = nyKundeRad.Id;
                return kundeId;
            }
            catch (Exception e)
            {
                Console.Write(e.Message);
                return 0;
            }
        }

        public async Task<bool> EndreEnKunde(Kunde endreKunde)
        {
            try
            {
                Kunder enKunde = await _kundeDB.Kunder.FindAsync(endreKunde.Id);
                if (enKunde.PostSteder.Postnr != endreKunde.Postnr)
                {
                    var sjekkPoststed = _kundeDB.PostSteder.Find(endreKunde.Postnr);
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

                await _kundeDB.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> SlettEnKunde(int id)
        {
            try
            {
                Kunder enKunde = await _kundeDB.Kunder.FindAsync(id);
                _kundeDB.Kunder.Remove(enKunde);
                await _kundeDB.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> SlettAlleKunder()
        {
            try
            {

                _kundeDB.Kunder.RemoveRange(_kundeDB.Kunder);
                _kundeDB.PostSteder.RemoveRange(_kundeDB.PostSteder);
                _kundeDB.Kreditt.RemoveRange(_kundeDB.Kreditt);
                _kundeDB.Billetter.RemoveRange(_kundeDB.Billetter);
                await _kundeDB.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> LagreKreditt(Kreditt kredittInfo) {
            try
            {
              var nyKreditt = new Kreditt();
              nyKreditt.Kortnummer = kredittInfo.Kortnummer;
              nyKreditt.KundeId = kredittInfo.KundeId;  
              nyKreditt.KortHolderNavn = kredittInfo.KortHolderNavn;
              nyKreditt.KortUtlopsdato = kredittInfo.KortUtlopsdato;
              nyKreditt.Cvc = kredittInfo.Cvc; 

              _kundeDB.Kreditt.Add(nyKreditt);
              await _kundeDB.SaveChangesAsync();
              return true;
            }
            catch (Exception e)
            {
                Console.Write(e.Message);
                return false;
            }
        }

        public async Task<List<Kunde>> HentAlleKunder()
        {
            try
            {
                List<Kunde> alleKundene = await _kundeDB.Kunder.Select(innKunde => new Kunde
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

        public async Task<Kunde> HentEnKunde(int id)
        {
            try
            {
                Kunder hentedKunde = await _kundeDB.Kunder.FindAsync(id);
                var enKunde = new Kunde()
                {
                    Id = hentedKunde.Id,
                    Fornavn = hentedKunde.Fornavn,
                    Etternavn = hentedKunde.Etternavn,
                    Telfonnr = hentedKunde.Telfonnr,
                    Epost = hentedKunde.Epost,
                    Adresse = hentedKunde.Adresse,
                    Postnr = hentedKunde.PostSteder.Postnr,
                    Poststed = hentedKunde.PostSteder.Poststed

                };
                return enKunde;
            }
            catch
            {
                return null;
            }
        }
      
        /*public static byte[] LagHash(string passord, byte[] salt)
        {
            return KeyDerivation.Pbkdf2(
                                password: passord,
                                salt: salt,
                                prf: KeyDerivationPrf.HMACSHA512,
                                iterationCount: 1000,
                                numBytesRequested: 32);
        }*/

        /* public static byte[] LagSalt()
         {
             var csp = new RNGCryptoServiceProvider();
             var salt = new byte[24];
             csp.GetBytes(salt);
             return salt;
         }*/

        /* public async Task<bool> LoggInn(Bruker bruker)
         {
             try
             {
                 Brukere funnetBruker = await _kundeDB.Brukere.FirstOrDefaultAsync(b => b.Brukernavn == bruker.Brukernavn);
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
         }*/
    }
}
