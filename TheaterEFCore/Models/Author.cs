namespace TheaterEFCore.Models
{
    public class Author
    {
        public Author() : base()
        {

        }
        public int AuthorID { get; set; }
        public string AuthorName { get; set; }
        public ICollection<MoviesAuthors> MoviesAuthors { get; set; }

    }
}
