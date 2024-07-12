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
        public int CountryID { get; set; } // Foreign key to Country
        public virtual Country Country { get; set; } // Navigation property back to Country
        public ICollection<MoviesAuthors> MoviesAuthors { get; set; } // One to Many relationship

    }
}
