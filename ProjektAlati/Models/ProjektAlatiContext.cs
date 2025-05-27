using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;

namespace ProjektAlati.Models
{
    public class ProjektAlatiContext : DbContext
    {
        public ProjektAlatiContext() : base("name=ProjektAlatiConnection") { }

        public virtual DbSet<Alat> Alati { get; set; }  //Moq može mockirati samo virtual metode/propertije.

        public virtual DbSet<Korisnik> Korisnici { get; set; }

        public virtual DbSet<Rezervacija> Rezervacije { get; set; }

        public virtual DbSet<Posudba> Posudbe { get; set; }

    }
}