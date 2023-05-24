namespace gestionticket_v2.Models
{
    public class PieceJointe
    {
        public int Id { get; set; }
        public string Nom { get; set; }
        public string Type { get; set; }
        public byte[] Contenu { get; set; }
        public DateTime DateUpload { get; set; }

        public int TicketId { get; set; }
        public virtual Ticket Ticket { get; set; }
    }
}