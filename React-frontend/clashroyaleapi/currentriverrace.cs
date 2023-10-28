        public async Task<Response> CurrentRiverRaceScheduler(SchedulerTime time)
        {
            Response res = new Response();
            try
            {
                string response = await RoyaleApiCall();
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
                    AddRiverRaceData(log, seasonId, dayOfWeek, time, res);
                }
                else
                {
                    UpdateExistingRiverRaceData(log, seasonId, dayOfWeek, time, res);
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

        private bool RiverRaceExists(int seasonId, int sectionIndex, int dayOfWeek)
        {
            return _dataContext.CurrentRiverRace.Any(x => x.SeasonId == seasonId && x.SectionId == sectionIndex && x.DayId == dayOfWeek);
        }

        private void AddRiverRaceData(Root log, int seasonId, int dayOfWeek, SchedulerTime time, Response res)
        {
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
                //_dataContext.CurrentRiverRace.Add(race);
                res.nrOfAttacksRemaining.Add(new NrOfAttacksRemaining(item.Tag, item.Name, decksNotUsed));
            }
            _dataContext.SaveChanges();
        }

        private void UpdateExistingRiverRaceData(Root log, int seasonId, int dayOfWeek, SchedulerTime time, Response res)
        {
            var lastRecord = _dataContext.CurrentRiverRace.OrderBy(x => x.SeasonId == seasonId && x.SectionId == log.sectionIndex && x.DayId == dayOfWeek).FirstOrDefault();

            if (lastRecord == null) throw new Exception("This river race day does not exist");

            if (time > lastRecord.Schedule)
            {
                var currentRiverRace = _dataContext.CurrentRiverRace.Where(x => x.SeasonId == seasonId && x.SectionId == log.sectionIndex && x.DayId == dayOfWeek).ToList();

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
                    res.nrOfAttacksRemaining.Add(new NrOfAttacksRemaining(item.Tag, item.Name, decksNotUsed));
                }
                _dataContext.SaveChanges();
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