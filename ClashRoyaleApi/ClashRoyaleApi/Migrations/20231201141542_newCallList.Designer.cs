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
    [Migration("20231201141542_newCallList")]
    partial class newCallList
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.11")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            modelBuilder.Entity("ClashRoyaleApi.Models.CurrentRiverRace.CRR_Response.CurrentRiverRaceLog", b =>
                {
                    b.Property<Guid>("Guid")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)");

                    b.Property<int>("DayId")
                        .HasColumnType("int");

                    b.Property<string>("Exception")
                        .HasColumnType("longtext");

                    b.Property<int>("SchedulerTime")
                        .HasColumnType("int");

                    b.Property<int>("SeasonId")
                        .HasColumnType("int");

                    b.Property<int>("SectionId")
                        .HasColumnType("int");

                    b.Property<int>("Status")
                        .HasColumnType("int");

                    b.Property<DateTime>("TimeStamp")
                        .HasColumnType("datetime(6)");

                    b.HasKey("Guid");

                    b.ToTable("CurrentRiverRaceLogs");
                });

            modelBuilder.Entity("ClashRoyaleApi.Models.DbModels.DBUser", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)");

                    b.Property<string>("ClanTag")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<DateTime>("DateEnrolled")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<int>("Role")
                        .HasColumnType("int");

                    b.Property<string>("UserName")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.HasKey("Id");

                    b.ToTable("DBUser");
                });

            modelBuilder.Entity("ClashRoyaleApi.Models.DbModels.DbCallList", b =>
                {
                    b.Property<Guid>("Guid")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)");

                    b.Property<string>("Coleader")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("ColeaderClanTag")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("Member")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("MemberTag")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.HasKey("Guid");

                    b.ToTable("CallList");
                });

            modelBuilder.Entity("ClashRoyaleApi.Models.DbModels.DbClanMemberLog", b =>
                {
                    b.Property<Guid>("Guid")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("NewValue")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("OldValue")
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

            modelBuilder.Entity("ClashRoyaleApi.Models.DbModels.DbClanMembers", b =>
                {
                    b.Property<Guid>("Guid")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)");

                    b.Property<string>("ClanTag")
                        .IsRequired()
                        .HasColumnType("longtext");

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

                    b.HasKey("Guid");

                    b.ToTable("DbClanMembers");
                });

            modelBuilder.Entity("ClashRoyaleApi.Models.DbModels.DbCurrentRiverRace", b =>
                {
                    b.Property<Guid>("Guid")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)");

                    b.Property<int>("DayId")
                        .HasColumnType("int");

                    b.Property<int>("DecksNotUsed")
                        .HasColumnType("int");

                    b.Property<int>("DecksUsedToday")
                        .HasColumnType("int");

                    b.Property<int>("Fame")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .HasColumnType("longtext");

                    b.Property<int>("Schedule")
                        .HasColumnType("int");

                    b.Property<int>("SeasonId")
                        .HasColumnType("int");

                    b.Property<string>("SeasonSectionDay")
                        .HasColumnType("longtext");

                    b.Property<int>("SectionId")
                        .HasColumnType("int");

                    b.Property<string>("Tag")
                        .HasColumnType("longtext");

                    b.HasKey("Guid");

                    b.ToTable("CurrentRiverRace");
                });

            modelBuilder.Entity("ClashRoyaleApi.Models.DbModels.DbRiverRaceClan", b =>
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

            modelBuilder.Entity("ClashRoyaleApi.Models.DbModels.DbRiverRaceLog", b =>
                {
                    b.Property<Guid>("Guid")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)");

                    b.Property<int>("SeasonId")
                        .HasColumnType("int");

                    b.Property<int>("SectionId")
                        .HasColumnType("int");

                    b.Property<DateTime>("TimeStamp")
                        .HasColumnType("datetime(6)");

                    b.Property<int>("Type")
                        .HasColumnType("int");

                    b.HasKey("Guid");

                    b.ToTable("RiverRaceLogs");
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

            modelBuilder.Entity("ClashRoyaleApi.Models.Mail.MailSubscriptions", b =>
                {
                    b.Property<Guid>("Guid")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)");

                    b.Property<string>("ClanTag")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<int>("MailType")
                        .HasColumnType("int");

                    b.Property<int?>("SchedulerTime")
                        .HasColumnType("int");

                    b.HasKey("Guid");

                    b.ToTable("MailSubscriptions");
                });
#pragma warning restore 612, 618
        }
    }
}
