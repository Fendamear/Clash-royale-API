using System.Net.Http.Headers;

namespace ClashRoyaleApi.Logic.RoyaleApi
{
    public class HttpClientWrapper : IHttpClientWrapper
    {
        private readonly HttpClient httpClient;

        public HttpClientWrapper(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }

        public Uri BaseAdress { get => httpClient.BaseAddress; set => httpClient.BaseAddress = value; }

        public void AddDefaultRequestHeader(string name, string value)
        {
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(name, value);
        }

        public void ClearDefaultRequestHeaders()
        {
            httpClient.DefaultRequestHeaders.Clear();
        }

        public async Task<HttpResponseMessage> GetAsync(string requestUri)
        {
            return await httpClient.GetAsync(requestUri);
        }
    }
}
