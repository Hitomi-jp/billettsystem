using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAppExamPart2.Models;

namespace WebAppExamPart2.DAL
{
    public interface IBillettRepository
    {
        Task<int> LagreBillett(Billett billett);
        //Task<bool> EndreEnBillett(Billett endreBillett);
        Task<List<Billett>> HentAlleBilletter();
        Task<Billett> HentEnBillett(int kundeId);

        Task<bool> EndreBillett(Billett endreBillett);

        Task<bool> SlettEnBillett(int billettId);
        Task<List<Destinasjon>> HentAlleDestinasjon();
        IEnumerable HentGyldigDestinasjoner(int destinasjonId);
    }
}
