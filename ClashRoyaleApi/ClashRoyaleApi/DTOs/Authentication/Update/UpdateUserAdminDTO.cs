using ClashRoyaleApi.Models.DbModels;

namespace ClashRoyaleApi.DTOs.Authentication.Update
{
    public class UpdateUserAdminDTO
    {
        public string ClanTag { get; set; }

        public UserRole Role { get; set; }
    }
}
