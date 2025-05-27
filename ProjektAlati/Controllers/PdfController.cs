using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.IO;
using System.Web.Mvc;
using iTextSharp.text;
using iTextSharp.text.pdf;
using ProjektAlati.Models;
using System.Data.Entity;

namespace ProjektAlati.Controllers
{
    public class PdfController : Controller
    {
        private ProjektAlatiContext db = new ProjektAlatiContext();

        public FileStreamResult PotvrdaPosudbe(int id)
        {
            var posudba = db.Posudbe
                .Include("Korisnik")
                .Include("Alat")
                .FirstOrDefault(p => p.Id == id);

            if (posudba == null)
                return null;

            MemoryStream memorija = new MemoryStream();
            Document dokument = new Document(PageSize.A4, 50, 50, 25, 25);
            PdfWriter.GetInstance(dokument, memorija).CloseStream = false;

            dokument.Open();

            var font = FontFactory.GetFont(FontFactory.HELVETICA, 12, BaseColor.BLACK);
            dokument.Add(new Paragraph("Potvrda o posudbi alata", new Font(Font.FontFamily.HELVETICA, 16, Font.BOLD)));
            dokument.Add(new Paragraph(" "));

            dokument.Add(new Paragraph($"Korisnik: {posudba.Korisnik.Ime} {posudba.Korisnik.Prezime}", font));
            dokument.Add(new Paragraph($"Alat: {posudba.Alat.Naziv}", font));
            dokument.Add(new Paragraph($"Opis: {posudba.Alat.Opis}", font));
            dokument.Add(new Paragraph($"Datum posudbe: {posudba.DatumPosudbe:dd.MM.yyyy.}", font));
            dokument.Add(new Paragraph($"Datum povratka: {posudba.DatumPovratka:dd.MM.yyyy.}", font));
            dokument.Add(new Paragraph(" "));
            dokument.Add(new Paragraph("Zahvaljujemo na korištenju sustava.", font));

            dokument.Close();

            byte[] bytes = memorija.ToArray();
            memorija.Write(bytes, 0, bytes.Length);
            memorija.Position = 0;

            return new FileStreamResult(memorija, "application/pdf")
            {
                FileDownloadName = $"Potvrda_{posudba.Id}.pdf"
            };
        }
    }
}