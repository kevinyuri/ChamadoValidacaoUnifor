using Microsoft.EntityFrameworkCore;
using UserValidacaoUnifor.Models;

namespace UserValidacaoUnifor.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
        public DbSet<User> Users { get; set; }
        public DbSet<Chamado> Chamados { get; set; }
    }
}
