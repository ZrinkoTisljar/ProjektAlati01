using Microsoft.VisualStudio.TestTools.UnitTesting;
using ProjektAlati.Controllers;
using ProjektAlati.Models;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Moq;
using System;

namespace ProjektAlati.Tests
{
    [TestClass]
    public class AlatControllerTest
    {
/*        Test provjerava da metoda Index(string search) u AlatControlleru:

ispravno filtrira alate prema unesenom tekstu za pretragu(search)

vraća točno one alate koji sadrže traženi pojam u nazivu ili opisu

ne vraća alate koji ne odgovaraju uvjetu pretrage


*/
        [TestMethod] 
        public void Index_SearchByName_ReturnsFilteredResults()
        {
            // 1. Podaci za testiranje
            // Priprema podataka (simulirana baza):
            var alatiData = new List<Alat>
            {
                new Alat { Id = 1, Naziv = "Bušilica", Opis = "Makita" },
                new Alat { Id = 2, Naziv = "Čekić", Opis = "Za beton" }
            }.AsQueryable();

            var posudbeData = new List<Posudba>
            {
                new Posudba
                {
                    Id = 1,
                    AlatId = 1,
                    DatumPosudbe = DateTime.Now.AddDays(-2),
                    DatumPovratka = DateTime.Now.AddDays(2)
                }
            }.AsQueryable();

            var rezervacijeData = new List<Rezervacija>().AsQueryable();

            // 2. Mock DbSet<Alat> Umjesto stvarne baze, koristi se Moq da bi controller.Index() mislio da čita iz baze.
            var mockAlati = new Mock<DbSet<Alat>>();
            mockAlati.As<IQueryable<Alat>>().Setup(m => m.Provider).Returns(alatiData.Provider);
            mockAlati.As<IQueryable<Alat>>().Setup(m => m.Expression).Returns(alatiData.Expression);
            mockAlati.As<IQueryable<Alat>>().Setup(m => m.ElementType).Returns(alatiData.ElementType);
            mockAlati.As<IQueryable<Alat>>().Setup(m => m.GetEnumerator()).Returns(alatiData.GetEnumerator());

            // 3. Mock DbSet<Posudba>
            var mockPosudbe = new Mock<DbSet<Posudba>>();
            mockPosudbe.As<IQueryable<Posudba>>().Setup(m => m.Provider).Returns(posudbeData.Provider);
            mockPosudbe.As<IQueryable<Posudba>>().Setup(m => m.Expression).Returns(posudbeData.Expression);
            mockPosudbe.As<IQueryable<Posudba>>().Setup(m => m.ElementType).Returns(posudbeData.ElementType);
            mockPosudbe.As<IQueryable<Posudba>>().Setup(m => m.GetEnumerator()).Returns(posudbeData.GetEnumerator());

            // 4. Mock DbSet<Rezervacija>
            var mockRezervacije = new Mock<DbSet<Rezervacija>>();
            mockRezervacije.As<IQueryable<Rezervacija>>().Setup(m => m.Provider).Returns(rezervacijeData.Provider);
            mockRezervacije.As<IQueryable<Rezervacija>>().Setup(m => m.Expression).Returns(rezervacijeData.Expression);
            mockRezervacije.As<IQueryable<Rezervacija>>().Setup(m => m.ElementType).Returns(rezervacijeData.ElementType);
            mockRezervacije.As<IQueryable<Rezervacija>>().Setup(m => m.GetEnumerator()).Returns(rezervacijeData.GetEnumerator());

            // 5. Mock DbContext
            var mockContext = new Mock<ProjektAlatiContext>();
            mockContext.Setup(c => c.Alati).Returns(mockAlati.Object);
            mockContext.Setup(c => c.Posudbe).Returns(mockPosudbe.Object);
            mockContext.Setup(c => c.Rezervacije).Returns(mockRezervacije.Object);

            // 6. Mock Session, Ovdje Session vraća null, jer nije bitno za pretragu — bitno je da ne baci grešku.
            var sessionMock = new Mock<HttpSessionStateBase>();
            sessionMock.Setup(s => s["KorisnikId"]).Returns(null); // korisnik nije prijavljen

            var contextMock = new Mock<HttpContextBase>();
            contextMock.Setup(c => c.Session).Returns(sessionMock.Object);

            // 7. Inicijalizacija kontrolera
            var controller = new AlatController(mockContext.Object)
            {
                ControllerContext = new ControllerContext
                {
                    HttpContext = contextMock.Object
                }
            };

            // 8. Poziv metode sa "search" parametrom, simulira se kao da korisnik traži riječ "Buš". 
            var result = controller.Index("Buš") as ViewResult;
            var model = result.Model as List<Alat>;

            // 9. Assert provjere
            Assert.IsNotNull(result);
            Assert.IsNotNull(model);

            //Ako pretraga radi kako treba, rezultat mora biti lista s jednim alatom: "Bušilica"
            Assert.AreEqual(1, model.Count);
            Assert.AreEqual("Bušilica", model[0].Naziv);
        }
    }
}
