using Microsoft.AspNetCore.Identity;
using System.Net.Sockets;

namespace gestionticket_v2.Models
{
    public class Client 
    {
        public Client()
        {
            Tickets = new List<Ticket>();
        }
        public string Id { get; set; }
        public string Nom { get; set; }
        public string Prenom { get; set; }

        // Cette liste de tickets est maintenant optionnelle
        public virtual ICollection<Ticket> Tickets { get; set; }
    }

}
