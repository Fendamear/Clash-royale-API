using ClashRoyaleApi.Models.CurrentRiverRace.CRR_Response;
using System.Net.Mail;
using static ClashRoyaleApi.Models.EnumClass;

namespace ClashRoyaleApi.Logic.MailHandler
{
    public interface IMailHandler
    {
        void SendEmail(Response res);

        SmtpClient GetSmtpClient();

        MailMessage GetMailAddresses(MailType type, SchedulerTime time);
    }
}
