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
using System.Diagnostics.CodeAnalysis;
using System.Collections;

namespace WebAppExamPart2Test
{
    public class BillettControllerTest
    {
        private const string _loggetInn = "loggetInn";
        private const string _ikkeLoggetInn = "";

        private readonly Mock<HttpContext> mockHttpContext = new Mock<HttpContext>();
        private readonly MockHttpSession mockHttpSession = new MockHttpSession();

        private readonly Mock<ILogger<BillettController>> mockLog = new Mock<ILogger<BillettController>>();
        private readonly Mock<IBillettRepository> mockBillettRepo = new Mock<IBillettRepository>();

        [Fact]
        public async Task LagreBillettOK()
        {
            var billett1 = new Billett
            {
                Id = 1,
                KundeId = 1,
                RuteId = 2,
                DestinationFrom = "Oslo",
                DestinationTo = "Kiel",
                TicketType = "Retur",
                LugarType = "Premium",
                DepartureDato = "2021-12-24",
                ReturnDato = "2021-12-30",
                AntallAdult = 2,
                AntallChild = 2,
                Pris = 1300
            };


          //Arrange
            mockBillettRepo.Setup(billett => billett.LagreBillett(billett1)).ReturnsAsync(billett1.Id);
            var billettController = new BillettController(mockBillettRepo.Object, mockLog.Object);
            
           //Act
            var resultat = await billettController.LagreBillett(billett1);

            //Assert
            Assert.Equal(billett1.Id, resultat.Value);

        }

        [Fact]
        public async Task LagreBillettFeil()
        {
            var billett1 = new Billett
            {
                Id = 1,
                KundeId = 1,
                RuteId = 2,
                DestinationFrom = "Oslo",
                DestinationTo = "Kiel",
                TicketType = "Retur",
                LugarType = "Premium",
                DepartureDato = "2021-12-24",
                ReturnDato = "2021-12-30",
                AntallAdult = 2,
                AntallChild = 2,
                Pris = 1300
            };


            mockBillettRepo.Setup(billett => billett.LagreBillett(billett1));
            var billettController = new BillettController(mockBillettRepo.Object, mockLog.Object);

            var resultat = await billettController.LagreBillett(billett1);

            Assert.Equal((int)HttpStatusCode.BadRequest, (resultat.Result as ObjectResult)?.StatusCode);
            Assert.Equal("Kunne ikke lagre Billett", (resultat.Result as ObjectResult)?.Value);

        }
        [Fact]
        public async Task HentAlleBilletterLoggetInnOK()
        {
            // Arrange
            var billett1 = new Billett
            {
                Id = 1,
                KundeId = 1,
                RuteId = 1,
                DestinationFrom = "Oslo",
                DestinationTo = "Kiel",
                TicketType = "En vei",
                LugarType = "Standard",
                DepartureDato = "2022-02-22",
                ReturnDato = "2022-02-26",
                AntallAdult = 2,
                AntallChild = 0,
                Pris = 1300
            };
            var billett2 = new Billett
            {
                Id = 2,
                KundeId = 2,
                RuteId = 2,
                DestinationFrom = "Danmark",
                DestinationTo = "Stavanger",
                TicketType = "Retur",
                LugarType = "Premium",
                DepartureDato = "2021-12-24",
                ReturnDato = "2021-12-30",
                AntallAdult = 2,
                AntallChild = 2,
                Pris = 1300
            };
            var billett3 = new Billett
            {
                Id = 3,
                KundeId = 3,
                RuteId = 3,
                DestinationFrom = "Oslo",
                DestinationTo = "Danmark",
                TicketType = "En vei",
                LugarType = "Premium",
                DepartureDato = "2022-01-01",
                ReturnDato = "2022-01-07",
                AntallAdult = 1,
                AntallChild = 2,
                Pris = 2500
            };

            var billettList = new List<Billett>();
            billettList.Add(billett1);
            billettList.Add(billett2);
            billettList.Add(billett3);

            mockBillettRepo.Setup(billetter => billetter.HentAlleBilletter()).ReturnsAsync(billettList);

            var billettController = new BillettController(mockBillettRepo.Object, mockLog.Object);

            mockHttpSession[_loggetInn] = _loggetInn;
            mockHttpContext.Setup(s => s.Session).Returns(mockHttpSession);
            billettController.ControllerContext.HttpContext = mockHttpContext.Object;

            // Act
            var resultat = await billettController.HentAlleBilletter();

            // Assert 
            Assert.Equal((int)HttpStatusCode.OK, (resultat.Result as ObjectResult)?.StatusCode);
            Assert.Equal<List<Billett>>((resultat.Result as ObjectResult)?.Value as List<Billett>, billettList);

        }

