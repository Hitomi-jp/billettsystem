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
        Task<int> Lagre(Kunde innKunde);
        //Task<bool> Endre(Kunde endreKunde);
        //Task<bool> Slett(int id);
        //Task<bool> SlettAlle();
        Task<bool> LagreKreditt(Kreditt kredittInfo);
        Task<int> LagreBillett(Billett billett);
        //Task<List<Kunde>> HentAlle();
        //Task<List<Billett>> HentAlleBilletter();
        Task<Kunde> HentEn(int id);
        Task<Billett> HentEnBillett(int kundeId);
        Task<List<Destinasjon>> HentAlleDestinasjon();
        IEnumerable HentGyldigDestinasjoner(int destinasjonId);
        //Task<bool> LoggInn(Bruker bruker);
    }
}
