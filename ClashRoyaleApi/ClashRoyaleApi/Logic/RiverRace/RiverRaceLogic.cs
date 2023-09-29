using ClashRoyaleApi.Data;
using ClashRoyaleApi.Models.DbModels;
using ClashRoyaleApi.Models.JsonModels;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Security.Cryptography.X509Certificates;
using System.Text.Json;

namespace ClashRoyaleApi.Logic.RiverRace
{
    public class RiverRaceLogic : IRiverRaceLogic
    {
        static HttpClient client = new HttpClient();
        private readonly DataContext _dataContext;
        private readonly IConfiguration _configuration;

        public RiverRaceLogic(DataContext context, IConfiguration configuration)        
        { 
              _dataContext = context;
              _configuration = configuration;
        }

        public async Task<Root> GetRiverRaceLog(int limit) 
        {
            string response = await ClashRoyaleApiCall(limit);
            var log = JsonConvert.DeserializeObject<Root>(response);
            string herresClanTag = _configuration.GetSection("Clan:ClanTag").Value!;

            foreach( var item in log.Items ) 
            {
                int seasonId = item.SeasonId;
                int sectionId = item.SectionIndex;
                string? createdAt = item.CreatedDate;
                 
                bool riverRaceClan = DoesSeasonAndSectionIndexExistClan(seasonId, sectionId);
                bool riverRaceParticipant = DoesSeasonAndSectionIndexExistParticipants(seasonId, sectionId);
                List<DbRiverRaceClan> clans = new List<DbRiverRaceClan>();

                foreach (var standing in item.Standings ) 
                {
                    int rank = standing.Rank;
                    int trophyChange = standing.TrophyChange;
                    string? clanTag = standing.Clan.Tag;

                    if (!riverRaceParticipant && clanTag == herresClanTag)
                    {
                        AddRiverRaceParticipants(standing.Clan, seasonId, sectionId, createdAt);
                    }

                    if (riverRaceClan) continue;

                    string season = seasonId.ToString();
                    string mergedString = season + "." + sectionId.ToString();

                    clans.Add(new DbRiverRaceClan()
                    {
                        Id = Guid.NewGuid(),
                        SeasonId = seasonId,
                        SectionIndex = sectionId,
                        SeasonSectionIndex = mergedString,
                        CreatedDate = createdAt,
                        Rank = rank,
                        Tag = standing.Clan.Tag,
                        Name = standing.Clan.Name,
                        Fame = standing.Clan.Fame,
                        TrophyChange = trophyChange,
                        NewTrophies = standing.Clan.clanScore + trophyChange
                    });
                }
                _dataContext.RiverRaceClan.AddRange(clans);
                _dataContext.SaveChanges();
            }
            return log;              
        }

        private async Task<string> ClashRoyaleApiCall(int limit)
        {
            string apiUrl = _configuration.GetSection("RoyaleAPI:HttpAdress").Value! + limit;
            string accessToken = _configuration.GetSection("RoyaleAPI:AccessToken").Value!;

            using (HttpClient httpClient = new HttpClient())
            {
                httpClient.BaseAddress = new Uri(apiUrl);

                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

                try
                {
                    HttpResponseMessage response = await httpClient.GetAsync(apiUrl);

                    if(response.IsSuccessStatusCode) 
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

        private bool DoesSeasonAndSectionIndexExistParticipants(int seasonId, int sectionId)
        {
            try
            {
                if (_dataContext.RiverRaceParticipant.Any(p => p.SeasonId == seasonId && p.SectionIndex == sectionId)) return true;
            }
            catch (Exception)
            {

                throw;
            }
            return false;
        }

        private bool DoesSeasonAndSectionIndexExistClan(int seasonId, int sectionId)
        {
            try
            {
                if (_dataContext.RiverRaceClan.Any(p => p.SeasonId == seasonId && p.SectionIndex == sectionId)) return true;
            }
            catch (Exception)
            {

                throw;
            }
            return false;
        }


        private bool AddRiverRaceParticipants(RiverRaceClan clan, int seasonId, int sectionId, string createdAt)
        {
            string season = seasonId.ToString();
            string mergedString = season + "." + sectionId.ToString();

            try
            {
                List<DbRiverRaceParticipant> participants = new List<DbRiverRaceParticipant>();
                int nrOfAttacks = int.Parse(_configuration.GetSection("Clan:NrOfAttacks").Value!);

                foreach (var participant in clan.Participants)
                {
                    participants.Add(new DbRiverRaceParticipant()
                    {
                        SeasonId = seasonId,
                        SectionIndex = sectionId,
                        SeasonSectionIndex = mergedString,
                        CreatedDate = createdAt,
                        Tag = participant.Tag,
                        Name = participant.Name,
                        Fame = participant.Fame,
                        RepairPoints = participant.RepairPoints,
                        BoatAttacks = participant.BoatAttacks,
                        DecksUsed = DecksUsed(participant.DecksUsed),
                        DecksUsedToday = participant.DecksUsedToday,
                        DecksNotUsed = DecksNotUsed(nrOfAttacks - participant.DecksUsed)
                    }) ;
                }

                _dataContext.RiverRaceParticipant.AddRange(participants);
                _dataContext.SaveChanges();
            }
            catch (Exception)
            {
                return false;
            }        
            return true;
        }

        private int DecksNotUsed(int decksUsed)
        {
            if (decksUsed > 0) return decksUsed;
            return 0;
        }

        private int DecksUsed(int decksUsed)
        {
            int nrOfAttacks = int.Parse(_configuration.GetSection("Clan:NrOfAttacks").Value!);
            if (decksUsed > nrOfAttacks) return nrOfAttacks;
            return decksUsed;
        }



    }
}
