using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace SenicaWeb.Models;

public class Teacher
{
    [Key]
    public string TeacherId { get; set; }
    public string FotoUrl { get; set; }
    public string Voornaam { get; set; }
    public string Familienaam { get; set; }
    public string Email { get; set; }
    public string GsmNr { get; set; }

}
