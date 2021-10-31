using Moq;
using System;
using System.Threading.Tasks;
using WebAppExamPart2.Controllers;
using WebAppExamPart2.DAL;
using WebAppExamPart2.Models;
using Xunit;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;

namespace WebAppExamPart2Test
{
    public class KundeControllerTest
    {

        private readonly Mock<ILogger<KundeController>> mockLog = new Mock<ILogger<KundeController>>();
        private readonly Mock<IKundeRepository> mockKundeRepo = new Mock<IKundeRepository>();

       
        [Fact]
        public async Task LagreOK()
        {
            var innKunde = new Kunde
            {
                Id = 1,
                Fornavn = "Liam",
                Etternavn = "Hansen",
                Telfonnr = "67235975",
                Epost = "test@oslomet.no",
                Adresse = "Pilestredet 35",
                Postnr = "0166",
                Poststed = "Oslo"
            };

            mockKundeRepo.Setup(k => k.LagreKunde(innKunde)).ReturnsAsync(innKunde.Id);
            var kundeController = new KundeController(mockKundeRepo.Object, mockLog.Object);
            var resultat = await kundeController.LagreKunde(innKunde);

            Assert.Equal(innKunde.Id,resultat.Value);
        }


        [Fact]
        public async Task LagreFeil()
        {
            var innKunde = new Kunde
            {
                Id = 1,
                Fornavn = "Liam",
                Etternavn = "Hansen",
                Telfonnr = "67235975",
                Epost = "test@oslomet.no",
                Adresse = "Pilestredet 35",
                Postnr = "0166",
                Poststed = "Oslo"
            };

            mockKundeRepo.Setup(k => k.LagreKunde(innKunde)).ReturnsAsync(0);
            var kundeController = new KundeController(mockKundeRepo.Object, mockLog.Object);
            var resultat = await kundeController.LagreKunde(innKunde);
            Assert.Equal(400, (resultat.Result as ObjectResult)?.StatusCode);

        }
    }
}
