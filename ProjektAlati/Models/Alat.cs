using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace ProjektAlati.Models
{
    [Table("Alati")] // <<< prisiljava EF da koristi točno ovo ime
    public class Alat
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Naziv je obavezan.")]
        public string Naziv { get; set; }

        [Required(ErrorMessage = "Opis je obavezan.")]
        public string Opis { get; set; }
        [Required(ErrorMessage = "Morate označiti da je alat dostupan.")]
        public bool Dostupan { get; set; }
    }
}