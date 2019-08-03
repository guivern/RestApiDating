using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using RestApiDating.Models;

namespace RestApiDating.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options)
        : base(options) { }

        public DbSet<Value> Values { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Foto> Fotos { get; set; }
    }
}