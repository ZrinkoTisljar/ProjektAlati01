using ProjektAlati.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ProjektAlati.Controllers
{
    public class AlatController : Controller
    {
        private ProjektAlatiContext db = new ProjektAlatiContext();

        // GET: Alat
        public ActionResult Index()
        {
            /*var alati = db.Alati.ToList(); // EF će ovdje automatski stvoriti tablicu ako ne postoji


            // Ako je korisnik prijavljen — dohvati njegove rezervacije
            if (Session["KorisnikId"] != null)
            {
                int korisnikId = Convert.ToInt32(Session["KorisnikId"]);
                var aktivne = db.Rezervacije
                                .Where(r => r.KorisnikId == korisnikId && !r.Vraceno)
                                .ToList();

                // Pošalji aktivne rezervacije u ViewBag
                ViewBag.MojeRezervacije = aktivne;
            }

            return View(alati);*/



            if (Session["KorisnikId"] != null)
            {
                int korisnikId = Convert.ToInt32(Session["KorisnikId"]);
                var mojePosudbe = db.Posudbe
                                    .Where(p => p.KorisnikId == korisnikId && p.DatumPovratka >= DateTime.Now)
                                    .ToList();

                ViewBag.MojePosudbe = mojePosudbe;
            }
            /*var alati = db.Alati.ToList();*/
            var alati = db.Alati.OrderByDescending(a => a.Id).ToList();

            // Aktivne rezervacije korisnika
            if (Session["KorisnikId"] != null)
            {
                int korisnikId = Convert.ToInt32(Session["KorisnikId"]);
                var aktivne = db.Rezervacije
                                .Where(r => r.KorisnikId == korisnikId && !r.Vraceno)
                               
                                .ToList();
                ViewBag.MojeRezervacije = aktivne;
            }

            // Aktivne posudbe (Vraceno == false)
            /* var aktivnePosudbe = db.Rezervacije
                 .Where(r => !r.Vraceno)
                 .GroupBy(r => r.AlatId)
                 .Select(g => g.OrderByDescending(r => r.DatumOd).FirstOrDefault())
                 .ToDictionary(r => r.AlatId, r => r);
 */
                var aktivnePosudbe = db.Posudbe
               .Where(p => p.DatumPovratka >= DateTime.Now)
               .GroupBy(p => p.AlatId)
               .Select(g => g.OrderByDescending(p => p.DatumPosudbe).FirstOrDefault())
               .ToDictionary(p => p.AlatId, p => p);

            ViewBag.AktivnePosudbe = aktivnePosudbe;


            ViewBag.AktivnePosudbe = aktivnePosudbe;

            return View(alati);
        }

        // GET: Alat/Create
        public ActionResult Create()
        {

            // da samo admin moze kreirati
            if (Session["Uloga"]?.ToString() != "admin")
                return new HttpUnauthorizedResult();

            return View();
        }

        // POST: Alat/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Alat alat)
        {
            //admin
            if (Session["Uloga"]?.ToString() != "admin")
                return new HttpUnauthorizedResult();

            if (ModelState.IsValid)
            {
                db.Alati.Add(alat);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(alat);
        }

        // GET: Alat/Edit/5
        public ActionResult Edit(int? id)
        {
            //samo admin moze editati
            if (Session["Uloga"]?.ToString() != "admin")
                return new HttpUnauthorizedResult();
            //
            if (id == null)
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);

            Alat alat = db.Alati.Find(id);
            if (alat == null)
                return HttpNotFound();

            //
            // Provjera: je li alat posuđen ili rezerviran
            /* bool imaPosudbu = db.Posudbe.Any(p => p.AlatId == id);
             bool imaRezervaciju = db.Rezervacije.Any(r => r.AlatId == id);*/

            /*bool imaPosudbu = db.Posudbe.Any(p => p.AlatId == id && p.DatumPovratka == null);*/

            bool imaPosudbu = !alat.Dostupan;
            bool imaRezervaciju = db.Rezervacije.Any(r => r.AlatId == id && !r.Vraceno);


            if (imaPosudbu || imaRezervaciju)
            {
                TempData["Greska"] = "Alat je trenutno posuđen i ne može se uređivati.";
                return RedirectToAction("Index");
            }
            //
            return View(alat);
        }

        // POST: Alat/Edit/5
        //Uređivanje postojećeg alata
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Alat alat)
        {

            //samo za admina
            if (Session["Uloga"]?.ToString() != "admin")
                return new HttpUnauthorizedResult();
            //


            //
         

           


            //

            if (ModelState.IsValid)
            {
                db.Entry(alat).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(alat);
        }

        // GET: Alat/Delete/5
        public ActionResult Delete(int? id)
        {


            //admin
            if (Session["Uloga"]?.ToString() != "admin")
                return new HttpUnauthorizedResult();

            if (id == null)
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);

            Alat alat = db.Alati.Find(id);
            if (alat == null)
                return HttpNotFound();

            return View(alat);
        }

        // POST: Alat/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            if (Session["Uloga"]?.ToString() != "admin")
                return new HttpUnauthorizedResult();

            Alat alat = db.Alati.Find(id);

            // Provjera: je li alat povezan s posudbama ili rezervacijama
            /*  bool imaPosudbu = db.Posudbe.Any(p => p.AlatId == id);
              bool imaRezervaciju = db.Rezervacije.Any(r => r.AlatId == id);*/

            /*bool imaPosudbu = db.Posudbe.Any(p => p.AlatId == id && p.DatumPovratka == null);*/
            bool imaPosudbu = !alat.Dostupan;
            bool imaRezervaciju = db.Rezervacije.Any(r => r.AlatId == id && !r.Vraceno);


            if (imaPosudbu || imaRezervaciju)
            {
                TempData["Greska"] = "Alat je trenutno posuđen i ne može se obrisati.";
                return RedirectToAction("Index");
            }



            db.Alati.Remove(alat);
            db.SaveChanges();
            return RedirectToAction("Index");
        }


        //metoda za posudbu
        public ActionResult Posudi(int id)
        {

            if (Session["Uloga"]?.ToString() == "admin")
                return new HttpUnauthorizedResult(); // ili RedirectToAction("Index", "Home");

            if (Session["KorisnikId"] == null)
                return RedirectToAction("Login", "Korisnik");

            var alat = db.Alati.Find(id);

            if (alat == null || !alat.Dostupan)
            {
                TempData["Poruka"] = "Alat trenutno nije dostupan za posudbu.";
                return RedirectToAction("Index");
            }

            Rezervacija r = new Rezervacija
            {
                AlatId = id,
                KorisnikId = Convert.ToInt32(Session["KorisnikId"]),
                DatumOd = DateTime.Now,
                Vraceno = false
            };

            alat.Dostupan = false;

            db.Rezervacije.Add(r);
            db.SaveChanges();

            TempData["Poruka"] = "Uspješno ste posudili alat.";
            return RedirectToAction("Index");
        }




        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}