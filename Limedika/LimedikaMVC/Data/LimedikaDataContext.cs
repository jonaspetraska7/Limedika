using LimedikaMVC.Models;
using Microsoft.EntityFrameworkCore;

namespace LimedikaMVC.Data
{
    public class LimedikaDataContext : DbContext
    {
        public LimedikaDataContext(DbContextOptions<LimedikaDataContext> options) : base(options) { }

        public DbSet<Client> Clients { get; set; }
        public DbSet<Log> Logs { get; set; }
    }
}
