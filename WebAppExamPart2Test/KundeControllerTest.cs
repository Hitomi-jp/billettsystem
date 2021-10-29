using Moq;
using System;
using System.Threading.Tasks;
using WebAppExamPart2.Controllers;
using WebAppExamPart2.DAL;
using WebAppExamPart2.Models;
using Xunit;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;

namespace WebAppExamPart2Test
{
    public class KundeControllerTest
    {
        private readonly Mock<ILogger<KundeController>> mockLog = new Mock<ILogger<KundeController>>();

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

            var mock = new Mock<IKundeRepository>();
            mock.Setup(k => k.LagreKunde(innKunde)).ReturnsAsync(innKunde.Id);
            var kundeController = new KundeController(mock.Object, mockLog.Object);
            var resultat = await kundeController.LagreKunde(innKunde);

            Assert.Equal(innKunde.Id,resultat.Value);
        }
    }
}
