using System.Runtime.Serialization;
using System.Security.Claims;

namespace ClashRoyaleApi.Models
{
    public static class UtilityClass
    {
        public static string GetEnumAttributeValue(Enum @enum)
        {
            var field = @enum.GetType().GetField(@enum.ToString());

            if (field == null)
            {
                return string.Empty;
            }

            var attribute = (EnumMemberAttribute?)Attribute.GetCustomAttribute(field, typeof(EnumMemberAttribute));

            if (attribute == null || attribute.Value == null)
            {
                return string.Empty;
            }

            return attribute.Value;
        }

        public static string GetValueFromToken(ClaimsIdentity identity, EnumClass.JWTToken token)
        {
            string auth = "Authorization";
            string res = string.Empty;

            if (identity != null)
            {
                IEnumerable<Claim> claims = identity.Claims;
                try
                {
                    res = claims.FirstOrDefault(x => x.Type == token.ToString()).Value;
                }
                catch (Exception)
                {
                    throw new Exception("Token Not available");
                }
            }
            return res;
        }





    }
}
