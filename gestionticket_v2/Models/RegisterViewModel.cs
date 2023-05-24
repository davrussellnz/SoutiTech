using System.ComponentModel.DataAnnotations;

public class RegisterViewModel
{
    [Required]
    public string Nom { get; set; }

    [Required]
    public string Prenom { get; set; }

    [Required]
    [EmailAddress]
    public string Email { get; set; }

    [Required]
    [DataType(DataType.Password)]
    public string Password { get; set; }

    [Required]
    public string UserType { get; set; } // Can be "Client" or "MembreSupportTechnique"
}
