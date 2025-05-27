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

        [Required(ErrorMessage = "Korisničko ime je obavezno.")]
        [Display(Name = "Korisničko ime")]
        public string KorisnickoIme { get; set; }

        [Required(ErrorMessage = "Lozinka je obavezna.")]
        public string Lozinka { get; set; }

        [Required(ErrorMessage = "Ime je obavezno.")]
        public string Ime { get; set; }

        [Required(ErrorMessage = "Prezime je obavezno.")]
        public string Prezime { get; set; }

        [Required(ErrorMessage = "Adresa je obavezna.")]
        public string Adresa { get; set; }

        public string Uloga { get; set; } // "admin" ili "korisnik"
    }
}