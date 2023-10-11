    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;



    namespace gestionticket_v2.Models
    {
        public class Ticket
        {
            public int Id { get; set; }
            public string Titre { get; set; }
            public string Description { get; set; }

            public int? PrioriteId { get; set; }
            public virtual Priorite Priorite { get; set; }

            public int CategorieId { get; set; }
            public virtual Categorie Categorie { get; set; }

            public string AuteurId { get; set; } // This is the ID of the user who created the ticket
            public virtual ApplicationUser Auteur { get; set; } // This should reference your ApplicationUser class

            public string AssigneeId { get; set; } // This is the ID of the technician assigned to the ticket
            public virtual ApplicationUser Assignee { get; set; } // This should also reference your ApplicationUser class

            public string Statut { get; set; }
            public DateTime DateCreation { get; set; }
            public DateTime DateModification { get; set; }

            public virtual ICollection<PieceJointe> PiecesJointes { get; set; }
        }

    }