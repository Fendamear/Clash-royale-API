namespace ClashRoyaleApi.Models.CurrentRiverRace.CRR_Response
{
    public class NrOfAttacksRemaining
    {
        public string ClanTag { get; set; } 

        public string Name { get; set; }

        public int DecksRemaining { get; set; }

        public NrOfAttacksRemaining(string clantag, string name, int decksremaining) 
        { 
            ClanTag = clantag;
            Name = name;
            DecksRemaining = decksremaining;
        }


    }
}
