using AutoMapper;
using ClashRoyaleApi.DTOs.River_Race_Season_Log;
using ClashRoyaleApi.Models.DbModels;

namespace ClashRoyaleApi
{
    public class AutoMapperProfile : Profile
    {

        public AutoMapperProfile() 
        { 
            CreateMap<DbRiverRaceLog, GetRiverRaceSeasonLogDTO>();
               
        }  



    }
}
