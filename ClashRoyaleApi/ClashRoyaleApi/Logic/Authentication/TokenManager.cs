using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using ClashRoyaleApi.Models.DbModels;

namespace ClashRoyaleApi.Logic.Authentication
{
    public class TokenManager
    {
        private readonly IConfiguration _configuration;

        public TokenManager(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string CreateToken(DBUser user)
        {
            List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.Role, user.Role.ToString()),
                new Claim("ID", user.Id.ToString()),
                new Claim("ClanTag", user.ClanTag),
                new Claim("Admin", (user.Role == UserRole.Admin ? true : false).ToString())
            };
            //_configuration.GetSection("AppSettings:Token").Value!)
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration.GetSection("AppSettings:Token").Value!));

            var cred = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            DateTime dateTime = DateTime.Now.AddHours(1);

            //feature for testing
            if (user.Role == UserRole.Admin)
            {
                dateTime = dateTime.AddHours(11);
                //dateTime.AddDays(1);
            }

            var token = new JwtSecurityToken(
                claims: claims,
                expires: dateTime,
                signingCredentials: cred             
            );

            var jwt = new JwtSecurityTokenHandler().WriteToken(token);

            return jwt;
        }
    }
}
