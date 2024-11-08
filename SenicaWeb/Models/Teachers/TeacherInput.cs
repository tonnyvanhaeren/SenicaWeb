using System;
using System.ComponentModel.DataAnnotations;

namespace SenicaWeb.Models.Teachers;

public class TeacherInput
{
    public string FotoUrl { get; set; }
    
    [Required]
    public string Voornaam { get; set; }
    [Required]
    public string Familienaam { get; set; }
    [Required]
    public string Email { get; set; }
    [Required]
    public string GsmNr { get; set; }
}
