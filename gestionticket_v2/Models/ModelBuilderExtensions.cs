namespace gestionticket_v2.Models
{
    using Microsoft.EntityFrameworkCore;
    using gestionticket_v2.Models;

    public static class ModelBuilderExtensions
    {
        public static void Seed(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Categorie>().HasData(
                new Categorie { Id = 1, Nom = "Technical Issue" },
                new Categorie { Id = 2, Nom = "Billing Inquiry" },
                new Categorie { Id = 3, Nom = "Sales Question" },
                new Categorie { Id = 4, Nom = "Other" },
                new Categorie { Id = 5, Nom = "Feature Request" },
                new Categorie { Id = 6, Nom = "Bug Report" },
                new Categorie { Id = 7, Nom = "Account Change" },
                new Categorie { Id = 8, Nom = "Account Termination" },
                new Categorie { Id = 9, Nom = "Account Creation" },
                new Categorie { Id = 10, Nom = "Account Billing" },
                new Categorie { Id = 11, Nom = "Account Login" }
            // Add more categories here
            );

            modelBuilder.Entity<Priorite>().HasData(
                new Priorite { Id = 1, Nom = "Low" },
                new Priorite { Id = 2, Nom = "Medium" },
                new Priorite { Id = 3, Nom = "High" }
            // Add more priorities here
            );
        }
    }

}
