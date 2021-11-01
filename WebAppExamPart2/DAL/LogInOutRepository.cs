using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;
using WebAppExamPart2.Models;

namespace WebAppExamPart2.DAL
{
    [ExcludeFromCodeCoverage]
    public class LogInOutRepository : ILogInOutRepository
    {

        private readonly KundeContext _kundeDb;
        private ILogger<LogInOutRepository> _kundeLog;

        public LogInOutRepository(KundeContext kundeDb, ILogger<LogInOutRepository> kundeLog)
        {
            _kundeDb = kundeDb;
            _kundeLog = kundeLog;
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
    }
}
