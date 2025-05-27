using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using ProjektAlati.Models;

namespace ProjektAlati.Controllers
{
    public class AdminController : Controller
    {
        private ProjektAlatiContext db = new ProjektAlatiContext();

        // GET: Admin
        public ActionResult Index()
        {
            // zaštita: samo admin
            if (Session["Uloga"] == null || Session["Uloga"].ToString() != "admin")
                return RedirectToAction("Login", "Korisnik");

            var korisnici = db.Korisnici
           .Where(k => k.Uloga != "admin")
           .ToList();


            var aktivnePosudbe = db.Posudbe
           .Where(p => !p.Vraceno)
           .Include(p => p.Alat)
           .ToList();

            ViewBag.AktivnePosudbe = aktivnePosudbe;

            return View(korisnici);
        }

        public ActionResult PosudbeKorisnika()
        {
            var posudbe = db.Posudbe
                            .Where(p => !p.Vraceno)
                            .Include("Korisnik")
                            .Include("Alat")
                            .ToList();

            return View(posudbe);
        }
    }
}