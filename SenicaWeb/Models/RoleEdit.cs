using System;
using Microsoft.AspNetCore.Identity;

namespace SenicaWeb.Models;

public class RoleEdit
{
    public IdentityRole Role { get; set; }
    public IEnumerable<AppUser> Members { get; set; }
    public IEnumerable<AppUser> NonMembers { get; set; }
}
