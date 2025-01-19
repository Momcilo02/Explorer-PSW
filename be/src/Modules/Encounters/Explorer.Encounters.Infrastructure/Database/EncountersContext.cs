using Explorer.Encounters.Core.Domain;
using Explorer.Encounters.Infrastructure.Database.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.Encounters.Infrastructure.Database
{
    public class EncountersContext : DbContext
    {

        public DbSet<Encounter>Encounters { get; set; }
        public DbSet<HiddenLocationEncounter> HiddenLocationEncounters { get; set; }

        public DbSet<SocialEncounter> SocialEncounters { get; set; }

        public DbSet<MiscEncounter> MiscEncounters { get; set; }
        public DbSet<EncounterExecution> EncounterExecution { get; set; }

        public EncountersContext(DbContextOptions<EncountersContext> options) : base(options) { }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.HasDefaultSchema("encounters");

            // Konfigurisanje HiddenLocationEncounter tabele
            modelBuilder.Entity<HiddenLocationEncounter>(entity =>
            {
                entity.ToTable("HiddenLocationEncounters");
                entity.HasBaseType<Encounter>();
                entity.Property(e => e.Image).IsRequired();
                entity.Property(e => e.ImageLongitude).IsRequired();
                entity.Property(e => e.ImageLatitude).IsRequired();
            });

            // Konfigurisanje SocialEncounter tabele
            modelBuilder.Entity<SocialEncounter>(entity =>
            {
                entity.ToTable("SocialEncounters");
                entity.HasBaseType<Encounter>();
                entity.Property(e => e.PeopleNumb).IsRequired();
            });

            // Konfigurisanje MiscEncounter tabele
            modelBuilder.Entity<MiscEncounter>(entity =>
            {
                entity.ToTable("MiscEncounters");
                entity.HasBaseType<Encounter>();
                entity.Property(e => e.Instructions).IsRequired();
            });

            base.OnModelCreating(modelBuilder);
        }
    }
}
