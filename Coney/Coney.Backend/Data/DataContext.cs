using Coney.Shared.Entities;
using Microsoft.EntityFrameworkCore;

namespace Coney.Backend.Data;

public class DataContext : DbContext

{
    public DataContext(DbContextOptions<DataContext> options) : base(options)
    {
    }

    public DbSet<Country> Countries { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<Country>().HasIndex(x => x.Name).IsUnique();
    }

    public DbSet<Rule> Rules { get; set; }
}