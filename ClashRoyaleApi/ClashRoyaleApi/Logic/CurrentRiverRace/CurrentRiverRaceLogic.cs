using AutoMapper;
using ClashRoyaleApi.Data;
using ClashRoyaleApi.DTOs.River_Race_Season_Log;
using ClashRoyaleApi.Logic.RoyaleApi;
using ClashRoyaleApi.Models.CurrentRiverRace;
using ClashRoyaleApi.Models.CurrentRiverRace.CRR_Response;
using ClashRoyaleApi.Models.DbModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using static ClashRoyaleApi.Models.EnumClass;

namespace ClashRoyaleApi.Logic.CurrentRiverRace
{
    public class CurrentRiverRaceLogic : ICurrentRiverRace
    {

        private readonly IConfiguration _configuration;
        private readonly DataContext _dataContext;    

        public CurrentRiverRaceLogic (IConfiguration configuration, DataContext context)
        {
            _configuration = configuration;
            _dataContext = context;
        }

        public CurrentRiverRaceLogic(DataContext context)
        {
            _dataContext = context;
        }

        public async Task<Root> GetCurrentRiverRace()
        {
            string response = await RoyaleApiCall();
            var log = JsonConvert.DeserializeObject<Root>(response);
            return log;
        }

        public async Task<List<GetRiverRaceSeasonLogDTO>> GetRiverRaceSeasonLog()
        {
            List<DbRiverRaceLog> log = await _dataContext.RiverRaceLogs.OrderByDescending(t => t.TimeStamp).ToListAsync();
            List<GetRiverRaceSeasonLogDTO> response = new List<GetRiverRaceSeasonLogDTO>();

            foreach (var logItem in log) 
            {
                response.Add(new GetRiverRaceSeasonLogDTO()
                {
                    TimeStamp = logItem.TimeStamp,
                    SeasonId = logItem.SeasonId,
                    SectionId = logItem.SectionId,
                    type = logItem.Type,
                });        
            }
            return response;
        }

        public async Task PostRiverRaceLog(PostRiverRaceLogDTO post)
        {
            if (await _dataContext.RiverRaceLogs.AnyAsync(x => x.SeasonId == post.SeasonId && x.SectionId == post.SectionId)) throw new Exception("this log already exists");

            DbRiverRaceLog log = new DbRiverRaceLog()
            {
                Guid = Guid.NewGuid(),
                TimeStamp = DateTime.Now,
                SeasonId = post.SeasonId,
                SectionId = post.SectionId, 
                Type = post.type,
            };
            _dataContext.RiverRaceLogs.Add(log);
            await _dataContext.SaveChangesAsync();
        }

        public async Task<bool> DeleteRiverRaceLog(int seasonId, int sectionId)
        {
            DbRiverRaceLog log = await _dataContext.RiverRaceLogs.Where(x => x.SeasonId == seasonId && x.SectionId == sectionId).FirstOrDefaultAsync();
            if (log == null) return false;
            _dataContext.RiverRaceLogs.Remove(log);
            await _dataContext.SaveChangesAsync();
            return true;
        }

        public DbRiverRaceLog GetRRSeasonLogBySeasonAndSectionID(int seasonId, int sectionId)
        {
            return _dataContext.RiverRaceLogs.Where(x => x.SeasonId == seasonId && x.SectionId == sectionId).FirstOrDefault();
        }


        public async Task<Response> CurrentRiverRaceScheduler(SchedulerTime time)
        {
            Response res = new Response();
            try
            {
                string response = await RoyaleApiCall();
                //string response = await _httpClientWrapper.RoyaleApiCall();
                //string response = File.ReadAllText("./currenrriverrace.json");
                var log = JsonConvert.DeserializeObject<Root>(response);

                if (log == null) throw new ArgumentNullException("the api call did not provide any data");

                PeriodType type = (PeriodType)Enum.Parse(typeof(PeriodType), log.periodType, true);

                int dayOfWeek = GetDayOfWeek();
                int seasonId = GetSeasonId(log.sectionIndex, type);

                var mergedString = $"{seasonId}.{log.sectionIndex}.{dayOfWeek}";

                res.log = new CurrentRiverRaceLog()
                {
                    DayId = dayOfWeek,
                    SeasonId = seasonId,
                    SectionId = log.sectionIndex,
                    SchedulerTime = time
                };

                if (!RiverRaceExists(seasonId, log.sectionIndex, dayOfWeek))
                {
                    AddRiverRaceData(log, seasonId, dayOfWeek, time);
                }
                else
                {
                    UpdateExistingRiverRaceData(log, seasonId, dayOfWeek, time);
                }

                return res;
            }
            catch (Exception ex)
            {
                res.Exception = ex;
                res.log.Status = Status.FAILED;
                res.log.TimeStamp = DateTime.Now;
                return res;
            }
        }

        public bool RiverRaceExists(int seasonId, int sectionIndex, int dayOfWeek)
        {
            return _dataContext.CurrentRiverRace.Any(x => x.SeasonId == seasonId && x.SectionId == sectionIndex && x.DayId == dayOfWeek);
        }

