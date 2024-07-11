namespace TheaterEFCore.Models
{
    public class Country
    {
        public Country() : base()
        {
            
        }
        public int CountryID { get; set; }
        public string CountryName { get; set; }
        public ICollection<Movie> Movies { get; set; } // One to Many relationship
    }
}
