﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using MySpot.Infrastructure.DAL;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace MySpot.Infrastructure.DAL.Migrations
{
    [DbContext(typeof(MySpotDbContext))]
    partial class MySpotDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.3")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("MySpot.Core.Entities.Reservation", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uuid");

                    b.Property<DateTimeOffset?>("Date")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("EmployeeName")
                        .HasColumnType("text");

                    b.Property<string>("LicensePlate")
                        .HasColumnType("text");

                    b.Property<Guid?>("ParkingSpotId")
                        .HasColumnType("uuid");

                    b.Property<Guid?>("WeeklyParkingSpotId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("WeeklyParkingSpotId");

                    b.ToTable("Reservations");
                });

            modelBuilder.Entity("MySpot.Core.Entities.WeeklyParkingSpot", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uuid");

                    b.Property<string>("Name")
                        .HasColumnType("text");

                    b.Property<DateTimeOffset?>("Week")
                        .HasColumnType("timestamp with time zone");

                    b.HasKey("Id");

                    b.ToTable("WeeklyParkingSpots");
                });

            modelBuilder.Entity("MySpot.Core.Entities.Reservation", b =>
                {
                    b.HasOne("MySpot.Core.Entities.WeeklyParkingSpot", null)
                        .WithMany("Reservations")
                        .HasForeignKey("WeeklyParkingSpotId");
                });

            modelBuilder.Entity("MySpot.Core.Entities.WeeklyParkingSpot", b =>
                {
                    b.Navigation("Reservations");
                });
#pragma warning restore 612, 618
        }
    }
}