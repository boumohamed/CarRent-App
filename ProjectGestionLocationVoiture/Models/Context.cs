using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace ProjectGestionLocationVoiture.Models
{
    public class Context : DbContext
    {
        public Context() : base("name=LVCS")
        {

        }

        public DbSet<Voiture> Voitures { get; set; }
        public DbSet<Modele> Modeles { get; set; }
        public DbSet<Client> Clients { get; set; }
        public DbSet<Reservation> Reservations { get; set; }
        public DbSet<Admin> Admins { get; set; }
        public DbSet<User> Users { get; set; }
    }

    
}