namespace TheaterEFCore.Models
{
    public class MoviesAuthors
    {
        public MoviesAuthors() : base()
        {
            
        }
        public int MovieID { get; set; }
        public Movie Movie { get; set; }
        public int AuthorID { get; set; }
        public Author Author { get; set; }
    }
}
