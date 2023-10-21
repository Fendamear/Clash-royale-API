using AutoMapper;
using ClashRoyaleApi.Data;
using ClashRoyaleApi.DTOs.River_Race_Season_Log;
using ClashRoyaleApi.Logic.Logging.LoggingModels;
using ClashRoyaleApi.Models.CurrentRiverRace;
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
        private readonly IMapper _Mapper;
        private readonly DataContext _dataContext;    

        public CurrentRiverRaceLogic (IConfiguration configuration, DataContext context, IMapper mapper)
        {
            _configuration = configuration;
            _dataContext = context;
            _Mapper = mapper;
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

        public async Task<CurrentRiverRaceLog> CurrentRiverRaceScheduler(SchedulerTime time)
        {
            CurrentRiverRaceLog res = new CurrentRiverRaceLog();    
            try
            {
                string response = await RoyaleApiCall();
                var log = JsonConvert.DeserializeObject<Root>(response);

                PeriodType type = (PeriodType)Enum.Parse(typeof(PeriodType), log.periodType, true);

                int dayOfWeek = GetDayOfWeek();
                int seasonId = GetSeasonId(log.sectionIndex, type);
                string mergedString = seasonId + "." + log.sectionIndex + "." + dayOfWeek;

                res.DayId = dayOfWeek;
                res.SeasonId = seasonId;
                res.SectionId = log.sectionIndex;
                res.SchedulerTime = time.ToString();

                if (!_dataContext.CurrentRiverRace.Any(x => x.SeasonId == seasonId && x.SectionId == log.sectionIndex && x.DayId == dayOfWeek))
                {
                    foreach (var item in log.clan.Participants)
                    {
                        DbCurrentRiverRace race = new DbCurrentRiverRace()
                        {
                            Guid = Guid.NewGuid(),
                            SeasonId = seasonId,
                            SectionId = log.sectionIndex,
                            DayId = dayOfWeek,
                            SeasonSectionDay = mergedString,
                            Tag = item.Tag,
                            Name = item.Name,
                            Fame = item.Fame,
                            DecksUsedToday = item.DecksUsedToday,
                            DecksNotUsed = 4 - item.DecksUsedToday,
                            Schedule = time,
                        };
                        _dataContext.CurrentRiverRace.Add(race);
                    }
                    _dataContext.SaveChanges();
                }
                else
                {
                    var lastRecord = _dataContext.CurrentRiverRace.OrderBy(x => x.SeasonId == seasonId && x.SectionId == log.sectionIndex && x.DayId == dayOfWeek).FirstOrDefault();

                    if (lastRecord == null) throw new Exception("this river race day does not exist");
                    if (time > lastRecord.Schedule)
                    {

                        List<DbCurrentRiverRace> currentRiverRace = _dataContext.CurrentRiverRace.Where(x => x.SeasonId == seasonId && x.SectionId == log.sectionIndex && x.DayId == dayOfWeek).ToList();

                        foreach (var item in log.clan.Participants)
                        {
                            DbCurrentRiverRace race = currentRiverRace.FirstOrDefault(x => x.Tag == item.Tag);

                            if (race == null) continue;

                            race.Fame = item.Fame;
                            race.DecksNotUsed = 4 - item.DecksUsedToday;
                            race.DecksUsedToday = item.DecksUsedToday;
                            race.Schedule = time;
                            _dataContext.CurrentRiverRace.Update(race);
                        }
                        _dataContext.SaveChanges();
                    }
                }
                res.Exception = "event schedule successfull";
                res.Status = Status.SUCCES;
                res.TimeStamp = DateTime.Now;
                return res;
            }
            catch (Exception ex)
            {
                res.Exception = ex.Message;
                res.Status = Status.FAILED;
                res.TimeStamp = DateTime.Now;
                return res;
            }
        }

        private async Task<string> RoyaleApiCall()
        {
            string apiUrl = _configuration.GetSection("RoyaleAPI:HttpAdressCurrentRiverRace").Value!;
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

            if(lastRecord == null)
            {
                DbRiverRaceLog log = new DbRiverRaceLog()
                {
                    Guid = Guid.NewGuid(),
                    SeasonId = lastRecord.SeasonId,
                    SectionId = sectionId,
                    TimeStamp = DateTime.Now,
                    Type = type
                };
                _dataContext.RiverRaceLogs.Add(log);     
                _dataContext.SaveChanges();
                lastRecord = log;
            }

            if (lastRecord.SectionId == sectionId) return lastRecord.SeasonId;
            if (type == PeriodType.TRAINING) throw new Exception("this method was called at a training day");

            if(lastRecord.Type == PeriodType.COLLOSEUM)
            {
                //write to database, sectionID + 1
                int newSeasonID = lastRecord.SeasonId + 1;

                _dataContext.Add(new DbRiverRaceLog()
                {
                    Guid = Guid.NewGuid(),
                    SectionId = sectionId,
                    SeasonId = newSeasonID,
                    TimeStamp = DateTime.Now,
                    Type = type
                });
                _dataContext.SaveChanges();
                return newSeasonID;
            }
            else
            {
                //write to database with current section ID
                _dataContext.Add(new DbRiverRaceLog()
                {
                    Guid = Guid.NewGuid(),
                    SectionId = sectionId,
                    SeasonId = lastRecord.SeasonId,
                    TimeStamp = DateTime.Now,
                    Type = type
                });
                _dataContext.SaveChanges();
                return lastRecord.SeasonId;
            }
        }
    }
}
