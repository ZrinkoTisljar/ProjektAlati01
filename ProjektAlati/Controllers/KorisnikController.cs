using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using ProjektAlati.Models;


using System.Web.Security;

namespace ProjektAlati.Controllers
{
    public class KorisnikController : Controller
    {
        private ProjektAlatiContext db = new ProjektAlatiContext();

        // GET: /Korisnik/Register
        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Register(Korisnik korisnik)
        {
            if (ModelState.IsValid)
            {
                korisnik.Uloga = "korisnik";
                db.Korisnici.Add(korisnik);
                db.SaveChanges();

                TempData["Poruka"] = "Registracija uspješna! Možete se prijaviti.";
                return RedirectToAction("Login");
            }
            return View(korisnik);
        }

        // GET: /Korisnik/Login
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(string korisnickoIme, string lozinka)
        {
            var korisnik = db.Korisnici.FirstOrDefault(k => k.KorisnickoIme == korisnickoIme && k.Lozinka == lozinka);
            if (korisnik != null)
            {
                Session["KorisnikId"] = korisnik.Id;
                Session["KorisnickoIme"] = korisnik.KorisnickoIme;
                Session["Uloga"] = korisnik.Uloga;
                return RedirectToAction("Index", "Alat");
            }
            ViewBag.Poruka = "Neispravno korisničko ime ili lozinka.";
            return View();
        }

        public ActionResult Logout()
        {
            Session.Clear();
            return RedirectToAction("Login");
        }
    }
}
