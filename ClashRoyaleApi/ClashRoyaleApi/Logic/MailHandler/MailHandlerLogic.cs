using ClashRoyaleApi.Data;
using ClashRoyaleApi.Models.CurrentRiverRace.CRR_Response;
using ClashRoyaleApi.Models.DbModels;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using static ClashRoyaleApi.Models.EnumClass;

namespace ClashRoyaleApi.Logic.MailHandler
{
    public class MailHandlerLogic : IMailHandler
    {
        private readonly DataContext _dataContext;

        public MailHandlerLogic(DataContext context) 
        {
            _dataContext = context;     
        }

        public void SendEmail(Response res)
        {
            foreach (int i in Enum.GetValues(typeof(MailType)))
            {
                MailType type = (MailType)i;
                SmtpClient myServer = GetSmtpClient();
           
                List<DBUser> mailAdresses = GetMailAddresses(type, res.log.SchedulerTime);

                if (mailAdresses.Count == 0) continue;
                if (res.log.Status == Status.FAILED && type == MailType.ATTACKSREMAINING) continue;

                MailMessage message = new MailMessage();

                if (res.log.Status == Status.FAILED) message.Priority = MailPriority.High;

                message.From = new MailAddress("Nickvdmeer2001@gmail.com", "Nick van der Meer sender");

                if (type == MailType.BUILD) continue;

                if (type == MailType.ATTACKSREMAINING)
                {
                    SendAttacksRemainingMail(mailAdresses, res, myServer);
                    continue;
                }

                message.Subject = Subject(type, res.log.SchedulerTime, res.log.Status);
                message.Body = Body(type, res);
                message.IsBodyHtml = true;
                
                myServer.Send(message);
            }        
        }
       
        private bool SendAttacksRemainingMail(List<DBUser> mailAdresses, Response response, SmtpClient myServer)
        {
            foreach (DBUser user in mailAdresses) 
            {
                MailMessage message = new MailMessage();
                message.From = new MailAddress("Nickvdmeer2001@gmail.com", "Nick van der Meer sender");
                message.To.Add(user.Email);
                message.Subject = Subject(MailType.ATTACKSREMAINING, response.log.SchedulerTime, response.log.Status);
                message.Body = AttacksRemainingBody(response, "#8GGRUUCQ");
                message.IsBodyHtml = true;

                myServer?.Send(message);
             }
            return true;
        }

        private string Subject(MailType type, SchedulerTime time, Status status)
        {
            StringBuilder sb = new StringBuilder(); 

            switch(type) 
            {
                case MailType.BUILD:
                    sb.Append("Build Status ");
                    break;
                case MailType.ATTACKSREMAINING:
                    sb.Append("Number of attacks remaining ");
                    break;
                default:
                    sb.Append("Mail Type undefined");
                    break;
            }

            switch(time) 
            {
                case SchedulerTime.MINUTESBEFORE120:
                    sb.Append("At 08:00");
                    break;
                case SchedulerTime.MINUTESBEFORE60:
                    sb.Append("At 10:30");
                    break;
                case SchedulerTime.MINUTESBEFORE30:
                    sb.Append("At 11:00");
                    break;
                case SchedulerTime.MINUTESBEFORE5:
                    sb.Append("At 11:30");
                    break;
                default:
                    sb.Append("Scheduling time undefined");
                    break;
            }

            if (type == MailType.BUILD) sb.Append($" {status.ToString()}");

            return sb.ToString();   
        }

        public SmtpClient GetSmtpClient() 
        {
            SmtpClient MyServer = new SmtpClient("smtp.gmail.com", 587);

            //Server Credentials

            NetworkCredential NC = new NetworkCredential();
            NC.UserName = "Nickvdmeer2001@gmail.com";
            NC.Password = "bwwm wcju qxge qdox";

            //assigned credetial details to server
            MyServer.Credentials = NC;
            MyServer.EnableSsl = true;
            return MyServer;
        }

        public List<DBUser> GetMailAddresses(MailType type, SchedulerTime time) 
        {
            List<DBUser> users = _dataContext.GetDBUsersWithMailSubscriptions(type, time);
            return users;
        }

        private string Body(MailType type, Response res)
        {
            StringBuilder sb = new StringBuilder();

            switch(type) 
            {
                case MailType.BUILD:
                    return BuildBody(res);
                case MailType.ATTACKSREMAINING:
                    return AttacksRemainingBody(res, "");
                default: return sb.ToString();       
            }
        }

