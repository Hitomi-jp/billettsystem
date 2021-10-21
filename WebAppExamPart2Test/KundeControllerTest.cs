using Moq;
using System;
using System.Threading.Tasks;
using WebAppExamPart2.Controllers;
using WebAppExamPart2.DAL;
using WebAppExamPart2.Models;
using Xunit;
using System.Collections.Generic;

namespace WebAppExamPart2Test
{
    public class KundeControllerTest
    {
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
            Console.WriteLine(innKunde.Id);

            var mock = new Mock<IKundeRepository>();
            mock.Setup(k => k.Lagre(innKunde)).ReturnsAsync(innKunde.Id);
            var kundeController = new KundeController(mock.Object);
            var resultat = await kundeController.Lagre(innKunde);

            Assert.Equal(innKunde.Id,resultat.Value);
        }
    }
}
