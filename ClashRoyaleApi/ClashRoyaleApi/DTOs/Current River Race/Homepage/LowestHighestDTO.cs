namespace ClashRoyaleApi.DTOs.Current_River_Race.Homepage
{
    public class LowestHighestDTO
    {
        public string Name { get; set; }

        public int Value { get; set; }

        public LowestHighestDTO(string name, int value) 
        { 
            Name = name;
            Value = value; 
        }
    }
}
