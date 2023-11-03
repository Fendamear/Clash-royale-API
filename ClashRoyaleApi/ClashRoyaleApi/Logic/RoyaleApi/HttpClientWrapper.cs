using Microsoft.Extensions.ObjectPool;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using static ClashRoyaleApi.Models.EnumClass;

namespace ClashRoyaleApi.Logic.RoyaleApi
{
    public class HttpClientWrapper : IHttpClientWrapper
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;

        public HttpClientWrapper(HttpClient httpClient, IConfiguration configuration)
        {
            this._httpClient = httpClient;
            this._configuration = configuration;
        }
        
        public virtual async Task<string> RoyaleApiCall(RoyaleApiType type)
        {
            string apiUrl = GetApiUrl(type);
            string accessToken = _configuration.GetSection("RoyaleAPI:AccessToken").Value!;

            _httpClient.BaseAddress = new Uri(apiUrl);  
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            try
            {
                HttpResponseMessage response = _httpClient.GetAsync(apiUrl).Result;

                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadAsStringAsync();
                }
                else
                {
                    throw new Exception("Failed with status " + response.StatusCode);
                }
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public virtual async Task<string> RoyaleApiCall(RoyaleApiType type, string[] variables)
        {
            throw new NotImplementedException();
        }

        private string GetApiUrl(RoyaleApiType type) 
        { 
            switch(type) 
            {
                case RoyaleApiType.CURRENTRIVERRACE :
                    return _configuration.GetSection("RoyaleAPI:HttpAdressCurrentRiverRace").Value!;
                case RoyaleApiType.CLANMEMBERINFO :
                    return _configuration.GetSection("RoyaleAPI:HttpAdressClanInfo").Value!;
                case RoyaleApiType.RIVERRACE:
                    return _configuration.GetSection("RoyaleAPI:HttpAdressRiverRace").Value!;
                default:
                    return string.Empty;
            }         
        }
    }
}
