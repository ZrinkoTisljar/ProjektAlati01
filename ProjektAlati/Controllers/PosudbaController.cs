using System;
using System.Linq;
using System.Web.Mvc;
using ProjektAlati.Models;

namespace ProjektAlati.Controllers
{
    public class PosudbaController : Controller
    {
        private ProjektAlatiContext db = new ProjektAlatiContext();

        // GET: Posudba
        public ActionResult Index()
        {
            var posudbe = db.Posudbe.Include("Alat").Include("Korisnik").ToList();
            return View(posudbe);
        }

        // GET: Posudba/Create
        public ActionResult Create(int? id) ////  ili sa upitnikom ako se rusi aplikacija ->public ActionResult Create(int? id) dopušta da id bude null 
        {
            var alat = db.Alati.Find(id);
            if (alat == null || !alat.Dostupan)
            {
                TempData["Greska"] = "Alat nije dostupan.";
                return RedirectToAction("Index", "Alat");
            }

            ViewBag.Alat = alat;
            return View();
        }

        // POST: Posudba/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(int AlatId, DateTime DatumPovratka)
        {
            if (Session["KorisnikId"] == null)
                return RedirectToAction("Login", "Korisnik");

            var alat = db.Alati.Find(AlatId);
            if (alat == null || !alat.Dostupan)
            {
                TempData["Greska"] = "Alat nije dostupan.";
                return RedirectToAction("Index", "Alat");
            }

            //  VALIDACIJA DATUMA POVRATKA
            if (DatumPovratka <= DateTime.Now.Date)
            {
                TempData["Greska"] = "Datum povratka mora biti u budućnosti.";
                return RedirectToAction("Create", new { id = AlatId });
            }

            var posudba = new Posudba
            {
                AlatId = AlatId,
                KorisnikId = Convert.ToInt32(Session["KorisnikId"]),
                DatumPosudbe = DateTime.Now,
                DatumPovratka = DatumPovratka
            };

            alat.Dostupan = false;
            db.Posudbe.Add(posudba);
            db.SaveChanges();

            TempData["Poruka"] = "Uspješno ste posudili alat.";
            return RedirectToAction("Index", "Alat");
        }

        public ActionResult Vrati(int id)
        {
            var posudba = db.Posudbe.FirstOrDefault(p => p.Id == id && !p.Vraceno);

            if (posudba == null)
            {
                TempData["Greska"] = "Posudba nije pronađena ili je već vraćena.";
                return RedirectToAction("Index", "Alat");
            }

            // Osvježi status alata
            var alat = db.Alati.Find(posudba.AlatId);
            if (alat != null)
                alat.Dostupan = true;

            // Ažuriraj povrat
            posudba.DatumPovratka = DateTime.Now;
            posudba.Vraceno = true;

            db.SaveChanges();

            TempData["Poruka"] = "Alat je uspješno vraćen.";
            return RedirectToAction("Index", "Alat");
        }

        public ActionResult MojaEvidencija()
        {
            if (Session["KorisnikId"] == null)
                return RedirectToAction("Login", "Korisnik");

            int korisnikId = Convert.ToInt32(Session["KorisnikId"]);

            var posudbe = db.Posudbe
                            .Include("Alat")
                            .Where(p => p.KorisnikId == korisnikId)
                            .OrderByDescending(p => p.DatumPosudbe)
                            .ToList();

            return View(posudbe);
        }

    }
}
