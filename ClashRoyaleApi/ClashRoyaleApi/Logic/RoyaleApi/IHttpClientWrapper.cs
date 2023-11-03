using static ClashRoyaleApi.Models.EnumClass;

namespace ClashRoyaleApi.Logic.RoyaleApi
{
    public interface IHttpClientWrapper
    {
        Task<string> RoyaleApiCall(RoyaleApiType type);
       
    }
}
