using System.ComponentModel.DataAnnotations;

namespace ClashRoyaleApi.Models.DbModels
{
    public class DBUser
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        public string ClanTag { get; set; } 

        [Required]
        public string Email { get; set; }

        public string UserName { get; set; }    

        [Required]
        public string Password { get; set; }

        [Required]
        public UserRole Role { get; set; }

        [Required]
        public DateTime DateEnrolled { get; set; }

        public DBUser()
        {

        }
    }

    public enum UserRole
    {
        Member,
        CoLeader,
        Admin
    }
}
