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
using ClashRoyaleApi.Logic.RoyaleApi;
using static ClashRoyaleApi.Models.EnumClass;
using ClashRoyaleApi.DTOs.Current_River_Race;
using ClashRoyaleApi.Models;

namespace ClashRoyaleApi.Logic.CurrentRiverRace
{
    public class CurrentRiverRaceLogic : ICurrentRiverRace
    {

        private readonly IConfiguration _configuration;
        private readonly DataContext _dataContext;   
        private readonly IHttpClientWrapper _httpClientWrapper;

        public CurrentRiverRaceLogic (IConfiguration configuration, DataContext context, IHttpClientWrapper wrapper)
        {
            _configuration = configuration;
            _dataContext = context;
            _httpClientWrapper = wrapper;
        }

        public CurrentRiverRaceLogic(DataContext context)
        {
            _dataContext = context;
        }

        //retrieving current river race logic

        public async Task<Root> GetCurrentRiverRace()
        {
            string response = await _httpClientWrapper.RoyaleApiCall(RoyaleApiType.CURRENTRIVERRACE);
            var log = JsonConvert.DeserializeObject<Root>(response);
            return log;
        }

        public List<CurrentRiverRaceSeasonDTO> GetCurrentRiverRace(int seasonId, int secId, int dayId)
        {
            List<DbCurrentRiverRace> riverRace = _dataContext.CurrentRiverRace.Where(x => x.SeasonId == seasonId).ToList();
            List<CurrentRiverRaceSeasonDTO> response = new List<CurrentRiverRaceSeasonDTO>();
            List<DbRiverRaceLog> log = _dataContext.RiverRaceLogs.Where(x => x.SeasonId == seasonId).ToList();

            foreach (string tag in riverRace.Select(x => x.Tag).Distinct())
            {
                List<DbCurrentRiverRace> member = riverRace.Where(x => x.Tag == tag).ToList();
                CurrentRiverRaceSeasonDTO dto = new CurrentRiverRaceSeasonDTO();
                string name = member.FirstOrDefault().Name;

                dto.DateIdentifier = $"Season {seasonId}";
                //dto.Time = member.Where(x => x.SectionId == 0 && x.DayId == 0).FirstOrDefault().TimeStamp;
                dto.Name = name;
                int fame = 0;
                dto.DecksUsed = member.Sum(x => x.DecksUsedToday);
                dto.DecksNotUsed = member.Sum(x => x.DecksNotUsed);

                List<CurrentRiverRaceSectionDTO> sections = new List<CurrentRiverRaceSectionDTO>();
                //ember.Select(x => x.SectionId).Distinct();

                foreach (int sectionId in secId < 0
                ? member.Select(x => x.SectionId).Distinct()
                : new[] { member.FirstOrDefault(x => x.SectionId == secId)?.SectionId ?? secId })
                {
                    CurrentRiverRaceSectionDTO section = new CurrentRiverRaceSectionDTO();

                    List<DbCurrentRiverRace> week = member.Where(x => x.SectionId == sectionId).ToList();
                    List<CurrentRiverRaceDayDTO> days = new List<CurrentRiverRaceDayDTO>();

                    if (week.Count == 0) continue;

                    section.Fame = fame;
                    section.Time = week.FirstOrDefault().TimeStamp;
                    section.DateIdentifier = $"Week {sectionId + 1}";
                    section.DecksNotUsed = member.Where(x => x.SectionId == sectionId).Sum(x => x.DecksNotUsed);
                    section.DecksUsed = member.Where(x => x.SectionId == sectionId).Sum(x => x.DecksUsedToday);

                    int dayFame = 0;

                    fame += week.Max(x => x.Fame);
                  
                    foreach (DbCurrentRiverRace entry in dayId < 0 ? week : week.Where(x => x.DayId == dayId).ToList()) 
                    {               
                        CurrentRiverRaceDayDTO day = new CurrentRiverRaceDayDTO();

                        day.DecksUsed = entry.DecksUsedToday;
                        day.DecksNotUsed = entry.DecksNotUsed;
                        day.DateIdentifier = GetDay(entry.DayId);
                        day.Time = entry.TimeStamp;
                        day.Fame = entry.Fame - dayFame;

                        dayFame = entry.Fame;
                        days.Add(day);  
                    } 
                    section.subRows = days;
                    sections.Add(section);
                }
                dto.Fame = fame;
                dto.subRows = sections;
                response.Add(dto);
            }
            return response;
        }

