using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAppExamPart2.Models;

namespace WebAppExamPart2.DAL
{
    public interface IKundeRepository
    {
        Task<int> LagreKunde(Kunde innKunde);
        Task<bool> EndreEnKunde(Kunde endreKunde);
        Task<bool> SlettEnKunde(int kundeId);
        Task<bool> SlettAlleKunder();
        Task<bool> LagreKreditt(Kreditt kredittInfo);
        Task<List<Kunde>> HentAlleKunder();
        Task<Kunde> HentEnKunde(int id);
      
    }
}
