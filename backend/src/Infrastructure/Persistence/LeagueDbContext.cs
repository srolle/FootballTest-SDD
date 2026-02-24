using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence;

public class LeagueDbContext(DbContextOptions<LeagueDbContext> options) : DbContext(options)
{
    public DbSet<Team> Teams => Set<Team>();
    public DbSet<Matchday> Matchdays => Set<Matchday>();
    public DbSet<Match> Matches => Set<Match>();
    public DbSet<StandingEntry> StandingEntries => Set<StandingEntry>();
    public DbSet<ResultChangeLog> ResultChangeLogs => Set<ResultChangeLog>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Team>(builder =>
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Name).IsRequired();
            builder.HasIndex(x => x.Name).IsUnique();
        });

        modelBuilder.Entity<Matchday>(builder =>
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Number).IsRequired();
            builder.HasIndex(x => x.Number).IsUnique();
        });

        modelBuilder.Entity<Match>(builder =>
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Status).IsRequired();
            builder.HasOne(x => x.Matchday)
                .WithMany()
                .HasForeignKey(x => x.MatchdayId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<StandingEntry>(builder =>
        {
            builder.HasKey(x => x.Id);
            builder.HasIndex(x => new { x.MatchdayId, x.TeamId }).IsUnique();
        });

        modelBuilder.Entity<ResultChangeLog>(builder =>
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.ChangeType).IsRequired();
        });

        base.OnModelCreating(modelBuilder);
    }
}
