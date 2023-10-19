﻿using System.ComponentModel.DataAnnotations;
using static ClashRoyaleApi.Models.EnumClass;

namespace ClashRoyaleApi.Logic.Logging.LoggingModels
{
    public class CurrentRiverRaceLog
    {
        [Key]
        public Guid Guid { get; set; }

        public int SeasonId { get; set; }

        public int SectionId { get; set; }

        public int DayId { get; set; }
        
        public Status Status { get; set;}

        public DateTime TimeStamp { get; set; }

        public string? SchedulerTime { get; set; }

        public string? Exception { get; set; }

        public CurrentRiverRaceLog()
        {
            Guid = Guid.NewGuid();    
        }

        public CurrentRiverRaceLog(string ex)
        {
            TimeStamp = DateTime.Now;
            Exception = ex;
            Status = Status.FAILED;
        }
    }
}
