namespace ClashRoyaleApi.Logic.RoyaleApi
{
    public interface IHttpClientWrapper
    {
        Task<HttpResponseMessage> GetAsync(string requestUri);

        Uri BaseAdress { get; set; }

        void AddAuthorizationRequestHeader(string name, string value);

        void ClearDefaultRequestHeaders();

        Task<string> RoyaleApiCall(string apiUrl, string accesstoken);

    }
}