        [Fact]
        public async Task HentAlleBilletterLoggetInnOKFeilDB()
        {
            // Arrange
            var billett1 = new Billett
            {
                Id = 1,
                KundeId = 1,
                RuteId = 1,
                DestinationFrom = "Oslo",
                DestinationTo = "Kiel",
                TicketType = "En vei",
                LugarType = "Standard",
                DepartureDato = "2022-02-22",
                ReturnDato = "2022-02-26",
                AntallAdult = 2,
                AntallChild = 0,
                Pris = 1300
            };
            var billett2 = new Billett
            {
                Id = 2,
                KundeId = 2,
                RuteId = 2,
                DestinationFrom = "Danmark",
                DestinationTo = "Stavanger",
                TicketType = "Retur",
                LugarType = "Premium",
                DepartureDato = "2021-12-24",
                ReturnDato = "2021-12-30",
                AntallAdult = 2,
                AntallChild = 2,
                Pris = 1300
            };
            var billett3 = new Billett
            {
                Id = 3,
                KundeId = 3,
                RuteId = 3,
                DestinationFrom = "Oslo",
                DestinationTo = "Danmark",
                TicketType = "En vei",
                LugarType = "Premium",
                DepartureDato = "2022-01-01",
                ReturnDato = "2022-01-07",
                AntallAdult = 1,
                AntallChild = 2,
                Pris = 2500
            };

            var billettList = new List<Billett>();
            billettList.Add(billett1);
            billettList.Add(billett2);
            billettList.Add(billett3);

            mockBillettRepo.Setup(billetter => billetter.HentAlleBilletter()).ReturnsAsync(() => null);

            var billettController = new BillettController(mockBillettRepo.Object, mockLog.Object);

            mockHttpSession[_loggetInn] = _loggetInn;
            mockHttpContext.Setup(s => s.Session).Returns(mockHttpSession);
            billettController.ControllerContext.HttpContext = mockHttpContext.Object;

            // Act
            var resultat = await billettController.HentAlleBilletter();

            // Assert 
            Assert.Equal((int)HttpStatusCode.OK, (resultat.Result as ObjectResult)?.StatusCode);
            Assert.Null(resultat.Value);

        }


        [Fact]
        public async Task HentAlleBilletterIkkeLoggetInn()
        {
          
            // Arrange
            var billett1 = new Billett
            {
                Id = 1,
                KundeId = 1,
                RuteId = 1,
                DestinationFrom = "Oslo",
                DestinationTo = "Kiel",
                TicketType = "En vei",
                LugarType = "Standard",
                DepartureDato = "2022-02-22",
                ReturnDato = "2022-02-26",
                AntallAdult = 2,
                AntallChild = 0,
                Pris = 1300
            };
            var billett2 = new Billett
            {
                Id = 2,
                KundeId = 2,
                RuteId = 2,
                DestinationFrom = "Danmark",
                DestinationTo = "Stavanger",
                TicketType = "Retur",
                LugarType = "Premium",
                DepartureDato = "2021-12-24",
                ReturnDato = "2021-12-30",
                AntallAdult = 2,
                AntallChild = 2,
                Pris = 1300
            };
            var billett3 = new Billett
            {
                Id = 3,
                KundeId = 3,
                RuteId = 3,
                DestinationFrom = "Oslo",
                DestinationTo = "Danmark",
                TicketType = "En vei",
                LugarType = "Premium",
                DepartureDato = "2022-01-01",
                ReturnDato = "2022-01-07",
                AntallAdult = 1,
                AntallChild = 2,
                Pris = 2500
            };

            var billettList = new List<Billett>();
            billettList.Add(billett1);
            billettList.Add(billett2);
            billettList.Add(billett3);

            mockBillettRepo.Setup(billetter => billetter.HentAlleBilletter()).ReturnsAsync(billettList);

            var billettController = new BillettController(mockBillettRepo.Object, mockLog.Object);

            mockHttpSession[_loggetInn] = _ikkeLoggetInn;
            mockHttpContext.Setup(s => s.Session).Returns(mockHttpSession);
            billettController.ControllerContext.HttpContext = mockHttpContext.Object;

            // Act
            var resultat = await billettController.HentAlleBilletter();

            // Assert
            Assert.IsType<UnauthorizedResult>(resultat.Result);
        }

