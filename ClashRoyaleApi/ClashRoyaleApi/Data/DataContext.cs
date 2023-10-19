using ClashRoyaleApi.Logic.Logging.LoggingModels;
using ClashRoyaleApi.Models.DbModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ClashRoyaleApi.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {

        }

        protected DataContext()
        {

        }

        public DbSet<DbRiverRaceParticipant> RiverRaceParticipant { get; set;}

        public DbSet<DbRiverRaceClan> RiverRaceClan { get; set; }

        public DbSet<DbClanMemberLog> RiverClanMemberLog { get; set; }

        public DbSet<DbClanMembers> DbClanMembers { get; set; }

        public DbSet<DBUser> DBUser { get; set; }   

        public DbSet<DbRiverRaceLog> RiverRaceLogs { get; set; }

        public DbSet<DbCurrentRiverRace> CurrentRiverRace { get; set;}


        //data entries for logging

        public DbSet<CurrentRiverRaceLog> CurrentRiverRaceLogs { get; set; }    

    }
}
