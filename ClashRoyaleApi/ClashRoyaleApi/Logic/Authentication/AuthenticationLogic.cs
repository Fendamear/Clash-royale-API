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
        private readonly IConfiguration _configuration; 

        public AuthenticationLogic(DataContext context, IConfiguration configuration)
        {
            _dataContext = context;
            _configuration = configuration;
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

            if (member.Role != "CoLeader")
            {
                role = UserRole.Member;
            }
            else
            {
                role = UserRole.CoLeader;
            }

            if (member.ClanTag == "#LY0UQ9R")
            {
                role = UserRole.Admin;
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

        public async Task<List<ClanTagNameDTO>> GetAllClanTagsWithNameFromDbClanMemberDb()
        {
            List<DbClanMembers> members = _dataContext.DbClanMembers.Where(x => x.IsInClan == true).ToList();
            List<ClanTagNameDTO> response = new List<ClanTagNameDTO>();

            foreach (DbClanMembers user in members) 
            { 
                response.Add(new ClanTagNameDTO(user.ClanTag, user.Name));         
            }
            return response;
        }

        public async Task<TokenDTO> GenerateToken(LoginDTO dto)
        {
            TokenDTO token = new TokenDTO();

            TokenManager manager = new TokenManager(_configuration);

            try
            {
                DBUser user = await _dataContext.DBUser.SingleAsync(u => u.Email == dto.Email);
                if (!BCrypt.Net.BCrypt.Verify(dto.Password, user.Password)) throw new Exception("Email or passwords do not match");
                token.Token = manager.CreateToken(user);
            }
            catch (InvalidOperationException ex)
            {
                throw new Exception("User does not exist");
            }
            catch (Exception ex)
            {
                throw;
            }
            return token;
        }
    }
}
