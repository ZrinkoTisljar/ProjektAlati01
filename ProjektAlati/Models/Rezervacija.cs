using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace ProjektAlati.Models
{
    [Table("rezervacija")]
    public class Rezervacija
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("Alat")]
        public int AlatId { get; set; }

        [ForeignKey("Korisnik")]
        public int KorisnikId { get; set; }

        public DateTime DatumOd { get; set; }
        public DateTime? DatumDo { get; set; }
        public bool Vraceno { get; set; }

        public virtual Alat Alat { get; set; }
        public virtual Korisnik Korisnik { get; set; }
    }
}