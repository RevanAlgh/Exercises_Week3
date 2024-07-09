namespace TheaterEFCore.Models
{
    public class Movie
    {
        public Movie() : base()
        {

        }

        public int MovieID { get; set; }
        public string MovieTitle { get; set; }
        public float ImdbRating { get; set; }
        public int YearReleased { get; set; }
        public decimal Budget { get; set; }
        public decimal BoxOffice { get; set; }
        public string Language { get; set; }
        public int CountryID { get; set; }
        public Country Country { get; set; }
        public ICollection<MoviesAuthors> MoviesAuthors { get; set; }

    }
}
