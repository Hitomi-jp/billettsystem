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
    public class RuteKontrollerTest
    {

        private const string _loggetInn = "loggetInn";
        private const string _ikkeLoggetInn = "";

        private readonly Mock<IRuteRepository> mockRuteRepo = new Mock<IRuteRepository>();
        private readonly Mock<ILogger<RuteController>> mockLog = new Mock<ILogger<RuteController>>();

        private readonly Mock<HttpContext> mockHttpContext = new Mock<HttpContext>();
        private readonly MockHttpSession mockHttpSession = new MockHttpSession();
        
        [Fact]
        public async Task LagreRuteLoggetInnOk()
        {
            mockRuteRepo.Setup(rute => rute.LagreRute(It.IsAny<Rute>())).ReturnsAsync(true);

            var ruteController = new RuteController(mockRuteRepo.Object, mockLog.Object);

            mockHttpSession[_loggetInn] = _loggetInn;
            mockHttpContext.Setup(s => s.Session).Returns(mockHttpSession);
            ruteController.ControllerContext.HttpContext = mockHttpContext.Object;

            //Act
            var resultat = await ruteController.LagreRute(It.IsAny<Rute>());

            Assert.IsType<OkResult>(resultat.Result);
          

        }
        
        [Fact]
        public async Task LagreRuteLoggetSessionOut()
        {
            //Arrange
            mockRuteRepo.Setup(rute => rute.LagreRute(It.IsAny<Rute>())).ReturnsAsync(false);

            var ruteController = new RuteController(mockRuteRepo.Object, mockLog.Object);
            mockHttpSession[_loggetInn] = _loggetInn;
            mockHttpContext.Setup(s => s.Session).Returns(mockHttpSession);
            ruteController.ControllerContext.HttpContext = mockHttpContext.Object;

            //Act
            var resultat = await ruteController.LagreRute(It.IsAny<Rute>());

            Assert.Equal((int)HttpStatusCode.BadRequest, (resultat.Result as ObjectResult)?.StatusCode);
            Assert.Equal("Kunnde ikke lagre rute", (resultat.Result as ObjectResult)?.Value);
        }

        [Fact]
        public async Task LagreRuteFailModel()
        {
            //Arrange
            var rute1 = new Rute
            {
                Id = 1,
                BoatNavn = "",
                RuteFra = "Larvik",
                RuteTil = "Danmark",
                PrisEnvei = 600,
                PrisRabattBarn = "0",
                PrisToVei = 1200,
                PrisStandardLugar = 0,
                PrisPremiumLugar = 0,
                Avgang = "0800",
                AntallDagerEnVei = 1,
                AntallDagerToVei = 2
            };
            mockRuteRepo.Setup(k => k.LagreRute(rute1)).ReturnsAsync(false);

            var ruteController = new RuteController(mockRuteRepo.Object, mockLog.Object);

            ruteController.ModelState.AddModelError("BoatNavn", "Feil i inputvalidering på server");

            mockHttpSession[_loggetInn] = _loggetInn;
            mockHttpContext.Setup(s => s.Session).Returns(mockHttpSession);
            ruteController.ControllerContext.HttpContext = mockHttpContext.Object;

            var resultat = await ruteController.LagreRute(rute1);

            Assert.Equal((int)HttpStatusCode.BadRequest, (resultat.Result as ObjectResult)?.StatusCode);
            Assert.Equal("Feil i inputvalidering på server", (resultat.Result as ObjectResult)?.Value);

        }     
        
        [Fact]
        public async Task LagreRuteIkkeLoggetInn()
        {
           
            mockRuteRepo.Setup(k => k.LagreRute(It.IsAny<Rute>())).ReturnsAsync(true);

            var ruteController = new RuteController(mockRuteRepo.Object, mockLog.Object);

            mockHttpSession[_loggetInn] = _ikkeLoggetInn;
            mockHttpContext.Setup(s => s.Session).Returns(mockHttpSession);
            ruteController.ControllerContext.HttpContext = mockHttpContext.Object;

            var resultat = await ruteController.LagreRute(It.IsAny<Rute>());
            Assert.IsType<UnauthorizedResult>(resultat.Result);
        }

        [Fact]
        public async Task EndreRuteOk()
        {
            mockRuteRepo.Setup(rute => rute.EndreRute(It.IsAny<Rute>())).ReturnsAsync(true);

            var ruteController = new RuteController(mockRuteRepo.Object, mockLog.Object);

            mockHttpSession[_loggetInn] = _loggetInn;
            mockHttpContext.Setup(s => s.Session).Returns(mockHttpSession);
            ruteController.ControllerContext.HttpContext = mockHttpContext.Object;

            //Act
            var resultat = await ruteController.EndreRute(It.IsAny<Rute>());

            Assert.IsType<OkResult>(resultat.Result);
        }

        [Fact]
        public async Task EndreRuteIkkOk()
        {
            //Arrange
            mockRuteRepo.Setup(rute => rute.EndreRute(It.IsAny<Rute>())).ReturnsAsync(false);

            var ruteController = new RuteController(mockRuteRepo.Object, mockLog.Object);
            mockHttpSession[_loggetInn] = _ikkeLoggetInn;
            mockHttpContext.Setup(s => s.Session).Returns(mockHttpSession);
            ruteController.ControllerContext.HttpContext = mockHttpContext.Object;

            //Act
            var resultat = await ruteController.EndreRute(It.IsAny<Rute>());

            Assert.Equal((int)HttpStatusCode.NotFound, (resultat.Result as ObjectResult)?.StatusCode);
            Assert.Equal("Kunne ikke endre ruten", (resultat.Result as ObjectResult)?.Value);
        }

        [Fact]
        public async Task EndreRuteFailModel()
        {
            //Arrange
            var rute1 = new Rute
            {
                Id = 1,
                BoatNavn = "",
                RuteFra = "Larvik",
                RuteTil = "Danmark",
                PrisEnvei = 600,
                PrisRabattBarn = "0",
                PrisToVei = 1200,
                PrisStandardLugar = 0,
                PrisPremiumLugar = 0,
                Avgang = "0800",
                AntallDagerEnVei = 1,
                AntallDagerToVei = 2
            };
            mockRuteRepo.Setup(k => k.EndreRute(rute1)).ReturnsAsync(false);

            var ruteController = new RuteController(mockRuteRepo.Object, mockLog.Object);

            ruteController.ModelState.AddModelError("BoatNavn", "Feil i inputvalidering på server");

            mockHttpSession[_loggetInn] = _loggetInn;
            mockHttpContext.Setup(s => s.Session).Returns(mockHttpSession);
            ruteController.ControllerContext.HttpContext = mockHttpContext.Object;

            var resultat = await ruteController.EndreRute(rute1);

            Assert.Equal((int)HttpStatusCode.BadRequest, (resultat.Result as ObjectResult)?.StatusCode);
            Assert.Equal("Fiel i input validering på server", (resultat.Result as ObjectResult)?.Value);

        }

        [Fact]
        public async Task HentAlleRuterOk()
        {
            var rute1 = new Rute
            {
                Id = 1,
                BoatNavn = "Sun",
                RuteFra = "Oslo",
                RuteTil = "Bergen",
                PrisEnvei = 500,
                PrisRabattBarn = "1",
                PrisToVei = 1000,
                PrisStandardLugar = 200,
                PrisPremiumLugar = 300,
                Avgang = "0900",
                AntallDagerEnVei = 1,
                AntallDagerToVei = 3
            };
            var rute2 = new Rute
            {
                Id = 2,
                BoatNavn = "Funky",
                RuteFra = "Oslo",
                RuteTil = "Stavnger",
                PrisEnvei = 400,
                PrisRabattBarn = "3",
                PrisToVei = 1500,
                PrisStandardLugar = 200,
                PrisPremiumLugar = 400,
                Avgang = "0500",
                AntallDagerEnVei = 1,
                AntallDagerToVei = 4
            };
            var rute3 = new Rute
            {
                Id = 1,
                BoatNavn = "Super",
                RuteFra = "Larvik",
                RuteTil = "Danmark",
                PrisEnvei = 600,
                PrisRabattBarn = "0",
                PrisToVei = 1200,
                PrisStandardLugar = 0,
                PrisPremiumLugar = 0,
                Avgang = "0800",
                AntallDagerEnVei = 1,
                AntallDagerToVei = 2
            };

            var ruteList = new List<Rute>();
            ruteList.Add(rute1);
            ruteList.Add(rute2);
            ruteList.Add(rute3);

            mockRuteRepo.Setup(K => K.HentAlleRuter()).ReturnsAsync(ruteList);
            var ruteController = new RuteController(mockRuteRepo.Object, mockLog.Object);

            mockHttpSession[_loggetInn] = _loggetInn;
            mockHttpContext.Setup(s => s.Session).Returns(mockHttpSession);
            ruteController.ControllerContext.HttpContext = mockHttpContext.Object;

           List<Rute> resultat = await ruteController.HentAlleRuter(It.IsAny<Rute>(ruteList));

           Assert.Equal<List<Rute>>(ruteList, resultat);

        }

        [Fact]
        public async Task HentAlleRuterTomListet()
        {
            var ruteList = new List<Rute>();

            mockRuteRepo.Setup(K => K.HentAlleRuter()).ReturnsAsync(()=> null);
            var ruteController = new RuteController(mockRuteRepo.Object, mockLog.Object);

            mockHttpSession[_loggetInn] = _loggetInn;
            mockHttpContext.Setup(s => s.Session).Returns(mockHttpSession);
            ruteController.ControllerContext.HttpContext = mockHttpContext.Object;

            //List<Rute> resultat = await ruteController.HentAlleRuter();
           // Assert.Null(resultat);
        }
        [Fact]
        public async Task HentEnRuteOk()
        {
            var rute1 = new Rute
            {
                Id = 1,
                BoatNavn = "Sun",
                RuteFra = "Oslo",
                RuteTil = "Bergen",
                PrisEnvei = 500,
                PrisRabattBarn = "1",
                PrisToVei = 1000,
                PrisStandardLugar = 200,
                PrisPremiumLugar = 300,
                Avgang = "0900",
                AntallDagerEnVei = 1,
                AntallDagerToVei = 3
            };

            mockRuteRepo.Setup(K => K.HentEnRute(1)).ReturnsAsync(rute1);
            var ruteController = new RuteController(mockRuteRepo.Object, mockLog.Object);

            mockHttpSession[_loggetInn] = _loggetInn;
            mockHttpContext.Setup(s => s.Session).Returns(mockHttpSession);
            ruteController.ControllerContext.HttpContext = mockHttpContext.Object;

            //Rute resultat = await ruteController.HentEnRute(1);
            //Assert.Equal<Rute>(rute1,resultat);

        }

        [Fact]
        public async Task HentEnRuteTom()
        {
            mockRuteRepo.Setup(K => K.HentEnRute(1)).ReturnsAsync(()=>null);
            var ruteController = new RuteController(mockRuteRepo.Object, mockLog.Object);

            mockHttpSession[_loggetInn] = _loggetInn;
            mockHttpContext.Setup(s => s.Session).Returns(mockHttpSession);
            ruteController.ControllerContext.HttpContext = mockHttpContext.Object;

           // Rute resultat = await ruteController.HentEnRute(1);
            //Assert.Null(resultat);
        }        
        
              
        [Fact]
        public async Task SletteRuteOk()
        {

        }   
        
        [Fact]
        public async Task SletteRuteIkkOk()
        {

        }        
        
        [Fact]
        public async Task HentAlleStrekningerOk()
        {

        }   
        
        [Fact]
        public async Task HentAlleStrekningerIkkOk()
        {

        }     
        
        [Fact]
        public async Task SletteEnStrekningOk()
        {

        }   
        
        [Fact]
        public async Task SletteEnStrekningIkkOk()
        {

        }
    }
}
