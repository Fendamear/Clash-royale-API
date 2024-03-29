﻿using ClashRoyaleApi.DTOs.Current_River_Race;
using ClashRoyaleApi.DTOs.Current_River_Race.Homepage;
using ClashRoyaleApi.DTOs.Current_River_Race.Homepage.AllTime;
using ClashRoyaleApi.DTOs.River_Race_Season_Log;
using ClashRoyaleApi.Models.CurrentRiverRace;
using ClashRoyaleApi.Models.CurrentRiverRace.CRR_Response;
using ClashRoyaleApi.Models.DbModels;
using static ClashRoyaleApi.Models.EnumClass;

namespace ClashRoyaleApi.Logic.CurrentRiverRace
{
    public interface ICurrentRiverRace
    {
        Task<Root> GetCurrentRiverRace();

        List<CurrentRiverRaceSeasonDTO> GetCurrentRiverRace(int seasonId, int sectionId, int dayId);

        Task<Response> CurrentRiverRaceScheduler(SchedulerTime time);

        Task PostRiverRaceLog(PostRiverRaceLogDTO post);

        Task<List<GetRiverRaceSeasonLogDTO>> GetRiverRaceSeasonLog();

        Task<bool> DeleteRiverRaceLog(string id);

        int GetSeasonId(int sectionId, PeriodType type);

        List<NrOfAttacksRemaining> UpdateExistingRiverRaceData(Root log, int seasonId, int dayOfWeek, SchedulerTime time);

        List<NrOfAttacksRemaining> AddRiverRaceData(Root log, int seasonId, int dayOfWeek, SchedulerTime time);

        DbRiverRaceLog GetRRSeasonLogBySeasonAndSectionID(int seasonId, int sectionId);

        WeeklyDataDTO GetWeeklyData(DateTime startDate, DateTime endDate);

        AllTimeDataDTO GetAllTimeData();
    }
}
