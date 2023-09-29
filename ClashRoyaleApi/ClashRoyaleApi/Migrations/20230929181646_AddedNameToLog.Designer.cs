﻿// <auto-generated />
using System;
using ClashRoyaleCodeBase.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace ClashRoyaleApi.Migrations
{
    [DbContext(typeof(DataContext))]
    [Migration("20230929181646_AddedNameToLog")]
    partial class AddedNameToLog
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.11")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            modelBuilder.Entity("ClashRoyaleCodeBase.Models.DbModels.DbClanMemberLog", b =>
                {
                    b.Property<Guid>("Guid")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<int>("Status")
                        .HasColumnType("int");

                    b.Property<string>("Tag")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<DateTime>("Time")
                        .HasColumnType("datetime(6)");

                    b.HasKey("Guid");

                    b.ToTable("RiverClanMemberLog");
                });

            modelBuilder.Entity("ClashRoyaleCodeBase.Models.DbModels.DbClanMembers", b =>
                {
                    b.Property<Guid>("Guid")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)");

                    b.Property<bool>("IsActive")
                        .HasColumnType("tinyint(1)");

                    b.Property<bool>("IsInClan")
                        .HasColumnType("tinyint(1)");

                    b.Property<DateTime>("LastSeen")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("Role")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("Tag")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.HasKey("Guid");

                    b.ToTable("DbClanMembers");
                });

            modelBuilder.Entity("ClashRoyaleCodeBase.Models.DbModels.DbRiverRaceClan", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)");

                    b.Property<string>("CreatedDate")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<int>("Fame")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<int>("NewTrophies")
                        .HasColumnType("int");

                    b.Property<int>("Rank")
                        .HasColumnType("int");

                    b.Property<int>("SeasonId")
                        .HasColumnType("int");

                    b.Property<string>("SeasonSectionIndex")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<int>("SectionIndex")
                        .HasColumnType("int");

                    b.Property<string>("Tag")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<int>("TrophyChange")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("RiverRaceClan");
                });

            modelBuilder.Entity("ClashRoyaleCodeBase.Models.DbModels.DbRiverRaceParticipant", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)");

                    b.Property<int>("BoatAttacks")
                        .HasColumnType("int");

                    b.Property<string>("CreatedDate")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<int>("DecksNotUsed")
                        .HasColumnType("int");

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

                    b.Property<string>("SeasonSectionIndex")
                        .IsRequired()
                        .HasColumnType("longtext");

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
