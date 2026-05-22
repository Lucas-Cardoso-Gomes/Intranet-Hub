using Microsoft.EntityFrameworkCore;
using IntranetHub.Models;

namespace IntranetHub.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Branch> Branches { get; set; }
        public DbSet<Aviso> Avisos { get; set; }
        public DbSet<Aniversario> Aniversarios { get; set; }
        public DbSet<OuvidoriaMessage> OuvidoriaMessages { get; set; }
        public DbSet<Pop> Pops { get; set; }
        public DbSet<CincoSAudit> CincoSAudits { get; set; }
        public DbSet<CincoSImage> CincoSImages { get; set; }
        public DbSet<Contact> Contacts { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Branch>().HasData(
                new Branch { Id = 1, Name = "Uruguaiana", Acronym = "UGN" },
                new Branch { Id = 2, Name = "São Borja", Acronym = "SBJ" },
                new Branch { Id = 3, Name = "Itajaí", Acronym = "ITJ" },
                new Branch { Id = 4, Name = "Foz do Iguaçu", Acronym = "FOZ" },
                new Branch { Id = 5, Name = "São Paulo", Acronym = "SPO" }
            );

            modelBuilder.Entity<User>().HasData(
                new User { Id = 1, Username = "admin", Password = "123", IsAdmin = true }
            );
        }
    }
}
