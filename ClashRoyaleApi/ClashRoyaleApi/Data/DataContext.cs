using ClashRoyaleApi.DTOs.Current_River_Race.Homepage;
using ClashRoyaleApi.Models.CurrentRiverRace.CRR_Response;
using ClashRoyaleApi.Models.DbModels;
using ClashRoyaleApi.Models.Mail;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using static ClashRoyaleApi.Models.EnumClass;

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

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }

        public DbSet<DbRiverRaceParticipant> RiverRaceParticipant { get; set;}

        public DbSet<DbRiverRaceClan> RiverRaceClan { get; set; }

        public DbSet<DbClanMemberLog> RiverClanMemberLog { get; set; }

        public virtual DbSet<DbClanMembers> DbClanMembers { get; set; }

        public DbSet<DBUser> DBUser { get; set; }   

        public virtual DbSet<DbRiverRaceLog> RiverRaceLogs { get; set; }

        public virtual DbSet<DbCurrentRiverRace> CurrentRiverRace { get; set;}

        //mail

        public DbSet<MailSubscriptions> MailSubscriptions { get; set; }

        //Call List

        public DbSet<DbCallList> CallList { get; set; }

        //data entries for logging

        public DbSet<CurrentRiverRaceLog> CurrentRiverRaceLogs { get; set; }

        public List<DBUser> GetDBUsersWithMailSubscriptions(MailType mailType, SchedulerTime? schedulerTime)
        {
            var query = from user in DBUser
                        join subscription in MailSubscriptions
                        on user.ClanTag equals subscription.ClanTag
                        where subscription.MailType == mailType && subscription.SchedulerTime == schedulerTime
                        select user;

            return query.ToList();
        }

        public LowestHighestDTO GetDecksNotUsed(bool asc)
        {
            var query = from race in CurrentRiverRace
                        group race by race.Tag into g
                        select new
                        {
                            Name = g.FirstOrDefault().Name,
                            TotalDecksNotUsed = g.Sum(r => r.DecksNotUsed)
                        } into result
                        orderby asc ? result.TotalDecksNotUsed : -result.TotalDecksNotUsed descending
                        select new { result.Name, result.TotalDecksNotUsed };
              
            var top = query.FirstOrDefault();
            return new LowestHighestDTO(top.Name, top.TotalDecksNotUsed);
        }
    }
}
