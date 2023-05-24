using Microsoft.AspNetCore.Identity;

namespace gestionticket_v2.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string? Nom { get; set; }
        public string? Prenom { get; set; }
        public string? UserType { get; set; }
    }


}
