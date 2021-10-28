using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAppExamPart2.Models;

namespace WebAppExamPart2.DAL
{
    public interface IRuteRepository
    {
        Task<bool> LagreRute(Rute rute);
        Task<List<Rute>> HentAlleRuter();
        Task<Rute> HentEnRute(int ruteId);
        Task<bool> EndreRute(Rute rute);
        Task<bool> SlettEnRute(int ruteId);
        Task<List<Strekning>> HentAlleStrekninger();
        Task<List<Kunde>> HentAlle();
        Task<List<Billett>> HentAlleBilletter();
        Task<bool> Slett(int id);
        Task<bool> SlettAlle();
        Task<bool> Endre(Kunde endreKunde);
        Task<bool> SlettEnStrekning (string strekningId);
        Task<bool> LoggInn(Bruker bruker);

    }
}