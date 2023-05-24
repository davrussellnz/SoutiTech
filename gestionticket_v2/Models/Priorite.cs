namespace gestionticket_v2.Models
{
    public class Priorite
    {
        public int Id { get; set; }
        public string Nom { get; set; }
        public int Niveau { get; set; }

        public virtual ICollection<Ticket> Tickets { get; set; }

    }

}