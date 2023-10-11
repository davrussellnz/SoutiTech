using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using gestionticket_v2.Models;

namespace gestionticket_v2.Data
{
    public class gestionticket_v2Context : IdentityDbContext<ApplicationUser>
    {
        public gestionticket_v2Context(DbContextOptions<gestionticket_v2Context> options)
            : base(options)
        {
        }

        public DbSet<Ticket> Tickets { get; set; }
        public DbSet<Categorie> Categorie { get; set; }
        public DbSet<Priorite> Priorite { get; set; }
        public DbSet<Statistiques> Statistiques { get; set; }
        //Clients
        public DbSet<Client> Client { get; set; }

        //Membres du support technique

        public DbSet<MembreSupportTechnique> MembreSupportTechnique { get; set; }




        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            
            base.OnModelCreating(modelBuilder); // This is needed to correctly set up the identity model
           

            modelBuilder.Entity<Ticket>()
                .HasOne(t => t.Assignee)
                .WithMany(m => m.TicketsAssignes)
                .HasForeignKey(t => t.AssigneeId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Ticket>()
                .HasOne(t => t.Auteur)
                .WithMany(c => c.Tickets)
                .HasForeignKey(t => t.AuteurId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Ticket>()
                .HasOne(t => t.Categorie)
                .WithMany(c => c.Tickets)
                .HasForeignKey(t => t.CategorieId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Ticket>()
                .HasOne(t => t.Priorite)
                .WithMany(p => p.Tickets)
                .HasForeignKey(t => t.PrioriteId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
