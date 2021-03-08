namespace StoreModels
{
    /// <summary>
    /// This class contains all the fields and properties that define a store location.
    /// </summary>
    public class Location
    {
        //Maybe change address into a compositve value thing? street name, state, city
        public string Address { get; set; }
        public int Id {get; set;}
        public string LocationName { get; set; }
        
        //public ItemGroup { get; set;}
    }
}