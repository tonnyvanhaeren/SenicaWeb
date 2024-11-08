using System;
using Microsoft.AspNetCore.Identity;

namespace SenicaWeb.Models;

public class AppUser : IdentityUser
{
    public string Voornaam { get; set; }
    public string Familienaam { get; set; }
    public string GsmNr { get; set; }
}