        private string AttacksRemainingBody(Response res, string clantag)
        {
            List<DbCallList> list = _dataContext.CallList.ToList();
            var orderedByClanTag = list.OrderBy(x => x.ColeaderClanTag != clantag).ThenBy(x => x.ColeaderClanTag).ToList();
            StringBuilder sb = new StringBuilder();

            bool coleader = true;

            string[] distinctValues = orderedByClanTag
            .Select(x => x.Coleader)
            .Where(value => value != null)
            .Distinct()
            .ToArray();

            sb.Append("<html>\r\n<head>\r\n    <style>\r\n        .header {\r\n            font-size: 24px;\r\n            text-align: center;\r\n            padding: 20px;\r\n            color: white;\r\n            font-weight: bold;\r\n            background-color: #1B163F;\r\n        }\r\n\r\n        #customers {\r\n            font-family: Arial, Helvetica, sans-serif;\r\n            border-collapse: collapse;\r\n            width: 30%;\r\n            margin-left: auto;\r\n            margin-right: auto;\r\n        }\r\n\r\n        h1 {\r\n            text-align: center;\r\n        }\r\n\r\n        h4 {\r\n            text-align: center;\r\n        }\r\n\r\n        .center {\r\n            margin-left: auto;\r\n            margin-right: auto;\r\n        }\r\n\r\n        #customers td,\r\n        #customers th {\r\n            border: 1px solid #ddd;\r\n            padding: 8px;\r\n            text-align: center;\r\n        }\r\n\r\n        #customers tr:nth-child(even) {\r\n            background-color: #f2f2f2;\r\n        }\r\n\r\n        #customers tr:hover {\r\n            background-color: #ddd;\r\n        }\r\n\r\n        #customers th {\r\n            padding-top: 12px;\r\n            padding-bottom: 12px;\r\n            text-align: center;\r\n            background-color: #04AA6D;\r\n            color: white;\r\n        }\r\n    </style>\r\n</head>");
            sb.Append($"<body>\r\n    <div class=\"header success\">\r\n        Clash royale API\r\n    </div>\r\n\r\n    <h1>Number of attacks remaining at {res.log.TimeStamp}</h1>\r\n    <h4>An overview of clan members that still have remaining attacks left</h4>\r\n    <h4>Scheduled event: {res.log.SchedulerTime.ToString()}</h4>");
            foreach (string item in distinctValues) 
            {
                sb.Append($"<h1>{item}</h1>" );
                sb.Append("<table id=\"customers\">\r\n            <tr>\r\n                <th>Player Tag</th>\r\n                <th>Name</th>\r\n                <th>Decks Remaining</th>\r\n            </tr>");
                foreach (DbCallList call in orderedByClanTag) 
                {
                    NrOfAttacksRemaining attack = new NrOfAttacksRemaining();

                    if (coleader)
                    {
                        attack = res.nrOfAttacksRemaining.Where(x => x.Name == item).FirstOrDefault();

                        if (attack != null && attack.DecksRemaining != 0)
                        {
                            sb.Append($"<tr>\r\n            <td>{attack.ClanTag}</td>\r\n                <td>{attack.Name}</td>\r\n                <td>{attack.DecksRemaining}</td>\r\n            </tr>");                            
                        }
                        coleader = false;
                    }

                    attack = res.nrOfAttacksRemaining.Where(x => x.ClanTag == call.MemberTag).FirstOrDefault();
                    
                    if (attack == null) continue;

                    if (call.Coleader == item)
                    {
                        if (attack.DecksRemaining == 0) continue;
                        sb.Append($"<tr>\r\n            <td>{attack.ClanTag}</td>\r\n                <td>{attack.Name}</td>\r\n                <td>{attack.DecksRemaining}</td>\r\n            </tr>");
                    }
                }
                sb.Append("        </table>\r\n</body>\r\n\r\n</html>");
                sb.Append("<br></br>");
                coleader = true;
            }
            return sb.ToString();
            //sb.Append("<html>\r\n<head>\r\n    <style>\r\n        .header {\r\n            font-size: 24px;\r\n            text-align: center;\r\n            padding: 20px;\r\n            color: white;\r\n            font-weight: bold;\r\n            background-color: #1B163F;\r\n        }\r\n\r\n        #customers {\r\n            font-family: Arial, Helvetica, sans-serif;\r\n            border-collapse: collapse;\r\n            width: 30%;\r\n            margin-left: auto;\r\n            margin-right: auto;\r\n        }\r\n\r\n        h1 {\r\n            text-align: center;\r\n        }\r\n\r\n        h4 {\r\n            text-align: center;\r\n        }\r\n\r\n        .center {\r\n            margin-left: auto;\r\n            margin-right: auto;\r\n        }\r\n\r\n        #customers td,\r\n        #customers th {\r\n            border: 1px solid #ddd;\r\n            padding: 8px;\r\n            text-align: center;\r\n        }\r\n\r\n        #customers tr:nth-child(even) {\r\n            background-color: #f2f2f2;\r\n        }\r\n\r\n        #customers tr:hover {\r\n            background-color: #ddd;\r\n        }\r\n\r\n        #customers th {\r\n            padding-top: 12px;\r\n            padding-bottom: 12px;\r\n            text-align: center;\r\n            background-color: #04AA6D;\r\n            color: white;\r\n        }\r\n    </style>\r\n</head>");
            //sb.Append($"<body>\r\n    <div class=\"header success\">\r\n        Clash royale API\r\n    </div>\r\n\r\n    <h1>Number of attacks remaining at {res.log.TimeStamp}</h1>\r\n    <h4>An overview of clan members that still have remaining attacks left</h4>\r\n    <h4>Scheduled event: {res.log.SchedulerTime.ToString()}</h4>");
            //sb.Append("<table id=\"customers\">\r\n            <tr>\r\n                <th>Player Tag</th>\r\n                <th>Name</th>\r\n                <th>Decks Remaining</th>\r\n            </tr>");

            //foreach (NrOfAttacksRemaining attacks in res.nrOfAttacksRemaining.OrderByDescending(x => x.DecksRemaining))
            //{
            //    if (attacks.DecksRemaining == 0) continue;
            //    sb.Append($"<tr>\r\n            <td>{attacks.ClanTag}</td>\r\n                <td>{attacks.Name}</td>\r\n                <td>{attacks.DecksRemaining}</td>\r\n            </tr>");
            //}    

            //sb.Append("        </table>\r\n</body>\r\n\r\n</html>");
            //return sb.ToString();
        }

