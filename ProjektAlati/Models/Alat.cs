using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ProjektAlati.Models
{
    [Table("Alati")] // <<< prisiljava EF da koristi točno ovo ime
    public class Alat
    {
        public int Id { get; set; }
        
        public string Naziv { get; set; }
        public string Opis { get; set; }
        public bool Dostupan { get; set; }
    }
}