using ClashRoyaleApi.Models;
using System.Security.Cryptography.X509Certificates;

namespace ClashRoyaleApi.Logic.RiverRace
{
    public class RiverRaceLogic : IRiverRaceLogic
    {

        public RiverRaceLogic()        
        { 
              
        }

        public RiverRaceLog GetRiverRaceLog() 
        { 
           RiverRaceLog log = new RiverRaceLog();
      
           return log;
               
        }


    }
}
