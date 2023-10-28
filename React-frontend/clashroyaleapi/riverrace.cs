public async Task<Response> CurrentRiverRaceScheduler(SchedulerTime time)
        {
            Response res = new Response();
            try
            {
                string response = await RoyaleApiCall();
                var log = JsonConvert.DeserializeObject<Root>(response);

                PeriodType type = (PeriodType)Enum.Parse(typeof(PeriodType), log.periodType, true);

                int dayOfWeek = GetDayOfWeek();
                int seasonId = GetSeasonId(log.sectionIndex, type);
                string mergedString = seasonId + "." + log.sectionIndex + "." + dayOfWeek;

                res.log.DayId = dayOfWeek;
                res.log.SeasonId = seasonId;
                res.log.SectionId = log.sectionIndex;
                res.log.SchedulerTime = time;

                if (!_dataContext.CurrentRiverRace.Any(x => x.SeasonId == seasonId && x.SectionId == log.sectionIndex && x.DayId == dayOfWeek))
                {
                    foreach (var item in log.clan.Participants)
                    {
                        int DecksNotUsed = 4 - item.DecksUsedToday;
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
                            DecksNotUsed = DecksNotUsed,
                            Schedule = time,
                        };
                        _dataContext.CurrentRiverRace.Add(race);
                        res.nrOfAttacksRemaining.Add(new NrOfAttacksRemaining(item.Tag, item.Name, DecksNotUsed));
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

                            int DecksNotUsed = 4 - item.DecksUsedToday;

                            race.Fame = item.Fame;
                            race.DecksNotUsed = DecksNotUsed;
                            race.DecksUsedToday = item.DecksUsedToday;
                            race.Schedule = time;
                            _dataContext.CurrentRiverRace.Update(race);
                            res.nrOfAttacksRemaining.Add(new NrOfAttacksRemaining(item.Tag, item.Name, DecksNotUsed));
                        }
                        _dataContext.SaveChanges();
                    }
                }
                res.log.Status = Status.SUCCES;
                res.log.TimeStamp = DateTime.Now;
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

            if (lastRecord == null) throw new Exception("River race log not found");
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