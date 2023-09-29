using System.ComponentModel.DataAnnotations;

namespace ClashRoyaleCodeBase.Models.DbModels
{
    public class DbRiverRaceParticipant
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
        public string Tag { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public int Fame { get; set; }

        [Required]
        public int RepairPoints { get; set; }

        [Required]
        public int BoatAttacks { get; set; }

        [Required]
        public int DecksUsed { get; set; }

        [Required]
        public int DecksUsedToday { get; set; }

        public int DecksNotUsed { get; set; }
    }
}
