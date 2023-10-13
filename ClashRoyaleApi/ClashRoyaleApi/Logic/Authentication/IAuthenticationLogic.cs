using ClashRoyaleApi.DTOs.Authentication;

namespace ClashRoyaleApi.Logic.Authentication
{
    public interface IAuthenticationLogic
    {
        ClanTagDTO RegisterWithClanTag(string clanTag);
        Task<CreateUserDTO> RegisterUser(CreateUserDTO register);

    }
}