        [Fact]
        public async Task EndreBillettLoggetInnOK()
        {
            // Arrange

            mockBillettRepo.Setup(billett => billett.EndreBillett(It.IsAny<Billett>())).ReturnsAsync(true);

            var billettController = new BillettController(mockBillettRepo.Object, mockLog.Object);

            mockHttpSession[_loggetInn] = _loggetInn;
            mockHttpContext.Setup(s => s.Session).Returns(mockHttpSession);
            billettController.ControllerContext.HttpContext = mockHttpContext.Object;

            // Act
            var resultat = await billettController.EndreBillett(It.IsAny<Billett>());

            // Assert 

            Assert.IsType<OkResult>(resultat);
        }

        [Fact]
        public async Task EndreLoggetInnIkkeOK()
        {
            // Arrange

            mockBillettRepo.Setup(billett => billett.EndreBillett(It.IsAny<Billett>())).ReturnsAsync(false);

            var billettController = new BillettController(mockBillettRepo.Object, mockLog.Object);

            mockHttpSession[_loggetInn] = _loggetInn;
            mockHttpContext.Setup(s => s.Session).Returns(mockHttpSession);
            billettController.ControllerContext.HttpContext = mockHttpContext.Object;

            // Act
            var resultat = await billettController.EndreBillett(It.IsAny<Billett>()) as NotFoundObjectResult;

            // Assert 
            Assert.Equal((int)HttpStatusCode.NotFound, resultat.StatusCode);
            Assert.Equal("Kunne ikke endre billett", resultat.Value);
        }

        [Fact]
         public async Task EndreBillettLoggetInnFeilModel()
         {
             var billett1 = new Billett
             {
                 Id = 3,
                 KundeId = 3,
                 RuteId = 3,
                 DestinationFrom = "Oslo",
                 DestinationTo = "",
                 TicketType = "En vei",
                 LugarType = "Premium",
                 DepartureDato = "2022-01-01",
                 ReturnDato = "2022-01-07",
                 AntallAdult = 1,
                 AntallChild = 2,
                 Pris = 2500
             };

             mockBillettRepo.Setup(billett => billett.EndreBillett(billett1)).ReturnsAsync(false);

             var billettController = new BillettController(mockBillettRepo.Object, mockLog.Object);

            billettController.ModelState.AddModelError("DestinationTo", "Feil i inputvalidering på server");

             mockHttpSession[_loggetInn] = _loggetInn;
             mockHttpContext.Setup(s => s.Session).Returns(mockHttpSession);
            billettController.ControllerContext.HttpContext = mockHttpContext.Object;

             // Act
             var resultat = await billettController.EndreBillett(billett1) as BadRequestObjectResult;

             // Assert 
             Assert.Equal((int)HttpStatusCode.BadRequest, resultat.StatusCode);
             Assert.Equal("Feil i inputvalidering på server", resultat.Value);
         }

