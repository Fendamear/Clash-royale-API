using System.ComponentModel.DataAnnotations;

namespace ClashRoyaleCodeBase.Models.DbModels
{
    public class DbRiverRaceClan
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        public int SeasonId { get; set; }

        [Required]
        public int SectionIndex { get; set; }

        [Required]
        public string? SeasonSectionIndex { get; set; }

        [Required]
        public string? CreatedDate { get; set; }

        [Required]
        public int Rank { get; set; }

        [Required]
        public string? Tag { get; set; }

        [Required]
        public string? Name { get; set; }

        [Required]
        public int Fame { get; set; }

        [Required]
        public int TrophyChange { get; set; }

        [Required]
        public int NewTrophies { get; set; }    

    }
}
