using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace FinalProject.Models.Domains
{
    public class DatabaseContext : IdentityDbContext<ApplicationUser>
    {
        public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options) { }
        public DbSet<Morador> Morador { get; set; }
        public DbSet<Reserva> Reserva { get; set; }
        public DbSet<Administrador> Administrador { get; set; }
    }
}
