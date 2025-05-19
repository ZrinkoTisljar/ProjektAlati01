using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProjektAlati.Models
{

    [Table("korisnik")] // <<< PRISILJAVA naziv tablice
    public class Korisnik
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string KorisnickoIme { get; set; }

        [Required]
        public string Lozinka { get; set; }

        public string Uloga { get; set; } // "admin" ili "korisnik"
    }
}