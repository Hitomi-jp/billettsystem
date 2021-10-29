﻿using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAppExamPart2.Models;

namespace WebAppExamPart2.DAL
{
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
        /*public sync Task<bool> EndreEnBillett(Billett endreBillett)
        {
            try
            {
                Kunder enBillett = await _kundeDB.Billetter.FindAsync(endreBillett.Id);
                if (enBillett.PostSteder.Postnr != endreBillett.Postnr)
                {
                    var sjekkPoststed = _kundeDB.PostSteder.Find(endreBillett.Postnr);
                    if (sjekkPoststed == null)
                    {
                        var nyPoststedsRad = new PostSteder();
                        nyPoststedsRad.Postnr = endreBillett.Postnr;
                        nyPoststedsRad.Poststed = endreBillett.Poststed;
                        enBillett.PostSteder = nyPoststedsRad;
                    }
                    else
                    {
                        enBillett.PostSteder.Postnr = endreBillett.Postnr;
                    }
                }
                enBillett.Fornavn = endreBillett.Fornavn;
                enKuenBillettnde.Etternavn = endreBillett.Etternavn;
                enBillett.Telfonnr = endreBillett.Telfonnr;
                enBillett.Epost = endreBillett.Epost;
                enBillett.Adresse = endreBillett.Adresse;

                await _kundeDB.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }*/

        public async Task<List<Billett>> HentAlleBilletter()
        {
            try
            {
                List<Billett> alleBilletter = await _kundeDB.Billetter.Select(innBillett => new Billett
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


        public IEnumerable HentGyldigDestinasjoner(int desintasjonId)
        {
            IEnumerable destinasjoner = null;
            if (desintasjonId == 1)
            {
                destinasjoner = _kundeDB.Destinasjoner.Where(d => d.Id > 2 && d.Id < 6);
            }
            else if (desintasjonId == 2)
            {
                destinasjoner = _kundeDB.Destinasjoner.Where(d => d.Id == 1 || d.Id > 4 && d.Id < 8);
            }
            else if (desintasjonId == 3)
            {
                destinasjoner = _kundeDB.Destinasjoner.Where(d => d.Id > 0 && d.Id < 3 || d.Id > 7 && d.Id < 9);
            }
            else if (desintasjonId == 4)
            {
                destinasjoner = _kundeDB.Destinasjoner.Where(d => d.Id > 8 && d.Id <= 11);
            }
            else if (desintasjonId == 5)
            {
                destinasjoner = _kundeDB.Destinasjoner.Where(d => d.Id > 5 && d.Id < 9);
            }
            else if (desintasjonId == 6)
            {
                destinasjoner = _kundeDB.Destinasjoner.Where(d => d.Id > 0 && d.Id < 6);
            }
            else if (desintasjonId == 7)
            {
                destinasjoner = _kundeDB.Destinasjoner.Where(d => d.Id > 4 && d.Id < 7 || d.Id > 0 && d.Id < 3);
            }
            else if (desintasjonId == 8)
            {
                destinasjoner = _kundeDB.Destinasjoner.Where(d => d.Id > 0 && d.Id < 5 || d.Id > 9 && d.Id <= 11);
            }
            else if (desintasjonId == 9)
            {
                destinasjoner = _kundeDB.Destinasjoner.Where(d => d.Id > 2 && d.Id < 9);
            }
            else if (desintasjonId == 10)
            {
                destinasjoner = _kundeDB.Destinasjoner.Where(d => d.Id > 0 && d.Id < 6);
            }
            else if (desintasjonId == 11)
            {
                destinasjoner = _kundeDB.Destinasjoner.Where(d => d.Id > 2 && d.Id < 9);
            }
            else
            {
                destinasjoner = _kundeDB.Destinasjoner.Where(d => d.Id < 5);
            }

            return destinasjoner;
        }
    }
}


