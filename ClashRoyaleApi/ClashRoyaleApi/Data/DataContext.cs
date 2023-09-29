using ClashRoyaleApi.Models.DbModels;
using Microsoft.EntityFrameworkCore;

namespace ClashRoyaleApi.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {

        }

        public DbSet<DbRiverRaceParticipant> RiverRaceParticipant { get; set;}

        public DbSet<DbRiverRaceClan> RiverRaceClan { get; set; }
    }
}
