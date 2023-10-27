using ClashRoyaleApi.DTOs.MailSubscriptions;

namespace ClashRoyaleApi.Logic.MailHandler.MailSubscription
{
    public interface IMailSubscription
    {
        List<MailSubscriptionsDTO> GetMailSubscriptions(string clanTag);

        void UpdateMailSubscriptions(List<MailSubscriptionsDTO> dto, string clanTag);


    }
}
