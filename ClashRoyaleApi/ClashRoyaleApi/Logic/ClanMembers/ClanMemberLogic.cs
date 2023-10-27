using ClashRoyaleApi.Data;
using ClashRoyaleApi.DTOs.Clan;
using ClashRoyaleApi.Logic.RoyaleApi;
using ClashRoyaleApi.Models;
using ClashRoyaleApi.Models.ClanMembers;
using ClashRoyaleApi.Models.DbModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;

namespace ClashRoyaleApi.Logic.ClanMembers
{
    public class ClanMemberLogic : IClanMemberLogic
    {
        private readonly IHttpClientWrapper _httpClient;
        private readonly DataContext _dataContext;
        private readonly IConfiguration _configuration;

        public ClanMemberLogic(DataContext context, IConfiguration configuration, IHttpClientWrapper wrapper) 
        {
            _dataContext = context;
            _configuration = configuration;
            _httpClient = wrapper;
        }    

        //testing constructor
        public ClanMemberLogic(DataContext context, IHttpClientWrapper wrapper) 
        {
            _dataContext = context;
            _httpClient = wrapper;
        }

        public async Task RetrieveClanInfoScheduler()
        {
            string response = await RoyaleApiCall();
            var list = JsonConvert.DeserializeObject<Root>(response);

            //string text = File.ReadAllText("./claninfo.json");
            //var list = JsonConvert.DeserializeObject<Root>(text);

            string format = "yyyyMMddTHHmmss.fffZ";
            DateTime Inactive = DateTime.Now.AddDays(-7);

            List<DbClanMembers> allMembers = GetAllClanMembers();

            foreach (var member in list.Items)
            { 
                DbClanMembers dbClanMember;
                //first flow check if there are any new unregisterd members
                if (!allMembers.Any(t => t.ClanTag == member.Tag))
                {
                    AddNewMember(member, format);
                    continue;
                }
                else
                {
                    dbClanMember = allMembers.FirstOrDefault(t => t?.ClanTag == member?.Tag);
                }            

                DateTime LastSeen = DateTime.ParseExact(member.LastSeen, format, System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.AssumeUniversal);

                if (LastSeen < Inactive) dbClanMember.IsActive = false;

                if(dbClanMember.Role != member.Role)
                {
                    _dataContext.RiverClanMemberLog.Add(AddLog(dbClanMember.ClanTag, dbClanMember.Name, EnumClass.MemberStatus.RoleChange, dbClanMember.Role, member.Role));
                    dbClanMember.Role = member.Role;

                }

                dbClanMember.LastSeen = LastSeen;
                dbClanMember.IsActive = true;

                if(!dbClanMember.IsInClan) 
                    _dataContext.RiverClanMemberLog.Add(AddLog(member.Tag, member.Name, EnumClass.MemberStatus.Joined, string.Empty, "joined"));

                dbClanMember.IsInClan = true;

                _dataContext.DbClanMembers.Update(dbClanMember);
                _dataContext.SaveChanges();
            }

            foreach (DbClanMembers member in allMembers)
            {
                if(!list.Items.Any(t => t.Tag == member.ClanTag))
                {
                    if(member.IsInClan) 
                        RemoveMember(member);
                }
            }
        }

        public async Task<List<GetClanMemberInfoDTO>> GetClanMemberInfo()
        {
            List<DbClanMembers> DbMembers = await _dataContext.DbClanMembers.ToListAsync();
            List<GetClanMemberInfoDTO> response = new List<GetClanMemberInfoDTO>();

            foreach (var member in DbMembers)
            {

                response.Add(new GetClanMemberInfoDTO()
                {
                    Tag = member.ClanTag,
                    Name = member.Name,
                    Role = member.Role,
                    LastSeen = member.LastSeen,
                    IsActive = member.IsActive,
                    IsInClan = member.IsInClan,
                });
            }
            return response;
        }

        public async Task<List<GetClanMemberLogDTO>> GetClanMemberLog()
        {
            List<DbClanMemberLog> log = await _dataContext.RiverClanMemberLog.OrderByDescending(x => x.Time).ToListAsync();
            List<GetClanMemberLogDTO> response = new List<GetClanMemberLogDTO>();

            foreach (var logItem in log)
            {
                response.Add(new GetClanMemberLogDTO()
                {
                    Guid = logItem.Guid,
                    Name = logItem.Name,
                    Tag = logItem.Tag,
                    Time = logItem.Time,
                    Status = logItem.Status,
                    NewValue = logItem.NewValue,
                    OldValue = logItem.OldValue,
                });
            }
            return response;
        }

        public async Task DeleteRiverRaceLog(Guid guid)
        {
            if (!_dataContext.RiverClanMemberLog.Any(x => x.Guid == guid)) throw new Exception("Guid does not exist");

            DbClanMemberLog log =  await _dataContext.RiverClanMemberLog.Where(x => x.Guid == guid).FirstOrDefaultAsync();

            _dataContext.RiverClanMemberLog.Remove(log);
            await _dataContext.SaveChangesAsync();
        }

        private List<DbClanMembers> GetAllClanMembers()
        {
            return _dataContext.DbClanMembers.ToList();
        }

        private void AddNewMember(Member member, string format)
        {
            DbClanMembers newMember = new DbClanMembers()
            {
                Guid = Guid.NewGuid(),
                ClanTag = member.Tag,
                Name = member.Name,
                Role = member.Role,
                LastSeen = DateTime.ParseExact(member.LastSeen, format, System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.AssumeUniversal),
                IsActive = true,
                IsInClan = true
            };

            _dataContext.DbClanMembers.Add(newMember);
            _dataContext.RiverClanMemberLog.Add(AddLog(member.Tag, member.Name, EnumClass.MemberStatus.Joined, string.Empty, "joined"));
            _dataContext.SaveChanges();
        }

        private void RemoveMember(DbClanMembers member)
        {
            member.LastSeen = DateTime.Now;
            member.IsActive = false;
            member.IsInClan = false;

            _dataContext.DbClanMembers.Update(member);
            _dataContext.RiverClanMemberLog.Add(AddLog(member.ClanTag, member.Name, EnumClass.MemberStatus.Removed, string.Empty, "Removed"));
            _dataContext.SaveChanges();
        }

        private DbClanMemberLog AddLog(string tag, string name, EnumClass.MemberStatus status, string oldValue, string newValue)
        {
            return new DbClanMemberLog()
            {
                Guid = Guid.NewGuid(),
                Tag = tag,
                Name = name,
                Status = status,
                OldValue = oldValue,
                NewValue = newValue,
                Time = DateTime.Now,
            };
        }

        private async Task<string> RoyaleApiCall()
        {
            string apiUrl = _configuration.GetSection("RoyaleAPI:HttpAdressClanInfo").Value!;
            string accessToken = _configuration.GetSection("RoyaleAPI:AccessToken").Value!;

            //string apiUrl = "";
            //string accessToken = "";

            _httpClient.BaseAdress = new Uri(apiUrl);
            _httpClient.AddDefaultRequestHeader("Bearer", accessToken);

            try
            {
                HttpResponseMessage response = await _httpClient.GetAsync(apiUrl);

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
