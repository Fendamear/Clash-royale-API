using ClashRoyaleApi.Models.CurrentRiverRace;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System.Net.Http.Headers;

namespace ClashRoyaleApi.Logic.CurrentRiverRace
{
    public class CurrentRiverRace : ICurrentRiverRace
    {

        private readonly IConfiguration _configuration;
        public CurrentRiverRace (IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<Root> GetCurrentRiverRace()
        {
            string response = await RoyaleApiCall();
            var log = JsonConvert.DeserializeObject<Root>(response);
            return log;
        }

        private async Task<string> RoyaleApiCall()
        {
            return File.ReadAllText("./currenrriverrace.json");


            string apiUrl = _configuration.GetSection("RoyaleAPI:HttpAdressClanInfo").Value!;
            string accessToken = _configuration.GetSection("RoyaleAPI:AccessToken").Value!;

            using (HttpClient httpClient = new HttpClient())
            {
                httpClient.BaseAddress = new Uri(apiUrl);

                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

                try
                {
                    HttpResponseMessage response = await httpClient.GetAsync(apiUrl);

                    if (response.IsSuccessStatusCode)
                    {
                        return await response.Content.ReadAsStringAsync();
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
