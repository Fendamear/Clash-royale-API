using System.ComponentModel.DataAnnotations;

namespace ClashRoyaleApi.Models.DbModels
{
    public class DbCallList
    {
        [Key]
        public Guid Guid { get; set; }

        public string Coleader { get; set; }

        public string ColeaderClanTag { get; set; }

        public string Member { get; set; }

        public string MemberTag { get; set; }
        
        public DbCallList() { }

        public DbCallList(string coleader, string coLeaderTag, string member, string memberTag) 
        { 
            Guid = Guid.NewGuid();
            Member = member;
            MemberTag = memberTag;
            ColeaderClanTag = coLeaderTag;
            Coleader = coleader;       
        }
    }
}
