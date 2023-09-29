using ClashRoyaleCodeBase.Data;
using ClashRoyaleCodeBase.Models;
using ClashRoyaleCodeBase.Models.ClanMembers;
using ClashRoyaleCodeBase.Models.DbModels;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;

namespace ClashRoyaleCodeBase.Logic.ClanMembers
{
    public class ClanMemberLogic : IClanMemberLogic
    {
        static HttpClient client = new HttpClient();
        private readonly DataContext _dataContext;
        private readonly IConfiguration _configuration;

        public ClanMemberLogic(DataContext context, IConfiguration configuration) 
        {
            _dataContext = context;
            _configuration = configuration;
        }    

        public async Task GetClanInfo()
        {
            string response = await RoyaleApiCall();
            var list = JsonConvert.DeserializeObject<Root>(response);
            string format = "yyyyMMddTHHmmss.fffZ";
            DateTime Inactive = DateTime.Now.AddMonths(-1);

            List<DbClanMembers> allMembers = GetAllClanMembers();

            foreach (var member in list.Items)
            { 
                DbClanMembers dbClanMember;
                //first flow check if there are any new unregisterd members
                if (!allMembers.Any(t => t.Tag == member.Tag))
                {
                    AddNewMember(member, format);
                    continue;
                }
                else
                {
                    dbClanMember = allMembers.FirstOrDefault(t => t?.Tag == member?.Tag);
                }            

                DateTime LastSeen = DateTime.ParseExact(member.LastSeen, format, System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.AssumeUniversal);

                if (LastSeen < Inactive) dbClanMember.IsActive = false;
                dbClanMember.Role = member.Role;
                dbClanMember.LastSeen = LastSeen;
                dbClanMember.IsActive = true;
                dbClanMember.IsInClan = true;

                _dataContext.DbClanMembers.Update(dbClanMember);
                _dataContext.SaveChanges();
            }

            foreach (DbClanMembers member in allMembers)
            {
                if(!list.Items.Any(t => t.Tag == member.Tag))
                {
                    RemoveMember(member);
                }
            }
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
                Tag = member.Tag,
                Name = member.Name,
                Role = member.Role,
                LastSeen = DateTime.ParseExact(member.LastSeen, format, System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.AssumeUniversal),
                IsActive = true,
                IsInClan = true
            };

            _dataContext.DbClanMembers.Add(newMember);
            _dataContext.RiverClanMemberLog.Add(AddLog(member.Tag, member.Name, EnumClass.MemberStatus.Joined));
            _dataContext.SaveChanges();
        }

        private void RemoveMember(DbClanMembers member)
        {
            member.LastSeen = DateTime.Now;
            member.IsActive = false;
            member.IsInClan = false;

            _dataContext.DbClanMembers.Update(member);
            _dataContext.RiverClanMemberLog.Add(AddLog(member.Tag, member.Name, EnumClass.MemberStatus.Removed));
            _dataContext.SaveChanges();
        }

        private DbClanMemberLog AddLog(string tag, string name, EnumClass.MemberStatus status)
        {
            return new DbClanMemberLog()
            {
                Guid = Guid.NewGuid(),
                Tag = tag,
                Name = name,
                Status = status,
                Time = DateTime.Now,
            };
        }

        private async Task<string> RoyaleApiCall()
        {
            string apiUrl = _configuration.GetSection("RoyaleAPI:HttpAdressClanInfo").Value!;
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



    }


}
