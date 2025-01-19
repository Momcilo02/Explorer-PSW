﻿// <auto-generated />
using System;
using Explorer.Encounters.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Explorer.Encounters.Infrastructure.Migrations
{
    [DbContext(typeof(EncountersContext))]
    partial class EncountersContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasDefaultSchema("encounters")
                .HasAnnotation("ProductVersion", "7.0.5")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("Explorer.Encounters.Core.Domain.Encounter", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<long>("Id"));

                    b.Property<double>("ActivateRange")
                        .HasColumnType("double precision");

                    b.Property<int>("CreatorId")
                        .HasColumnType("integer");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("EncounterType")
                        .HasColumnType("integer");

                    b.Property<double>("Latitude")
                        .HasColumnType("double precision");

                    b.Property<double>("Longitude")
                        .HasColumnType("double precision");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("Status")
                        .HasColumnType("integer");

                    b.Property<int>("TotalXp")
                        .HasColumnType("integer");

                    b.Property<int?>("TourId")
                        .HasColumnType("integer");

                    b.Property<int?>("TouristRequestStatus")
                        .HasColumnType("integer");

                    b.Property<bool?>("isTourRequired")
                        .HasColumnType("boolean");

                    b.HasKey("Id");

                    b.ToTable("Encounters", "encounters");

                    b.UseTptMappingStrategy();
                });

            modelBuilder.Entity("Explorer.Encounters.Core.Domain.EncounterExecution", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<long>("Id"));

                    b.Property<int>("EncounterId")
                        .HasColumnType("integer");

                    b.Property<int>("NumberOfActiveTourists")
                        .HasColumnType("integer");

                    b.Property<int>("Status")
                        .HasColumnType("integer");

                    b.Property<int>("TouristId")
                        .HasColumnType("integer");

                    b.Property<double>("TouristLatitude")
                        .HasColumnType("double precision");

                    b.Property<double>("TouristLongitude")
                        .HasColumnType("double precision");

                    b.HasKey("Id");

                    b.ToTable("EncounterExecution", "encounters");
                });

            modelBuilder.Entity("Explorer.Encounters.Core.Domain.HiddenLocationEncounter", b =>
                {
                    b.HasBaseType("Explorer.Encounters.Core.Domain.Encounter");

                    b.Property<string>("Image")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<double>("ImageLatitude")
                        .HasColumnType("double precision");

                    b.Property<double>("ImageLongitude")
                        .HasColumnType("double precision");

                    b.ToTable("HiddenLocationEncounters", "encounters");
                });

            modelBuilder.Entity("Explorer.Encounters.Core.Domain.MiscEncounter", b =>
                {
                    b.HasBaseType("Explorer.Encounters.Core.Domain.Encounter");

                    b.Property<string>("Instructions")
                        .IsRequired()
                        .HasColumnType("text");

                    b.ToTable("MiscEncounters", "encounters");
                });

            modelBuilder.Entity("Explorer.Encounters.Core.Domain.SocialEncounter", b =>
                {
                    b.HasBaseType("Explorer.Encounters.Core.Domain.Encounter");

                    b.Property<int>("PeopleNumb")
                        .HasColumnType("integer");

                    b.ToTable("SocialEncounters", "encounters");
                });

            modelBuilder.Entity("Explorer.Encounters.Core.Domain.HiddenLocationEncounter", b =>
                {
                    b.HasOne("Explorer.Encounters.Core.Domain.Encounter", null)
                        .WithOne()
                        .HasForeignKey("Explorer.Encounters.Core.Domain.HiddenLocationEncounter", "Id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Explorer.Encounters.Core.Domain.MiscEncounter", b =>
                {
                    b.HasOne("Explorer.Encounters.Core.Domain.Encounter", null)
                        .WithOne()
                        .HasForeignKey("Explorer.Encounters.Core.Domain.MiscEncounter", "Id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Explorer.Encounters.Core.Domain.SocialEncounter", b =>
                {
                    b.HasOne("Explorer.Encounters.Core.Domain.Encounter", null)
                        .WithOne()
                        .HasForeignKey("Explorer.Encounters.Core.Domain.SocialEncounter", "Id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
