using System;
using System.ComponentModel.DataAnnotations;

namespace SenicaWeb.Models.Auth;

public class RegisterInput
{
    [Required]
    [RegularExpression("^[a-zA-Z0-9_\\.-]+@([a-zA-Z0-9-]+\\.)+[a-zA-Z]{2,6}$", ErrorMessage = "E-mail is not valid")]
    public string Email { get; set; }

    [Required]
    public string Password { get; set; }

    [Required]
    public string Voornaam { get; set; }

    [Required]
    public string Familienaam { get; set; }

    [Required]
    public string GsmNr { get; set; }

    public string PasswordConfirmation { get; set; }
}