        public List<NrOfAttacksRemaining> AddRiverRaceData(Root log, int seasonId, int dayOfWeek, SchedulerTime time)
        {
            List<NrOfAttacksRemaining> attacksRemainings = new List<NrOfAttacksRemaining>();
            foreach (var item in log.clan.Participants)
            {
                int decksNotUsed = 4 - item.DecksUsedToday;
                var race = new DbCurrentRiverRace
                {
                    Guid = Guid.NewGuid(),
                    SeasonId = seasonId,
                    SectionId = log.sectionIndex,
                    DayId = dayOfWeek,
                    SeasonSectionDay = $"{seasonId}.{log.sectionIndex}.{dayOfWeek}",
                    Tag = item.Tag,
                    Name = item.Name,
                    Fame = item.Fame,
                    DecksUsedToday = item.DecksUsedToday,
                    DecksNotUsed = decksNotUsed,
                    Schedule = time
                };
                _dataContext.CurrentRiverRace.Add(race);
                attacksRemainings.Add(new NrOfAttacksRemaining(item.Tag, item.Name, decksNotUsed));
            }
            _dataContext.SaveChanges();
            return attacksRemainings;
        }

        public List<NrOfAttacksRemaining> UpdateExistingRiverRaceData(Root log, int seasonId, int dayOfWeek, SchedulerTime time)
        {
            //var lastRecord = _dataContext.CurrentRiverRace.OrderBy(x => x.SeasonId == seasonId && x.SectionId == log.sectionIndex && x.DayId == dayOfWeek).FirstOrDefault();
            var currentRiverRace = _dataContext.CurrentRiverRace.Where(x => x.SeasonId == seasonId && x.SectionId == log.sectionIndex && x.DayId == dayOfWeek).ToList();

            if (currentRiverRace.Count == 0) throw new ArgumentNullException("This river race day does not exist");

            List<NrOfAttacksRemaining> attacksRemainings = new List<NrOfAttacksRemaining>();
            DbCurrentRiverRace lastRecord = currentRiverRace.FirstOrDefault();

            if (time < lastRecord.Schedule) return attacksRemainings;
           
            foreach (var item in log.clan.Participants)
            {
                var race = currentRiverRace.FirstOrDefault(x => x.Tag == item.Tag);

                if (race == null) continue;

                int decksNotUsed = 4 - item.DecksUsedToday;

                race.Fame = item.Fame;
                race.DecksNotUsed = decksNotUsed;
                race.DecksUsedToday = item.DecksUsedToday;
                race.Schedule = time;
                _dataContext.CurrentRiverRace.Update(race);
                attacksRemainings.Add(new NrOfAttacksRemaining(item.Tag, item.Name, decksNotUsed));
            }
            _dataContext.SaveChanges();
            return attacksRemainings;

        }

        private static int GetDayOfWeek()
        {
            DayOfWeek dayOfWeek = DateTime.Today.DayOfWeek;

            switch (dayOfWeek)
            {
                case DayOfWeek.Friday:
                    return 0;
                case DayOfWeek.Saturday:
                    return 1;
                case DayOfWeek.Sunday:
                    return 2;
                case DayOfWeek.Monday:
                    return 3;
                default:
                    return -1; // Handle other days of the week as needed
            }
        }

        public int GetSeasonId(int sectionId, PeriodType type)
        {
            var lastRecord = _dataContext.RiverRaceLogs.OrderByDescending(t => t.TimeStamp).FirstOrDefault();

            if (lastRecord.SeasonId == 0 || lastRecord == null) throw new ArgumentNullException("River race log not found");
            if (lastRecord.SectionId == sectionId) return lastRecord.SeasonId;
            if (type == PeriodType.TRAINING) throw new Exception("this method was called at a training day");

            DbRiverRaceLog log = new DbRiverRaceLog()
            {
                Guid = Guid.NewGuid(),
                SectionId = sectionId,
                TimeStamp = DateTime.Now,
                Type = type
            };

            if(lastRecord.Type == PeriodType.COLLOSEUM)
            {
                //write to database, seasonID + 1
                int newSeasonID = lastRecord.SeasonId + 1;
                log.SeasonId = newSeasonID;

                _dataContext.Add(log);
                _dataContext.SaveChanges();
                return newSeasonID;
            }
            else
            {
                //write to database with current section ID
                log.SeasonId = lastRecord.SeasonId;

                _dataContext.Add(log);
                _dataContext.SaveChanges();
                return lastRecord.SeasonId;
            }
        }

        private async Task<string> RoyaleApiCall()
        {
            string apiUrl = _configuration.GetSection("RoyaleAPI:HttpAdressCurrentRiverRace").Value!;
            string accessToken = _configuration.GetSection("RoyaleAPI:AccessToken").Value!;

            HttpClient httpClient = new HttpClient();

            httpClient.BaseAddress = new Uri(apiUrl);
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            try
            {
                HttpResponseMessage response = httpClient.GetAsync(apiUrl).Result;

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
    }
}
