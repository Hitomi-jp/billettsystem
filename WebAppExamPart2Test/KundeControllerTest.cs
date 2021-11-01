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
using System.Net;

namespace WebAppExamPart2Test
{
    public class KundeControllerTest
    {

        private const string _loggetInn = "loggetInn";
        private const string _ikkeLoggetInn = "";

        private readonly Mock<HttpContext> mockHttpContext = new Mock<HttpContext>();
        private readonly MockHttpSession mockHttpSession = new MockHttpSession();

        private readonly Mock<ILogger<KundeController>> mockLog = new Mock<ILogger<KundeController>>();
        private readonly Mock<IKundeRepository> mockKundeRepo = new Mock<IKundeRepository>();

       
        [Fact]
        public async Task LagreOK()
        {
            var kunde1 = new Kunde
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

            mockKundeRepo.Setup(k => k.LagreKunde(kunde1)).ReturnsAsync(kunde1.Id);
            var kundeController = new KundeController(mockKundeRepo.Object, mockLog.Object);
            var resultat = await kundeController.LagreKunde(kunde1);

            Assert.Equal(kunde1.Id,resultat.Value);
        
        }


        [Fact]
        public async Task LagreFeil()
        {
            var kunde1 = new Kunde
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

            mockKundeRepo.Setup(k => k.LagreKunde(kunde1));
            var kundeController = new KundeController(mockKundeRepo.Object, mockLog.Object);
            
            var resultat = await kundeController.LagreKunde(kunde1);
            
            Assert.Equal((int)HttpStatusCode.BadRequest, (resultat.Result as ObjectResult)?.StatusCode);
            Assert.Equal("Kunne ikke lagre kunden", (resultat.Result as ObjectResult)?.Value);

        }

        [Fact]
        public async Task LagreFeilModel()
        {
            //Arrange
            var kunde1 = new Kunde

            {
                Id = 1,
                Fornavn = "",
                Etternavn = "Hansen",
                Telfonnr = "67235975",
                Epost = "test@oslomet.no",
                Adresse = "Pilestredet 35",
                Postnr = "0166",
                Poststed = "Oslo"
            };
            mockKundeRepo.Setup(k => k.LagreKunde(kunde1)).ReturnsAsync(kunde1.Id);

            var kundeController = new KundeController(mockKundeRepo.Object, mockLog.Object);

            kundeController.ModelState.AddModelError("Fornavn", "Feil i inputvalidering på server");

            var resultat = await kundeController.LagreKunde(kunde1);

            Assert.Equal((int)HttpStatusCode.BadRequest, (resultat.Result as ObjectResult)?.StatusCode);
            Assert.Equal("Feil i inputvalidering på server", (resultat.Result as ObjectResult)?.Value);
        }

        [Fact]
        public async Task EndreEnKunde()
        {
        
        }

    }
}
