using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

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

            var korisnici = db.Korisnici.ToList();
            return View(korisnici);
        }
    }
}