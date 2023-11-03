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
        public ClanMemberLogic(DataContext context) 
        {
            _dataContext = context;
        }

        public async Task<List<DbClanMembers>> RetrieveClanInfoScheduler()
        {
            throw new ArgumentException("this is a test exception");
            string response = await _httpClient.RoyaleApiCall(EnumClass.RoyaleApiType.CLANMEMBERINFO);
            var list = JsonConvert.DeserializeObject<Root>(response);

            //string text = File.ReadAllText("./claninfo.json");
            //var list = JsonConvert.DeserializeObject<Root>(text);

            string format = "yyyyMMddTHHmmss.fffZ";
            DateTime Inactive = DateTime.Now.AddDays(-7);

            List<DbClanMembers> allMembers = GetAllClanMembers();
            List<DbClanMembers> res = new List<DbClanMembers>();

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

                if (LastSeen < Inactive)
                {
                    dbClanMember.IsActive = false;
                }
                else
                {
                    dbClanMember.IsActive = true;
                }

                if (dbClanMember.Role != member.Role)
                {
                    AddLog(dbClanMember.ClanTag, dbClanMember.Name, EnumClass.MemberStatus.RoleChange, dbClanMember.Role, member.Role);
                    dbClanMember.Role = member.Role;
                }

                dbClanMember.LastSeen = LastSeen;

                if (!dbClanMember.IsInClan)
                    AddLog(member.Tag, member.Name, EnumClass.MemberStatus.Joined, string.Empty, "joined");

                dbClanMember.IsInClan = true;

                _dataContext.DbClanMembers.Update(dbClanMember);
                _dataContext.SaveChanges();
                res.Add(dbClanMember);
            }

            foreach (DbClanMembers member in allMembers)
            {
                if(!list.Items.Any(t => t.Tag == member.ClanTag))
                {
                    if(member.IsInClan)
                    {
                        RemoveMember(member);
                        allMembers.Remove(member);
                    }                    
                }
            }
            return res;
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

        public virtual List<DbClanMembers> GetAllClanMembers()
        {
            return _dataContext.DbClanMembers.ToList();
        }

        public virtual void AddNewMember(Member member, string format)
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
            AddLog(member.Tag, member.Name, EnumClass.MemberStatus.Joined, string.Empty, "joined");
            _dataContext.SaveChanges();
        }

        private void RemoveMember(DbClanMembers member)
        {
            member.LastSeen = DateTime.Now;
            member.IsActive = false;
            member.IsInClan = false;

            _dataContext.DbClanMembers.Update(member);
            AddLog(member.ClanTag, member.Name, EnumClass.MemberStatus.Removed, string.Empty, "Removed");
            _dataContext.SaveChanges();
        }

        public virtual void AddLog(string tag, string name, EnumClass.MemberStatus status, string oldValue, string newValue)
        {
            DbClanMemberLog log = new DbClanMemberLog()
            {
                Guid = Guid.NewGuid(),
                Tag = tag,
                Name = name,
                Status = status,
                OldValue = oldValue,
                NewValue = newValue,
                Time = DateTime.Now,
            };
            _dataContext.RiverClanMemberLog.Add(log);
            _dataContext.SaveChanges();
        }
    }
}
