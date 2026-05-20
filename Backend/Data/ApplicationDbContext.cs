using Microsoft.EntityFrameworkCore;
using Backend.Models;

namespace Backend.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<Acreditacion> Acreditaciones { get; set; }

        public DbSet<CertificadoEmitido> CertificadosEmitidos { get; set; }
    }
}