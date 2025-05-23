using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProjektAlati.Models
{
    [Table("Posudbe")] // <<< PRISILJAVA naziv tablice
    public class Posudba
    {
        public int Id { get; set; }

        [ForeignKey("Alat")]
        public int AlatId { get; set; }
        public virtual Alat Alat { get; set; }

        [ForeignKey("Korisnik")]
        public int KorisnikId { get; set; }
        public virtual Korisnik Korisnik { get; set; }

        public DateTime DatumPosudbe { get; set; }
        public DateTime? DatumPovratka { get; set; }
    }
}