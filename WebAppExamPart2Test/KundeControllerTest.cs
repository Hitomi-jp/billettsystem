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

            Assert.Equal(kunde1.Id, resultat.Value);

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
            mockKundeRepo.Setup(k => k.LagreKunde(kunde1)).ReturnsAsync(null);

            var kundeController = new KundeController(mockKundeRepo.Object, mockLog.Object);

            kundeController.ModelState.AddModelError("Fornavn", "Feil i inputvalidering på server");

            var resultat = await kundeController.LagreKunde(kunde1);

            Assert.Equal((int)HttpStatusCode.BadRequest, (resultat.Result as ObjectResult)?.StatusCode);
            Assert.Equal("Feil i inputvalidering på server", (resultat.Result as ObjectResult)?.Value);
        }

        [Fact]
        public async Task EndreLoggetInnOK()
        {
            // Arrange

            mockKundeRepo.Setup(k => k.EndreEnKunde(It.IsAny<Kunde>())).ReturnsAsync(true);

            var kundeController = new KundeController(mockKundeRepo.Object, mockLog.Object);

            mockHttpSession[_loggetInn] = _loggetInn;
            mockHttpContext.Setup(s => s.Session).Returns(mockHttpSession);
            kundeController.ControllerContext.HttpContext = mockHttpContext.Object;

            // Act
            var resultat = await kundeController.EndreKunde(It.IsAny<Kunde>());

            // Assert 

            Assert.IsType<OkResult>(resultat);
        }

        [Fact]
        public async Task EndreLoggetInnIkkeOK()
        {
            // Arrange

            mockKundeRepo.Setup(k => k.EndreEnKunde(It.IsAny<Kunde>())).ReturnsAsync(false);

            var kundeController = new KundeController(mockKundeRepo.Object, mockLog.Object);

            mockHttpSession[_loggetInn] = _loggetInn;
            mockHttpContext.Setup(s => s.Session).Returns(mockHttpSession);
            kundeController.ControllerContext.HttpContext = mockHttpContext.Object;

            // Act
            var resultat = await kundeController.EndreKunde(It.IsAny<Kunde>()) as NotFoundObjectResult;

            // Assert 
            Assert.Equal((int)HttpStatusCode.NotFound, resultat.StatusCode);
            Assert.Equal("Kunne ikke endre kunden", resultat.Value);
        }

        [Fact]
        public async Task EndreLoggetInnFeilModel()
        {
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

            mockKundeRepo.Setup(k => k.EndreEnKunde(kunde1)).ReturnsAsync(false);

            var kundeController = new KundeController(mockKundeRepo.Object, mockLog.Object);

            kundeController.ModelState.AddModelError("Fornavn", "Feil i inputvalidering på server");

            mockHttpSession[_loggetInn] = _loggetInn;
            mockHttpContext.Setup(s => s.Session).Returns(mockHttpSession);
            kundeController.ControllerContext.HttpContext = mockHttpContext.Object;

            // Act
            var resultat = await kundeController.EndreKunde(kunde1) as BadRequestObjectResult;

            // Assert 
            Assert.Equal((int)HttpStatusCode.BadRequest, resultat.StatusCode);
            Assert.Equal("Feil i inputvalidering på server", resultat.Value);
        }

        [Fact]
        public async Task EndreIkkeLoggetInn()
        {
            mockKundeRepo.Setup(k => k.EndreEnKunde(It.IsAny<Kunde>())).ReturnsAsync(false);

            var kundeController = new KundeController(mockKundeRepo.Object, mockLog.Object);

            mockHttpSession[_loggetInn] = _ikkeLoggetInn;
            mockHttpContext.Setup(s => s.Session).Returns(mockHttpSession);
            kundeController.ControllerContext.HttpContext = mockHttpContext.Object;

            // Act
            var resultat = await kundeController.EndreKunde(It.IsAny<Kunde>());

            // Assert 

            Assert.IsType<UnauthorizedResult>(resultat);

        }

        [Fact]
        public async Task SlettLoggetInnOK()
        {
            // Arrange

            mockKundeRepo.Setup(k => k.SlettEnKunde(It.IsAny<int>())).ReturnsAsync(true);

            var kundeController = new KundeController(mockKundeRepo.Object, mockLog.Object);

            mockHttpSession[_loggetInn] = _loggetInn;
            mockHttpContext.Setup(s => s.Session).Returns(mockHttpSession);
            kundeController.ControllerContext.HttpContext = mockHttpContext.Object;

            // Act
            var resultat = await kundeController.SlettEnKunde(It.IsAny<int>());

            // Assert 
            Assert.IsType<OkResult>(resultat);
        }

        [Fact]
        public async Task SlettLoggetInnIkkeOK()
        {
            // Arrange

            mockKundeRepo.Setup(k => k.SlettEnKunde(It.IsAny<int>())).ReturnsAsync(false);

            var kundeController = new KundeController(mockKundeRepo.Object, mockLog.Object);

            mockHttpSession[_loggetInn] = _loggetInn;
            mockHttpContext.Setup(s => s.Session).Returns(mockHttpSession);
            kundeController.ControllerContext.HttpContext = mockHttpContext.Object;

            // Act
            var resultat = await kundeController.SlettEnKunde(It.IsAny<int>()) as NotFoundObjectResult;

            // Assert 
            Assert.Equal((int)HttpStatusCode.NotFound, resultat.StatusCode);
            Assert.Equal("Kunne ikke slette kunden", resultat.Value);
        }

        [Fact]
        public async Task SletteIkkeLoggetInn()
        {
            mockKundeRepo.Setup(k => k.SlettEnKunde(It.IsAny<int>())).ReturnsAsync(false);

            var kundeController = new KundeController(mockKundeRepo.Object, mockLog.Object);

            mockHttpSession[_loggetInn] = _ikkeLoggetInn;
            mockHttpContext.Setup(s => s.Session).Returns(mockHttpSession);
            kundeController.ControllerContext.HttpContext = mockHttpContext.Object;

            // Act
            var resultat = await kundeController.SlettEnKunde(It.IsAny<int>());

            // Assert 
            Assert.IsType<UnauthorizedResult>(resultat);
        }


       [Fact]
        public async Task LagreKredittOK()
        {
            var kredittKort1 = new Kreditt()
            {
                Id = 1,
                KundeId = 1,
                Kortnummer = "1478523698521478",
                KortHolderNavn = "Henrik Solberg",
                KortUtlopsdato = "0124",
                Cvc ="369"
            };

            mockKundeRepo.Setup(kreditt => kreditt.LagreKreditt(kredittKort1)).ReturnsAsync(true);
            var kundeController = new KundeController(mockKundeRepo.Object, mockLog.Object);
            var resultat = await kundeController.LagreKreditt(kredittKort1);

            Assert.IsType<OkResult>(resultat);

        }


        [Fact]
        public async Task LagreKredittFeilModel()
        {
            var kredittKort1 = new Kreditt()
            {
                Id = 1,
                KundeId = 1,
                Kortnummer = "1478523698521478",
                KortHolderNavn = "",
                KortUtlopsdato = "0124",
                Cvc = "369"
            };

            mockKundeRepo.Setup(kreditt => kreditt.LagreKreditt(kredittKort1)).ReturnsAsync(false);
            var kundeController = new KundeController(mockKundeRepo.Object, mockLog.Object);

            kundeController.ModelState.AddModelError("KortHolderNavn", "Feil i inputvalidering på server");

            var resultat = await kundeController.LagreKreditt(kredittKort1) as BadRequestObjectResult;

            Assert.IsType<BadRequestObjectResult>(resultat);
            Assert.Equal("Kunne ikke lagre kredittinfo", resultat.Value);

        }

        [Fact]
        public async Task HentAlleKunderLoggetInnOK()
        {
            // Arrange
            var kunde1 = new Kunde
            {
                Id = 1,
                Fornavn = "Taro",
                Etternavn = "Suzuki",
                Adresse = "Hokkaidoveien 42",
                Postnr = "0014",
                Poststed = "Sapporo"
            };
            var kunde2 = new Kunde
            {
                Id = 2,
                Fornavn = "Ume",
                Etternavn = "Tamura",
                Adresse = "Aomoriveien 41",
                Postnr = "0024",
                Poststed = "Aomori"
            };
            var kunde3 = new Kunde
            {
                Id = 3,
                Fornavn = "Take",
                Etternavn = "Hayashi",
                Adresse = "Akitaveien 40",
                Postnr = "0034",
                Poststed = "Akita"
            };

            var kundeListe = new List<Kunde>();
            kundeListe.Add(kunde1);
            kundeListe.Add(kunde2);
            kundeListe.Add(kunde3);

            mockKundeRepo.Setup(k => k.HentAlleKunder()).ReturnsAsync(kundeListe);

            var kundeController = new KundeController(mockKundeRepo.Object, mockLog.Object);

            mockHttpSession[_loggetInn] = _loggetInn;
            mockHttpContext.Setup(s => s.Session).Returns(mockHttpSession);
            kundeController.ControllerContext.HttpContext = mockHttpContext.Object;

            // Act
            var resultat = await kundeController.HentAlleKunder();

            // Assert 
            Assert.Equal((int)HttpStatusCode.OK, (resultat.Result as ObjectResult)?.StatusCode);
            Assert.Equal<List<Kunde>>((resultat.Result as ObjectResult)?.Value as List<Kunde>, kundeListe);

        }

        [Fact]
        public async Task HentAlleIkkeLoggetInn()
        {
            // Arrange

            mockKundeRepo.Setup(alleKunder => alleKunder.HentAlleKunder()).ReturnsAsync(It.IsAny<List<Kunde>>());

            var kundeController = new KundeController(mockKundeRepo.Object, mockLog.Object);

            mockHttpSession[_loggetInn] = _ikkeLoggetInn;
            mockHttpContext.Setup(s => s.Session).Returns(mockHttpSession);
            kundeController.ControllerContext.HttpContext = mockHttpContext.Object;

            // Act
            var resultat = await kundeController.HentAlleKunder();

            // Assert
            Assert.IsType<UnauthorizedResult>(resultat.Result);
        }

        [Fact]
        public async Task HentEnLoggetInnOK()
        {
            // Arrange
            var kunde1 = new Kunde
            {
                Id = 1,
                Fornavn = "Jiro",
                Etternavn = "Tanaka",
                Adresse = "Iwateveien 40",
                Postnr = "0039",
                Poststed = "Morioka"
            };

            mockKundeRepo.Setup(k => k.HentEnKunde(It.IsAny<int>())).ReturnsAsync(kunde1);

            var kundeController = new KundeController(mockKundeRepo.Object, mockLog.Object);

            mockHttpSession[_loggetInn] = _loggetInn;
            mockHttpContext.Setup(s => s.Session).Returns(mockHttpSession);
            kundeController.ControllerContext.HttpContext = mockHttpContext.Object;

            // Act
            var resultat = await kundeController.HentEnKunde(It.IsAny<int>()) as OkObjectResult;

            // Assert 
            Assert.Equal((int)HttpStatusCode.OK, resultat.StatusCode);
            Assert.Equal<Kunde>(kunde1, (Kunde)resultat.Value);
        }

        [Fact]
        public async Task HentEnLoggetInnIkkeOK()
        {
            // Arrange

            mockKundeRepo.Setup(k => k.HentEnKunde(It.IsAny<int>())).ReturnsAsync(() => null); // -> return no kunde

            var kundeController = new KundeController(mockKundeRepo.Object, mockLog.Object);

            mockHttpSession[_loggetInn] = _loggetInn;
            mockHttpContext.Setup(s => s.Session).Returns(mockHttpSession);
            kundeController.ControllerContext.HttpContext = mockHttpContext.Object;

            // Act
            var resultat = await kundeController.HentEnKunde(It.IsAny<int>()) as NotFoundObjectResult;

            // Assert 
            Assert.Equal((int)HttpStatusCode.NotFound, resultat.StatusCode);
            Assert.Equal("Kunne ikke finne kunden", resultat.Value);
        }

         [Fact]
         public async Task HentEnIkkeLoggetInn()
         {
             mockKundeRepo.Setup(k => k.HentEnKunde(It.IsAny<int>())).ReturnsAsync(() => null);

             var kundeController = new KundeController(mockKundeRepo.Object, mockLog.Object);

             mockHttpSession[_loggetInn] = _ikkeLoggetInn;
             mockHttpContext.Setup(s => s.Session).Returns(mockHttpSession);
             kundeController.ControllerContext.HttpContext = mockHttpContext.Object;

            // Act
            var resultat = await kundeController.HentEnKunde(It.IsAny<int>());

            // Assert 
            Assert.IsType<UnauthorizedResult>(resultat);
            //Assert.Equal((int)HttpStatusCode.Unauthorized, resultat.StatusCode);

        }

       [Fact]
       public async Task HentEnLoggetInnFeilServer()
       {
           mockKundeRepo.Setup(k => k.HentEnKunde(It.IsAny<int>())).ReturnsAsync(() => null);

           var kundeController = new KundeController(mockKundeRepo.Object, mockLog.Object);

           mockHttpSession[_loggetInn] = _loggetInn;
           mockHttpContext.Setup(s => s.Session).Returns(mockHttpSession);
           kundeController.ControllerContext.HttpContext = mockHttpContext.Object;

           // Act
           var resultat = await kundeController.HentEnKunde(It.IsAny<int>());

            // Assert 
            Assert.Equal((int)HttpStatusCode.NotFound, (resultat as ObjectResult)?.StatusCode);
            Assert.Equal("Kunne ikke finne kunden", (resultat as ObjectResult)?.Value);
       }
    }
}