        private string GetDay(int dayId)
        {
            switch(dayId) 
            { 
                case 0:
                    return "Thursday";
                case 1:
                    return "Friday";
                case 2:
                    return "Saturday";
                case 3:
                    return "Sunday";
                default:
                    throw new Exception("illegal day provided");                   
            }
        }

        //river race logs

        public async Task<List<GetRiverRaceSeasonLogDTO>> GetRiverRaceSeasonLog()
        {
            List<DbRiverRaceLog> log = await _dataContext.RiverRaceLogs.OrderByDescending(t => t.TimeStamp).ToListAsync();
            List<GetRiverRaceSeasonLogDTO> response = new List<GetRiverRaceSeasonLogDTO>();

            foreach (var logItem in log) 
            {
                response.Add(new GetRiverRaceSeasonLogDTO()
                {
                    Guid = logItem.Guid.ToString(),
                    TimeStamp = logItem.TimeStamp,
                    SeasonId = logItem.SeasonId,
                    SectionId = $"Week {logItem.SectionId + 1}",
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

        public async Task<bool> DeleteRiverRaceLog(string id)
        {
            Guid guid;
            if (!Guid.TryParse(id, out guid))
            {
                throw new FormatException("Id is not a GUID Id");
            }

            DbRiverRaceLog log = await _dataContext.RiverRaceLogs.Where(x => x.Guid == guid).FirstOrDefaultAsync();
            if (log == null) return false;
            _dataContext.RiverRaceLogs.Remove(log);
            await _dataContext.SaveChangesAsync();
            return true;
        }

        public DbRiverRaceLog GetRRSeasonLogBySeasonAndSectionID(int seasonId, int sectionId)
        {
            return _dataContext.RiverRaceLogs.Where(x => x.SeasonId == seasonId && x.SectionId == sectionId).FirstOrDefault();
        }

        //current river race logic

        public async Task<Response> CurrentRiverRaceScheduler(SchedulerTime time)
        {
            Response res = new Response();
            try
            {
                string response = await _httpClientWrapper.RoyaleApiCall(RoyaleApiType.CURRENTRIVERRACE);
                //string response = await _httpClientWrapper.RoyaleApiCall();
                //string response = File.ReadAllText("./currenrriverrace.json");
                var log = JsonConvert.DeserializeObject<Root>(response);


                if (log == null) throw new ArgumentNullException("the api call did not provide any data");

                PeriodType type = (PeriodType)Enum.Parse(typeof(PeriodType), log.periodType, true);

                int dayOfWeek = GetDayOfWeek();
                int seasonId = GetSeasonId(log.sectionIndex, type);

                res.log = new CurrentRiverRaceLog()
                {
                    DayId = dayOfWeek,
                    SeasonId = seasonId,
                    SectionId = log.sectionIndex,
                    SchedulerTime = time
                };

                RemoveInactiveMembers(log);
           
                if (!RiverRaceExists(seasonId, log.sectionIndex, dayOfWeek))
                {
                    res.nrOfAttacksRemaining = AddRiverRaceData(log, seasonId, dayOfWeek, time);
                }
                else
                {
                    res.nrOfAttacksRemaining = UpdateExistingRiverRaceData(log, seasonId, dayOfWeek, time);
                }
                res.log.TimeStamp = DateTime.Now;
                res.log.Status = Status.SUCCES;
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
                    Schedule = time,
                    TimeStamp = DateTime.Now               
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
                race.TimeStamp = DateTime.Now;
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

            if(lastRecord.Type == PeriodType.COLOSSEUM)
            {
                //write to database, seasonID + 1
                int newSeasonID = lastRecord.SeasonId + 1;
                log.SeasonId = newSeasonID;

                _dataContext.Add(log);
                _dataContext.SaveChanges();
                return newSeasonID;
            }

            //write to database with current section ID
            log.SeasonId = lastRecord.SeasonId;

            _dataContext.Add(log);
            _dataContext.SaveChanges();
            return lastRecord.SeasonId;
        }

        private void RemoveInactiveMembers(Root log)
        {
            List<Participant> toBeRemoved = new List<Participant>();

            foreach (Participant participant in log.clan.Participants)
            {
                DbClanMembers member = _dataContext.DbClanMembers.Where(x => x.ClanTag == participant.Tag).FirstOrDefault();

                if (member == null) continue;
                if (!member.IsInClan) toBeRemoved.Add(participant);
            }

            if (toBeRemoved.Count > 0)
            {
                foreach (Participant participant in toBeRemoved)
                {
                    log.clan.Participants.Remove(participant);
                }
            }
        }
    }
}
