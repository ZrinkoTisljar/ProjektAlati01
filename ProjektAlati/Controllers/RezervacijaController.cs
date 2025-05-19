using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ProjektAlati.Models;
using System.Data.Entity;


namespace ProjektAlati.Controllers
{
    public class RezervacijaController : Controller
    {
        private ProjektAlatiContext db = new ProjektAlatiContext();

        // GET: Rezervacija
        public ActionResult Index()
        {
            if (Session["Uloga"]?.ToString() != "admin")
            {
                int korisnikId = Convert.ToInt32(Session["KorisnikId"]);
                var moje = db.Rezervacije

                    .Include(r => r.Alat) // <-- ovo dodaje alat u rezultat
                             .Where(r => r.KorisnikId == korisnikId)
                             .ToList();
                return View(moje);
            }

            var sve = db.Rezervacije.ToList();
            return View(sve);
        }

        // GET: Rezervacija/Create
        public ActionResult Create(int? alatId)
        {

            if (Session["KorisnikId"] == null)
                return RedirectToAction("Login", "Korisnik");

            ViewBag.AlatId = new SelectList(db.Alati, "Id", "Naziv", alatId);
            return View();
        }

        // POST: Rezervacija/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Rezervacija rezervacija)
        {

            if (Session["KorisnikId"] == null)
                return RedirectToAction("Login", "Korisnik");

            // 
            rezervacija.KorisnikId = Convert.ToInt32(Session["KorisnikId"]);
            rezervacija.Vraceno = false;
            rezervacija.DatumOd = DateTime.Now;

            db.Rezervacije.Add(rezervacija);
            db.SaveChanges();

            return RedirectToAction("Index");


            /* if (ModelState.IsValid)
             {
                 rezervacija.KorisnikId = Convert.ToInt32(Session["KorisnikId"]);
                 rezervacija.Vraceno = false;
                 rezervacija.DatumOd = DateTime.Now;

                 db.Rezervacije.Add(rezervacija);
                 db.SaveChanges();
                 return RedirectToAction("Index");
             }*/

           /* ViewBag.AlatId = new SelectList(db.Alati, "Id", "Naziv", rezervacija.AlatId);
            return View(rezervacija);*/
        }


    


        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }


        public ActionResult Vrati(int id)
        {
            var rezervacija = db.Rezervacije.FirstOrDefault(r => r.Id == id && !r.Vraceno);

            if (rezervacija == null)
                return HttpNotFound();

            // Osvježi status
            rezervacija.Vraceno = true;
            rezervacija.DatumDo = DateTime.Now;

            // Alat ponovno dostupan
            var alat = db.Alati.Find(rezervacija.AlatId);
            if (alat != null)
                alat.Dostupan = true;

            db.SaveChanges();

            TempData["Poruka"] = "Alat je uspješno vraćen.";
            return RedirectToAction("Index");
        }
    }
}