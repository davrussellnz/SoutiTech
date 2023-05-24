using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace gestionticket_v2.Models
{
    public class Ticket
    {
        public int Id { get; set; }

        [Required]
        public string Titre { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        public int? PrioriteId { get; set; }
        public virtual Priorite Priorite { get; set; }

        [Required]
        public int CategorieId { get; set; }
        public virtual Categorie Categorie { get; set; }

        public string AuteurId { get; set; }
        public virtual Client Auteur { get; set; }

        public string AssigneeId { get; set; }
        public string Statut { get; set; }
        public DateTime DateCreation { get; set; }
        public DateTime DateModification { get; set; }
        public virtual MembreSupportTechnique Assignee { get; set; }

        public virtual ICollection<PieceJointe> PiecesJointes { get; set; }
    }
}
