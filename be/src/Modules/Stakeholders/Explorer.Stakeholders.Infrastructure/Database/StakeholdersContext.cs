using Explorer.Stakeholders.Core.Domain;
using Explorer.Stakeholders.Core.Domain.TourProblemReports;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Metadata;
using System.Xml.Linq;

namespace Explorer.Stakeholders.Infrastructure.Database;

public class StakeholdersContext : DbContext
{
    public DbSet<User> Users { get; set; }
    public DbSet<Person> People { get; set; }
    public DbSet<TourProblemReport> TourProblemReports { get; set; }
    public DbSet<Notification> Notifications { get; set; }

    public DbSet<TouristClub> TouristClub { get; set; }
    public DbSet<ProfileMessage> ProfileMessages { get; set; }
    public DbSet<ClubMessage> ClubMessages { get; set; }


    public StakeholdersContext(DbContextOptions<StakeholdersContext> options) : base(options) {}

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<TourProblemReport>().Property(item => item.Messages).HasColumnType("jsonb");
        modelBuilder.HasDefaultSchema("stakeholders");

        modelBuilder.Entity<User>().HasIndex(u => u.Username).IsUnique();

        ConfigureStakeholder(modelBuilder);
    }

    private static void ConfigureStakeholder(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Person>()
            .HasOne<User>()
            .WithOne()
        .HasForeignKey<Person>(s => s.UserId);
    }
}
