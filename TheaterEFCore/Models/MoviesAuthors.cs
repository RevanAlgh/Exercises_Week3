namespace TheaterEFCore.Models
{
    public class MoviesAuthors
    {
        public MoviesAuthors() : base()
        {
            
        }
        public int MovieID { get; set; }
        public virtual Movie Movie { get; set; }
        public int AuthorID { get; set; }
        public virtual Author Author { get; set; }
    }
}
