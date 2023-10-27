using ClashRoyaleApi.Data;
using ClashRoyaleApi.DTOs.MailSubscriptions;
using ClashRoyaleApi.Models.Mail;
using static ClashRoyaleApi.Models.EnumClass;

namespace ClashRoyaleApi.Logic.MailHandler.MailSubscription
{
    public class MailSubscriptionLogic : IMailSubscription
    {
        private readonly DataContext _dataContext;
        public MailSubscriptionLogic(DataContext context) 
        { 
            _dataContext = context;             
        }

        public void UpdateMailSubscriptions(List<MailSubscriptionsDTO> dto, string clanTag)
        {
            List<MailSubscriptions> subs = _dataContext.MailSubscriptions.Where(x => x.ClanTag == clanTag).ToList();

            foreach (MailSubscriptionsDTO mSub in dto)
            {
                var existingSub = subs.FirstOrDefault(x => x.MailType == mSub.MailType && x.SchedulerTime == mSub.SchedulerTime);

                if (mSub.IsEnabled)
                {
                    if (existingSub == null)
                    {
                        _dataContext.MailSubscriptions.Add(new MailSubscriptions(mSub.MailType, mSub.SchedulerTime, clanTag));
                    }
                }
                else
                {
                    if (existingSub != null)
                    {
                        _dataContext.MailSubscriptions.Remove(existingSub);
                    }
                }
            }

            _dataContext.SaveChanges();









            //foreach (MailSubscriptionsDTO mSub in dto)
            //{
            //    if (mSub.IsEnabled)
            //    {
            //        if (subs.Any(x => x.MailType == mSub.MailType && x.SchedulerTime == mSub.SchedulerTime)) continue;

            //        _dataContext.MailSubscriptions.Add(new MailSubscriptions(mSub.MailType, mSub.SchedulerTime, clanTag));
            //    }
            //    else
            //    {
            //        if (!subs.Any(x => x.MailType == mSub.MailType && x.SchedulerTime == mSub.SchedulerTime)) continue;

            //        _dataContext.MailSubscriptions.Remove(subs.FirstOrDefault(x => x.MailType == mSub.MailType && x.SchedulerTime == mSub.SchedulerTime));
            //    }
            //}
            //_dataContext.SaveChanges(); 
        }

        public List<MailSubscriptionsDTO> GetMailSubscriptions(string clanTag)
        {
            List<MailSubscriptions> subs = _dataContext.MailSubscriptions.Where(x => x.ClanTag == clanTag).ToList();
            List<MailSubscriptionsDTO> response = new List<MailSubscriptionsDTO>();

            foreach (int mailType in Enum.GetValues(typeof(MailType)))
            {
                MailType type = (MailType)mailType;

                foreach (int scheduleTime in Enum.GetValues(typeof(SchedulerTime)))
                {
                    SchedulerTime time = (SchedulerTime)scheduleTime;
                    response.Add(new MailSubscriptionsDTO()
                    {
                        MailType = type,
                        SchedulerTime = time,
                        IsEnabled = subs.Any(x => x.MailType == type && x.SchedulerTime == time)
                    });
                }
            }
            return response;
        }
    }
}
