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

        public DbSet<Alat> Alati { get; set; }

        public DbSet<Korisnik> Korisnici { get; set; }

        public DbSet<Rezervacija> Rezervacije { get; set; }


    }
}