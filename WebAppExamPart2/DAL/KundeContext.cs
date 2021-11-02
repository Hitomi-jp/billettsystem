using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using WebAppExamPart2.Models;

namespace WebAppExamPart2.DAL
{
    [ExcludeFromCodeCoverage]
    public class Kunder
    {
        public int Id { get; set; }
        public string Fornavn { get; set; }
        public string Etternavn { get; set; }
        public string Telfonnr { get; set; }
        public string Epost { get; set; }
        public string Adresse { get; set; }
        virtual public Billett Billetter { get; set; }
        virtual public PostSteder PostSteder { get; set; }
        virtual public Kreditt Kreditt { get; set; }
    }
    public class Brukere
    {
        public int Id { get; set; }
        public string Brukernavn { get; set; }
        public byte[] Passord { get; set; }
        public byte[] Salt { get; set; }
    }

    public class KundeContext : DbContext
    {
       
        public KundeContext(DbContextOptions<KundeContext> options) : base(options)
        {
            Database.EnsureCreated();
        }

        public DbSet<Kunder> Kunder { get; set; }
        public DbSet<PostSteder> PostSteder { get; set; }
        public DbSet<Billett> Billetter { get; set; }
        public DbSet<Kreditt> Kreditt { get; set; }
        public DbSet<Destinasjon> Destinasjoner { get; set; }
        public DbSet<Brukere> Brukere { get; set; }
        public DbSet<Rute> Ruter { get; set; }

        public DbSet<Strekning> Strekninger { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
         optionsBuilder.UseLazyLoadingProxies();
        }

    }
}