        [Fact]
        public async Task EndreBillettIkkeLoggetInn()
        {
            mockBillettRepo.Setup(billett => billett.EndreBillett(It.IsAny<Billett>())).ReturnsAsync(false);

            var billettController = new BillettController(mockBillettRepo.Object, mockLog.Object);

            mockHttpSession[_loggetInn] = _ikkeLoggetInn;
            mockHttpContext.Setup(s => s.Session).Returns(mockHttpSession);
            billettController.ControllerContext.HttpContext = mockHttpContext.Object;

            // Act
            var resultat = await billettController.EndreBillett(It.IsAny<Billett>());

            // Assert 

            Assert.IsType<UnauthorizedResult>(resultat);

        }

        [Fact]
        public async Task HentEnBillettLoggetInnOK()
        {
            // Arrange
            var billett1 = new Billett
            {
                Id = 3,
                KundeId = 3,
                RuteId = 3,
                DestinationFrom = "Oslo",
                DestinationTo = "",
                TicketType = "En vei",
                LugarType = "Premium",
                DepartureDato = "2022-01-01",
                ReturnDato = "2022-01-07",
                AntallAdult = 1,
                AntallChild = 2,
                Pris = 2500
            };

            mockBillettRepo.Setup(billett => billett.HentEnBillett(It.IsAny<int>())).ReturnsAsync(billett1);

            var billettController = new BillettController(mockBillettRepo.Object, mockLog.Object);

            mockHttpSession[_loggetInn] = _loggetInn;
            mockHttpContext.Setup(s => s.Session).Returns(mockHttpSession);
            billettController.ControllerContext.HttpContext = mockHttpContext.Object;

            // Act
            var resultat = await billettController.HentEnBillett(It.IsAny<int>());

            // Assert 
            Assert.Equal((int)HttpStatusCode.OK, (resultat.Result as ObjectResult)?.StatusCode);
        }

       [Fact]
        public async Task HentEnBillettLoggetInnIkkeOK()
        {
            // Arrange

            mockBillettRepo.Setup(billett => billett.HentEnBillett(It.IsAny<int>())).ReturnsAsync(() => null); // -> return no kunde

            var billettController = new BillettController(mockBillettRepo.Object, mockLog.Object);

            mockHttpSession[_loggetInn] = _loggetInn;
            mockHttpContext.Setup(s => s.Session).Returns(mockHttpSession);
            billettController.ControllerContext.HttpContext = mockHttpContext.Object;

            // Act
            var resultat = await billettController.HentEnBillett(It.IsAny<int>());

            // Assert 
            Assert.Equal((int)HttpStatusCode.NotFound, (resultat.Result as ObjectResult)?.StatusCode);
            Assert.Equal("Kunne ikke finne billett", (resultat.Result as ObjectResult)?.Value);
        }

        [Fact]
        public async Task HentEnBillettIkkeLoggetInn()
        {
            mockBillettRepo.Setup(billett => billett.HentEnBillett(It.IsAny<int>())).ReturnsAsync(() => null);

            var billettController = new BillettController(mockBillettRepo.Object, mockLog.Object);

            mockHttpSession[_loggetInn] = _ikkeLoggetInn;
            mockHttpContext.Setup(s => s.Session).Returns(mockHttpSession);
            billettController.ControllerContext.HttpContext = mockHttpContext.Object;

            // Act
            var resultat = await billettController.HentEnBillett(It.IsAny<int>());

            // Assert 
            Assert.IsType<UnauthorizedResult>(resultat.Result);
        }

        [Fact]
        public async Task SlettEnBillettLoggetInnOK()
        {
            // Arrange

            mockBillettRepo.Setup(billett => billett.SlettEnBillett(It.IsAny<int>())).ReturnsAsync(true);

            var billettController = new BillettController(mockBillettRepo.Object, mockLog.Object);

            mockHttpSession[_loggetInn] = _loggetInn;
            mockHttpContext.Setup(s => s.Session).Returns(mockHttpSession);
            billettController.ControllerContext.HttpContext = mockHttpContext.Object;

            // Act
            var resultat = await billettController.SlettEnBillett(It.IsAny<int>());

            // Assert 
            Assert.IsType<OkResult>(resultat);
        }

