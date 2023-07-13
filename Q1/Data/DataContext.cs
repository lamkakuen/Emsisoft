using Microsoft.EntityFrameworkCore;
using Q1.Models;

namespace Q1.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
        }

        public DbSet<Hash> Hashes { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Hash>().ToTable("Hashes");
        }
    }
}
