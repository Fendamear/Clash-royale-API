using System.Runtime.Serialization;

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
    }
}