        [Fact]
        public async Task SlettLoggetInnIkkeOK()
        {
            // Arrange

            mockBillettRepo.Setup(billett => billett.SlettEnBillett(It.IsAny<int>())).ReturnsAsync(false);

            var billettController = new BillettController(mockBillettRepo.Object, mockLog.Object);

            mockHttpSession[_loggetInn] = _loggetInn;
            mockHttpContext.Setup(s => s.Session).Returns(mockHttpSession);
            billettController.ControllerContext.HttpContext = mockHttpContext.Object;

            // Act
            var resultat = await billettController.SlettEnBillett(It.IsAny<int>()) as NotFoundObjectResult;

            // Assert 
            Assert.Equal((int)HttpStatusCode.NotFound, resultat.StatusCode);
            Assert.Equal("Kunnde ikke slette billett", resultat.Value);
        }

        [Fact]
        public async Task SletteIkkeLoggetInn()
        {
           mockBillettRepo.Setup(billett => billett.SlettEnBillett(It.IsAny<int>())).ReturnsAsync(false);

            var billettController = new BillettController(mockBillettRepo.Object, mockLog.Object);

            mockHttpSession[_loggetInn] = _ikkeLoggetInn;
            mockHttpContext.Setup(s => s.Session).Returns(mockHttpSession);
            billettController.ControllerContext.HttpContext = mockHttpContext.Object;
            // Act
            var resultat = await billettController.SlettEnBillett(It.IsAny<int>());

            // Assert 
            Assert.IsType<UnauthorizedResult>(resultat);
        }

        [Fact]
        public async Task HentAlleDestinasjonerOK()
        {
            // Arrange
            var destinasjon1 = new Destinasjon
            {
                Id = 1,
                Sted = "Tokyo"   
            };

            var destinasjon2 = new Destinasjon
            {
                Id = 2,
                Sted = "Kyoto"
            };

            var destinasjon3 = new Destinasjon
            {
                Id = 3,
                Sted = "Okinawa"
            };

            var destinasjonList = new List<Destinasjon>();
            destinasjonList.Add(destinasjon1);
            destinasjonList.Add(destinasjon2);
            destinasjonList.Add(destinasjon3);

            mockBillettRepo.Setup(destinasjon => destinasjon.HentAlleDestinasjon()).ReturnsAsync(destinasjonList);

            var billettController = new BillettController(mockBillettRepo.Object, mockLog.Object);

            /*mockHttpSession[_loggetInn] = _loggetInn;
            mockHttpContext.Setup(s => s.Session).Returns(mockHttpSession);
            billettController.ControllerContext.HttpContext = mockHttpContext.Object;*/

            // Act
            var resultat = await billettController.HentAlleDestinasjon();

            // Assert 
            Assert.Equal((int)HttpStatusCode.OK, (resultat.Result as ObjectResult)?.StatusCode);
            Assert.Equal<List<Destinasjon>>((resultat.Result as ObjectResult)?.Value as List<Destinasjon>, destinasjonList);

        }

        [Fact]
        public async Task HentAlleDestinasjonerFeilDB()
        {
            // Arrange
            var destinasjon1 = new Destinasjon
            {
                Id = 1,
                Sted = "Tokyo"
            };

            var destinasjon2 = new Destinasjon
            {
                Id = 2,
                Sted = "Kyoto"
            };

            var destinasjon3 = new Destinasjon
            {
                Id = 3,
                Sted = "Okinawa"
            };

            var destinasjonList = new List<Destinasjon>();
            destinasjonList.Add(destinasjon1);
            destinasjonList.Add(destinasjon2);
            destinasjonList.Add(destinasjon3);

            mockBillettRepo.Setup(destinasjon => destinasjon.HentAlleDestinasjon()).ReturnsAsync(() => null);

            var billettController = new BillettController(mockBillettRepo.Object, mockLog.Object);

            // Act
            var resultat = await billettController.HentAlleDestinasjon();

            // Assert 
            Assert.Equal((int)HttpStatusCode.OK, (resultat.Result as ObjectResult)?.StatusCode);
            Assert.Null(resultat.Value);

        }
    }
}
