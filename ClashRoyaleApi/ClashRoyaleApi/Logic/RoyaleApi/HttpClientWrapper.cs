using System.Net.Http.Headers;

namespace ClashRoyaleApi.Logic.RoyaleApi
{
    public class HttpClientWrapper : IHttpClientWrapper
    {
        private readonly HttpClient httpClient;
        private readonly IConfiguration _configuration;

        public HttpClientWrapper(HttpClient httpClient, IConfiguration configuration)
        {
            this.httpClient = httpClient;
            this._configuration = configuration;
        }

        public Uri BaseAdress { get => httpClient.BaseAddress; set => httpClient.BaseAddress = value; }

        public void AddAuthorizationRequestHeader(string name, string value)
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

        public async Task<string> RoyaleApiCall(string apiUrl, string accessToken)
        {
            using (HttpClient httpClient = new HttpClient())
            {
                httpClient.BaseAddress = new Uri(apiUrl);

                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

                try
                {
                    HttpResponseMessage response = await httpClient.GetAsync(apiUrl);

                    if (response.IsSuccessStatusCode)
                    {
                        string res = await response.Content.ReadAsStringAsync();
                        httpClient.Dispose();
                        return res;
                        
                    }
                    else
                    {
                        throw new Exception("failed with status " + response.StatusCode);
                    }
                }
                catch (Exception ex)
                {
                    throw;
                }
            }
        }


    }
}
