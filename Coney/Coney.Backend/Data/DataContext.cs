﻿using Coney.Shared.Entities;
using Microsoft.EntityFrameworkCore;

namespace Coney.Backend.Data;

public class DataContext : DbContext

{
    public DataContext(DbContextOptions<DataContext> options) : base(options)
    {
    }

    public DbSet<Country> Countries { get; set; }
    public DbSet<State> States { get; set; }
    public DbSet<City> Cities { get; set; }

    public DbSet<Rule> Rules { get; set; }
    public DbSet<Prize> Prices { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<Notification> Notifications { get; set; }

    public DbSet<Riffle> Riffles { get; set; }
    public DbSet<Ticket> Tickets { get; set; }
    public DbSet<RiffleXRule> RiffleXRules { get; set; }
    public DbSet<Support> Supports { get; set; }

    public DbSet<Winner> Winners { get; set; }
    public DbSet<Comment> Comments { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<Country>().HasIndex(x => x.Name).IsUnique();
        modelBuilder.Entity<State>().HasIndex(x => new { x.CountryId, x.Name }).IsUnique();
        modelBuilder.Entity<City>().HasIndex(c => new { c.StateId, c.Name }).IsUnique();
        modelBuilder.Entity<Ticket>().HasIndex(t => new { t.RiffleId, t.Code }).IsUnique();
        modelBuilder.Entity<RiffleXRule>().HasIndex(rr => new { rr.RiffleId, rr.RuleId }).IsUnique();
        modelBuilder.Entity<Prize>().Property(p => p.Value).ValueGeneratedNever();
        modelBuilder.Entity<Winner>().HasIndex(w => new { w.PrizeId, w.UserId, w.RiffleId }).IsUnique();

        DisableCascadingDelete(modelBuilder);
    }

    private void DisableCascadingDelete(ModelBuilder modelBuilder)
    {
        var relationships = modelBuilder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys());
        foreach (var relationship in relationships)
        {
            relationship.DeleteBehavior = DeleteBehavior.Restrict;
        }
    }
}