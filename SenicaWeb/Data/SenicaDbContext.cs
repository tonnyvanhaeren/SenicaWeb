using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using SenicaWeb.Models;

namespace SenicaWeb.Data;

public class SenicaDbContext : IdentityDbContext<AppUser>
{
    public SenicaDbContext(DbContextOptions<SenicaDbContext> options) : base(options) {}

        public DbSet<Teacher> Teachers {get; set;}
}