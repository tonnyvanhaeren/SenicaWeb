using System.ComponentModel.DataAnnotations;

namespace SenicaWeb.Models.Auth;

public class LoginInput
{
    [Required]
    public string Email { get; set; }

    [Required]
    public string Password { get; set; }

    public string ReturnUrl { get; set; }
    
    public bool Remember { get; set; }
}
