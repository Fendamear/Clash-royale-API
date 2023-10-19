using ClashRoyaleApi.Data;
using ClashRoyaleApi.Logic.Logging.LoggingModels;
using ClashRoyaleApi.Models.CurrentRiverRace;
using ClashRoyaleApi.Models.DbModels;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using static ClashRoyaleApi.Models.EnumClass;

namespace ClashRoyaleApi.Logic.CurrentRiverRace
{
    public class CurrentRiverRace : ICurrentRiverRace
    {

        private readonly IConfiguration _configuration;
        private readonly DataContext _dataContext;    

        public CurrentRiverRace (IConfiguration configuration, DataContext context)
        {
            _configuration = configuration;
            _dataContext = context;
        }

        public async Task<Root> GetCurrentRiverRace()
        {
            string response = RoyaleApiCall();
            var log = JsonConvert.DeserializeObject<Root>(response);
            return log;
        }

        public CurrentRiverRaceLog CurrentRiverRaceScheduler(SchedulerTime time)
        {
            CurrentRiverRaceLog res = new CurrentRiverRaceLog();    
            try
            {
                string response = RoyaleApiCall();
                var log = JsonConvert.DeserializeObject<Root>(response);

                PeriodType type = (PeriodType)Enum.Parse(typeof(PeriodType), log.periodType, true);

                int dayOfWeek = GetDayOfWeek();
                int seasonId = GetSeasonId(log.sectionIndex, type);
                string mergedString = seasonId + "." + log.sectionIndex + "." + dayOfWeek;

                res.DayId = dayOfWeek;
                res.SeasonId = seasonId;
                res.SectionId = log.sectionIndex;
                res.SchedulerTime = time.ToString();
                res.TimeStamp = DateTime.Now;

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
                return res;
            }
            catch (Exception ex)
            {
                res.Exception = ex.Message;
                res.Status = Status.FAILED;
                return res;
            }
        }

        private string RoyaleApiCall()
        {
            return File.ReadAllText("./currenrriverrace.json");

            string apiUrl = _configuration.GetSection("RoyaleAPI:HttpAdressClanInfo").Value!;
            string accessToken = _configuration.GetSection("RoyaleAPI:AccessToken").Value!;

            //using (HttpClient httpClient = new HttpClient())
            //{
            //    httpClient.BaseAddress = new Uri(apiUrl);

            //    httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            //    try
            //    {
            //        HttpResponseMessage response = httpClient.GetAsync(apiUrl);

            //        if (response.IsSuccessStatusCode)
            //        {
            //            return response.Content.ReadAsStringAsync();
            //        }
            //        else
            //        {
            //            throw new Exception("failed with status " + response.StatusCode);
            //        }
            //    }
            //    catch (Exception ex)
            //    {
            //        throw;
            //    }
        
            
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

        private int GetSeasonId(int sectionId, PeriodType type)
        {
            var lastRecord = _dataContext.RiverRaceLogs.OrderByDescending(t => t.timeStamp).FirstOrDefault();

            if(lastRecord == null)
            {
                _dataContext.RiverRaceLogs.Add(new DbRiverRaceLog()
                {
                    Guid = Guid.NewGuid(),
                    SeasonId = 100,
                    SectionId = sectionId,
                    timeStamp = DateTime.Now,
                    type = type
                });
                _dataContext.SaveChanges();
                lastRecord = _dataContext.RiverRaceLogs.OrderByDescending(t => t.timeStamp).FirstOrDefault();
            }

            if (lastRecord.SectionId == sectionId) return lastRecord.SeasonId;
            if (type == PeriodType.TRAINING) throw new Exception("this method was called at a training day");

            if(lastRecord.type == PeriodType.COLLOSEUM)
            {
                //write to database, sectionID + 1
                int newSeasonID = lastRecord.SeasonId + 1;

                _dataContext.Add(new DbRiverRaceLog()
                {
                    Guid = Guid.NewGuid(),
                    SectionId = sectionId,
                    SeasonId = newSeasonID,
                    timeStamp = DateTime.Now,
                    type = type
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
                    timeStamp = DateTime.Now,
                    type = type
                });
                _dataContext.SaveChanges();
                return lastRecord.SeasonId;
            }
        }
    }
}
