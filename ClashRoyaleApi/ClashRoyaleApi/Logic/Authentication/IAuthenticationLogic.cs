using ClashRoyaleApi.DTOs.Authentication;
using ClashRoyaleApi.DTOs.Authentication.Register;

namespace ClashRoyaleApi.Logic.Authentication
{
    public interface IAuthenticationLogic
    {
        ClanTagDTO RegisterWithClanTag(string clanTag);
        Task<CreateUserDTO> RegisterUser(CreateUserDTO register);

        Task<List<ClanTagNameDTO>> GetAllClanTagsWithName();
    }
}
