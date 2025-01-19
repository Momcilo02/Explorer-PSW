using Explorer.Tours.Core.Domain;
using Explorer.Tours.Core.Domain.TourExecutions;
using Microsoft.EntityFrameworkCore;
using TourObject = Explorer.Tours.Core.Domain.TourObject;

namespace Explorer.Tours.Infrastructure.Database;

public class ToursContext : DbContext
{
    public DbSet<Equipment> Equipment { get; set; }
    public DbSet<TourEquipment> TourEquipment { get; set; }
    public DbSet<TourReview> TourReview { get; set; }
    public DbSet<KeyPoint> KeyPoints { get; set; }
    public DbSet<Tour> Tours { get; set; }
    public DbSet<TourObject> TourObjects { get; set; }
    public DbSet<TourExecution>  TourExecutions { get; set; }
    public DbSet<TouristLocation> TouristLocation { get; set; }
    public DbSet<Quiz> Quizzes { get; set; }
    public DbSet<QuizQuestion> QuizQuestions { get; set; }
    public DbSet<QuizAnswer> QuizAnswers { get; set; }
    public DbSet<TouristEquipment> TouristEquipments { get; set; }
    public ToursContext(DbContextOptions<ToursContext> options) : base(options) {}

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema("tours");
        modelBuilder.Entity<Tour>().
            HasMany(t => t.KeyPoints)
            .WithOne();

        modelBuilder.Entity<Tour>().
            HasMany(t => t.Equipments)
            .WithMany();

        modelBuilder.Entity<Tour>().Property(item => item.TourDurations).HasColumnType("jsonb");
        modelBuilder.Entity<TourExecution>().Property(item => item.CompletedKeyPoints).HasColumnType("jsonb");

        modelBuilder.Entity<Quiz>(entity =>
        {
            entity.HasKey(q => q.Id);

            // Reward kao owned type
            entity.OwnsOne(q => q.Reward, reward =>
            {
                reward.Property(r => r.Type)
                      .HasConversion<string>(); // Konvertuje RewardType enum u string

                reward.Property(r => r.Amount)
                      .IsRequired();
            });

            // Quiz Questions
            entity.HasMany(q => q.Questions)
                  .WithOne()
                  .HasForeignKey("QuizId");
        });

        modelBuilder.Entity<QuizQuestion>(entity =>
        {
            entity.HasKey(qq => qq.Id);

            entity.OwnsMany(qq => qq.Answers, a =>
            {
                a.WithOwner().HasForeignKey("QuizQuestionId");
                a.HasKey("QuizQuestionId", "Id");
                a.Property(a => a.AnswerText).IsRequired();
            });
        });
        modelBuilder.Entity<TouristEquipment>()
        .HasOne<Equipment>()
        .WithMany()
        .HasForeignKey(te => te.EquipmentId);
    }

   
}