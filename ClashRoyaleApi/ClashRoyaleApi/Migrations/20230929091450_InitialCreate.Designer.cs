﻿// <auto-generated />
using System;
using ClashRoyaleApi.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace ClashRoyaleApi.Migrations
{
    [DbContext(typeof(DataContext))]
    [Migration("20230929091450_InitialCreate")]
    partial class InitialCreate
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.11")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            modelBuilder.Entity("ClashRoyaleApi.Models.DbModels.DbRiverRaceClan", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)");

                    b.Property<string>("CreatedDate")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<int>("SeasonId")
                        .HasColumnType("int");

                    b.Property<int>("SectionIndex")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("RiverRaceClan");
                });

            modelBuilder.Entity("ClashRoyaleApi.Models.DbModels.DbRiverRaceParticipant", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)");

                    b.Property<int>("BoatAttacks")
                        .HasColumnType("int");

                    b.Property<string>("CreatedDate")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<int>("DecksUsed")
                        .HasColumnType("int");

                    b.Property<int>("DecksUsedToday")
                        .HasColumnType("int");

                    b.Property<int>("Fame")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<int>("RepairPoints")
                        .HasColumnType("int");

                    b.Property<int>("SeasonId")
                        .HasColumnType("int");

                    b.Property<int>("SectionIndex")
                        .HasColumnType("int");

                    b.Property<string>("Tag")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.HasKey("Id");

                    b.ToTable("RiverRaceParticipant");
                });
#pragma warning restore 612, 618
        }
    }
}
