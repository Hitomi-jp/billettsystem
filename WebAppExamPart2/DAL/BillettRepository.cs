using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using WebAppExamPart2.Models;

namespace WebAppExamPart2.DAL
{
    [ExcludeFromCodeCoverage]
    public class BillettRepository : IBillettRepository
    {
        private readonly KundeContext _kundeDB;

        private ILogger<BillettRepository> _kundeLog;


        public BillettRepository(KundeContext kundeDb, ILogger<BillettRepository> kundeLog)
        {
            _kundeDB = kundeDb;
            _kundeLog = kundeLog;

        }

        public async Task<int> LagreBillett(Billett innBillett)
        {
            try
            {
                var nyBillett = new Billett();
                nyBillett.DestinationFrom = innBillett.DestinationFrom;
                nyBillett.KundeId = innBillett.KundeId;
                nyBillett.RuteId = innBillett.RuteId;
                nyBillett.DestinationTo = innBillett.DestinationTo;
                nyBillett.TicketType = innBillett.TicketType;
                nyBillett.LugarType = innBillett.LugarType;
                nyBillett.AntallAdult = innBillett.AntallAdult;
                nyBillett.AntallChild = innBillett.AntallChild;
                nyBillett.DepartureDato = innBillett.DepartureDato;
                nyBillett.ReturnDato = innBillett.ReturnDato;
                nyBillett.Pris = innBillett.Pris;


                _kundeDB.Billetter.Add(nyBillett);
                await _kundeDB.SaveChangesAsync();
                var billettId = nyBillett.Id;
                return billettId;
            }
            catch
            {
                return 0;
            }
        }
        public async Task<bool> EndreBillett(Billett endreBillett)
        {
            try
            {
                Billett enBillett = await _kundeDB.Billetter.FindAsync(endreBillett.Id);
                enBillett.RuteId = endreBillett.RuteId;
                enBillett.DestinationFrom = endreBillett.DestinationFrom;
                enBillett.DestinationTo = endreBillett.DestinationTo;
                enBillett.TicketType = endreBillett.TicketType;
                enBillett.LugarType = endreBillett.LugarType;
                enBillett.DepartureDato = endreBillett.DepartureDato;
                enBillett.ReturnDato = endreBillett.ReturnDato;
                enBillett.AntallAdult = endreBillett.AntallAdult;
                enBillett.AntallChild = endreBillett.AntallChild;
                enBillett.Pris = endreBillett.Pris;
                
                await _kundeDB.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<List<Billett>> HentAlleBilletter()
        {
            try
            {
                List<Billett> alleBilletter = await _kundeDB.Billetter.Select(innBillett => new Billett
                {
                    Id = innBillett.Id,
                    KundeId = innBillett.KundeId,
                    RuteId = innBillett.RuteId,
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

        public async Task<Billett> HentEnBillett(int billettId)
        {
            try
            {
                Billett enBillett = await _kundeDB.Billetter.FindAsync(billettId);
                var hentetBillett = new Billett()
                {
                    Id = enBillett.Id,
                    KundeId = enBillett.KundeId,
                    RuteId = enBillett.RuteId,
                    DestinationTo = enBillett.DestinationTo,
                    DestinationFrom = enBillett.DestinationFrom,
                    TicketType = enBillett.TicketType,
                    LugarType = enBillett.LugarType,
                    DepartureDato = enBillett.DepartureDato,
                    ReturnDato = enBillett.ReturnDato,
                    AntallAdult = enBillett.AntallAdult,
                    AntallChild = enBillett.AntallChild,
                    Pris = enBillett.Pris

                };
                return hentetBillett;
            }
            catch
            {
                return null;
            }
        }

        public async Task<bool> SlettEnBillett(int billettId) {
            try
            {
                 Billett enBillett = await _kundeDB.Billetter.FindAsync(billettId);
                 _kundeDB.Billetter.Remove(enBillett);
                 await _kundeDB.SaveChangesAsync();
                 return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<List<Destinasjon>> HentAlleDestinasjon()
        {
            try
            {
                List<Destinasjon> alleDestinasjon = await _kundeDB.Destinasjoner.Select(innDestinasjon => new Destinasjon
                {
                    Id = innDestinasjon.Id,
                    Sted = innDestinasjon.Sted,

                }).ToListAsync();

                return alleDestinasjon;
            }
            catch
            {
                return null;
            }
        }

    }
}


