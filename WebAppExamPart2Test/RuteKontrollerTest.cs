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

        private const string _loggetInn = "LoggetInn";
        private const string _ikkeLoggetInn = "";

        private readonly Mock<ILogger<RuteController>> mockLog = new Mock<ILogger<RuteController>>();
        private readonly Mock<IRuteRepository> mockRuteRepo = new Mock<IRuteRepository>();

        private readonly Mock<HttpContext> mockHttpContext = new Mock<HttpContext>();
        private readonly MockHttpSession mockHttpSession = new MockHttpSession();
        
        [Fact]
        public async Task LagreRuteLoggetInnOk()
        {
            //Arrange
           /*var rute1 = new Rute
            {
                Id = 1,
                BoatNavn = "Cash Flow",
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
            };*/

   

            mockRuteRepo.Setup(rute => rute.LagreRute(It.IsAny<Rute>())).ReturnsAsync(true);

            var ruteController = new RuteController(mockRuteRepo.Object, mockLog.Object);

            mockHttpSession[_loggetInn] = _loggetInn;
            mockHttpContext.Setup(s => s.Session).Returns(mockHttpSession);
            ruteController.ControllerContext.HttpContext = mockHttpContext.Object;

            //Act
            var resultat = await ruteController.LagreRute(It.IsAny<Rute>()) as OkObjectResult;

            //Asset
            Assert.Equal((int)HttpStatusCode.OK, resultat.StatusCode);
            Assert.Equal("Lagret rute", resultat.Value);
        }
    }
}
