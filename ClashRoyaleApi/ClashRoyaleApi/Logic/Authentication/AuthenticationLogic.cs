using ClashRoyaleApi.Data;
using ClashRoyaleApi.DTOs.Authentication;
using ClashRoyaleApi.DTOs.Authentication.Register;
using ClashRoyaleApi.Models.DbModels;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using System.Reflection.Metadata.Ecma335;

namespace ClashRoyaleApi.Logic.Authentication
{
    public class AuthenticationLogic : IAuthenticationLogic
    {

        private readonly DataContext _dataContext;

        public AuthenticationLogic(DataContext context)
        {
            _dataContext = context;
        }

        public ClanTagDTO RegisterWithClanTag(string clanTag) 
        {
            if (!_dataContext.DbClanMembers.Any(t => t.ClanTag == clanTag)) throw new Exception("clan tag does not exist within herres");
            if (_dataContext.DBUser.Any(t => t.ClanTag == clanTag)) throw new Exception("this clantag is already linked to a user!");

            return new ClanTagDTO(clanTag);
        }

        public async Task<CreateUserDTO> RegisterUser(CreateUserDTO register)
        {
            //check if all values are there
            foreach (PropertyInfo pi in register.GetType().GetProperties())
            {
                if (pi.PropertyType == typeof(string))
                {
                    string value = (string)pi.GetValue(register);
                    if (string.IsNullOrEmpty(value))
                    {
                        throw new InvalidOperationException("missing value");
                    }
                }
            }

            DbClanMembers member = _dataContext.DbClanMembers.FirstOrDefault(t => t.ClanTag == register.ClanTag);

            string password = BCrypt.Net.BCrypt.HashPassword(register.Password);
            Guid id = Guid.NewGuid();

            UserRole role = new UserRole();

            if(member.Role != "CoLeader")
            {
                role = UserRole.Member;
            }
            else
            {
                role = UserRole.CoLeader;
            }

            DBUser user = new DBUser()
            {
                Id = id,
                ClanTag = register.ClanTag,
                Email = register.Email,
                Password = password,
                Role = role,
                UserName = member.Name,
                DateEnrolled = DateTime.Now,    
            };

            if (_dataContext.DBUser.Any(e => e.Email == user.Email)) throw new ArgumentException("Email already exists");
            _dataContext.DBUser.Add(user);
            await _dataContext.SaveChangesAsync();

            return register;

        }

        public async Task<List<ClanTagNameDTO>> GetAllClanTagsWithName()
        {
            List<DBUser> users = await _dataContext.DBUser.ToListAsync(); 
            List<ClanTagNameDTO> response = new List<ClanTagNameDTO>();

            foreach (DBUser user in users) 
            { 
                ClanTagNameDTO dto = new ClanTagNameDTO();
                dto.ClanTag = user.ClanTag;
                dto.Name = user.UserName;
                response.Add(dto);         
            }
            return response;
        }
    }
}
