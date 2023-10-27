namespace ClashRoyaleApi.Models.CurrentRiverRace.CRR_Response
{
    public class Response
    {

        public List<NrOfAttacksRemaining> nrOfAttacksRemaining = new List<NrOfAttacksRemaining>();

        public CurrentRiverRaceLog log = new CurrentRiverRaceLog(); 

        public Exception? Exception { get; set; }

        public Response() 
        { 
            
        
        }   



    }
}
