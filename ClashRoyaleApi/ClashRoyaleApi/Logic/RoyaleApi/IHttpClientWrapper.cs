namespace ClashRoyaleApi.Logic.RoyaleApi
{
    public interface IHttpClientWrapper
    {
        Task<HttpResponseMessage> GetAsync(string requestUri);

        Uri BaseAdress { get; set; }

        void AddDefaultRequestHeader(string name, string value);

        void ClearDefaultRequestHeaders();

    }
}