        private string BuildBody(Response res)
        {
            StringBuilder sb = new StringBuilder();

            sb.Append("<html>\r\n<head>\r\n<style>\r\n.alignment {text-align : center;}  .header {\r\n    font-size: 24px;\r\n    text-align: center;\r\n    padding: 20px;\r\n    color: white;\r\n    font-weight: bold;\r\n  }\r\n  .success {\r\n    background-color: #4CAF50; /* Green */\r\n  }\r\n  .failure {\r\n    background-color: #f44336; /* Red */\r\n  }\r\n  #rcorners2 {\r\n  border-radius: 25px;\r\n  border: 2px solid #73AD21;\r\n  padding: 20px; \r\n  width: auto;\r\n  height: auto;  \r\n  text-align: center;\r\n  font-family: Verdana, Geneva, Tahoma, sans-serif;\r\n} " +
                "#customers {\r\n            font-family: Arial, Helvetica, sans-serif;\r\n            border-collapse: collapse;\r\n            width: 100%;\r\n            margin-left: auto;\r\n            margin-right: auto;\r\n        }\r\n\r\n#customers td,\r\n        #customers th {\r\n            border: 1px solid #ddd;\r\n            padding: 8px;\r\n            text-align: center;\r\n        }\r\n\r\n        #customers tr:nth-child(even) {\r\n            background-color: #f2f2f2;\r\n        }\r\n\r\n        #customers tr:hover {\r\n            background-color: #ddd;\r\n        }\r\n\r\n        #customers th {\r\n            padding-top: 12px;\r\n            padding-bottom: 12px;\r\n            text-align: center;\r\n            background-color: #f44336;\r\n            color: white;\r\n        }</style>\r\n</head>");
            sb.Append("<body>");

            if (res.log.Status == Status.SUCCES)
            {
                sb.Append("<div class=\"header success\">\r\n    Build Successful\r\n  </div>");
                sb.Append($"<p id=\"rcorners2\"> at {res.log.TimeStamp} the current rive race data was retrieved with status: {res.log.Status.ToString()} for the scheduled event: {res.log.SchedulerTime.ToString()} and stored in the database! </p>");
            }
            else
            {
                sb.Append("<div class=\"header failure\">\r\n    Build Failed\r\n  </div>");
                if (res.log.SchedulerTime == SchedulerTime.CLANINFOSCHEDULE)
                {
                    sb.Append($"<p id=\"rcorners2\" style=\"border: 2px solid #f44336;\"> At {res.log.TimeStamp} the clan member info data was retrieved with status: {res.log.Status.ToString()} <br><br>");
                }
                else
                {
                    sb.Append($"<p id=\"rcorners2\" style=\"border: 2px solid #f44336;\"> At {res.log.TimeStamp} the current rive race data was retrieved with status: {res.log.Status.ToString()} for the scheduled event: {res.log.SchedulerTime.ToString()} <br><br>");
                }
                sb.Append($"The code Exited with the following Exception:\r\n\r\n    <table id=\"customers\">\r\n        <tr>\r\n            <th>Name</th>\r\n            <th>Exception Type</th>\r\n        </tr>\r\n        <tr>\r\n            <td>Exception Type</td>\r\n            <td>{res.Exception.GetType().ToString()}</td>\r\n        </tr>\r\n        <tr>\r\n            <td>Exception Message</td>\r\n            <td>{res.Exception.Message}</td>\r\n        </tr>\r\n        <tr>\r\n            <td>Exception StackTrace</td>\r\n            <td>{res.Exception.StackTrace}</td>\r\n        </tr>\r\n\r\n    </table>\r\n</p>");
            }
            sb.Append("</body> \r\n </html>");
            return sb.ToString();
        }

        private string SchedulerNotSet(Response set)
        {
            return "";
        }
    }
}
