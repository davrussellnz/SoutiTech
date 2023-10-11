using Microsoft.AspNetCore.Identity;



namespace gestionticket_v2.Models
{



    public class MembreSupportTechnique
    {
        public MembreSupportTechnique()
        {
            TicketsAssignes = new List<Ticket>();
        }
        public string Id { get; set; }
        public string Nom { get; set; }
        public string Prenom { get; set; }
        public virtual ICollection<Ticket> TicketsAssignes { get; set; }
    }
}