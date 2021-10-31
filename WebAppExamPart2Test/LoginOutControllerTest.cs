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
    public class LoginOutControllerTest
    {

        private const string _loggetInn = "loggetInn";
        private const string _ikkeLoggetInn = "";

        private readonly Mock<ILogInOutRepository> mockLogInOut = new Mock<ILogInOutRepository>();
        private readonly Mock<ILogger<LogInOutController>> mockLog = new Mock<ILogger<LogInOutController>>();

        private readonly Mock<HttpContext> mockHttpContext = new Mock<HttpContext>();
        private readonly MockHttpSession mockHttpSession = new MockHttpSession();

        [Fact]
        public async Task LoggInnOK()
        {
            mockLogInOut.Setup( admin => admin.LoggInn(It.IsAny<Bruker>())).ReturnsAsync(true);

            var logInOutController = new LogInOutController(mockLogInOut.Object, mockLog.Object);

            mockHttpSession[_loggetInn] = _loggetInn;
            mockHttpContext.Setup(s => s.Session).Returns(mockHttpSession);
            logInOutController.ControllerContext.HttpContext = mockHttpContext.Object;

            // Act
            var resultat = await logInOutController.LoggInn(It.IsAny<Bruker>()) as OkObjectResult;

            // Assert 
            Assert.Equal((int)HttpStatusCode.OK, resultat.StatusCode);
            Assert.True((bool)resultat.Value);
        }

        [Fact]
        public async Task LoggInnFeilPassordEllerBruker()
        {
            mockLogInOut.Setup(admin => admin.LoggInn(It.IsAny<Bruker>())).ReturnsAsync(false);

            var logInOutController = new LogInOutController(mockLogInOut.Object, mockLog.Object);

            mockHttpSession[_loggetInn] = "";
            mockHttpContext.Setup(s => s.Session).Returns(mockHttpSession);
            logInOutController.ControllerContext.HttpContext = mockHttpContext.Object;

            // Act
            var resultat = await logInOutController.LoggInn(It.IsAny<Bruker>()) as OkObjectResult;

            // Assert 

            
           // Assert.Equal((int)HttpStatusCode.OK, resultat.StatusCode);
           // Assert.False((bool)resultat.Value);
        }

       /* [Fact]
        public async Task LoggInnInputFeil()
        {
            mockRep.Setup(k => k.LoggInn(It.IsAny<Bruker>())).ReturnsAsync(true);

            var kundeController = new KundeController(mockRep.Object, mockLog.Object);

            kundeController.ModelState.AddModelError("Brukernavn", "Feil i inputvalidering på server");

            mockSession[_loggetInn] = _loggetInn;
            mockHttpContext.Setup(s => s.Session).Returns(mockSession);
            kundeController.ControllerContext.HttpContext = mockHttpContext.Object;

            // Act
            var resultat = await kundeController.LoggInn(It.IsAny<Bruker>()) as BadRequestObjectResult;

            // Assert 
            Assert.Equal((int)HttpStatusCode.BadRequest, resultat.StatusCode);
            Assert.Equal("Feil i inputvalidering på server", resultat.Value);
        }*/

        /*[Fact]
        public void LoggUt()
        {
            var kundeController = new KundeController(mockRep.Object, mockLog.Object);

            mockHttpContext.Setup(s => s.Session).Returns(mockSession);
            mockSession[_loggetInn] = _loggetInn;
            kundeController.ControllerContext.HttpContext = mockHttpContext.Object;

            // Act
            kundeController.LoggUt();

            // Assert
            Assert.Equal(_ikkeLoggetInn, mockSession[_loggetInn]);
        }*/
    }
}
