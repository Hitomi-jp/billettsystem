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
            mockKundeRepo.Setup(k => k.LagreKunde(kunde1)).ReturnsAsync(kunde1.Id);

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

            mockKundeRepo.Setup(k => k.EndreEnKunde(kunde1)).ReturnsAsync(true);

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
            mockKundeRepo.Setup(k => k.EndreEnKunde(It.IsAny<Kunde>())).ReturnsAsync(true);

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
            var resultat = await kundeController.SlettEnKunde(It.IsAny<int>()) as OkObjectResult;

            // Assert 
            Assert.Equal((int)HttpStatusCode.OK, resultat.StatusCode);
            Assert.Equal("Kunde ble slettet", resultat.Value);
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
            mockKundeRepo.Setup(k => k.SlettEnKunde(It.IsAny<int>())).ReturnsAsync(true);

            var kundeController = new KundeController(mockKundeRepo.Object, mockLog.Object);

            mockHttpSession[_loggetInn] = _ikkeLoggetInn;
            mockHttpContext.Setup(s => s.Session).Returns(mockHttpSession);
            kundeController.ControllerContext.HttpContext = mockHttpContext.Object;

            // Act
            var resultat = await kundeController.SlettEnKunde(It.IsAny<int>());

            // Assert 
            Assert.IsType<UnauthorizedResult>(resultat);
        }
    }
}

