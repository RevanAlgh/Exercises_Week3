using TheaterEFCore.Models;

namespace TheaterEFCore
{
    public class Program
    {
        static void Main(string[] args)
        {
            AppDbContext _context = new AppDbContext();

            //Country Table
            var country = new Country { CountryName = "South Korea" };
            _context.Countries.Add(country);
            _context.SaveChanges();

            //Movie Table
            var movie = new Movie
            {
                MovieTitle = "Forgotten",
                ImdbRating = 7.4f,
                YearReleased = 2017,
                Budget = 40000000m,
                BoxOffice = 10000000m,
                Language = "Korean",
                CountryID = country.CountryID // Link to Country
            };
            _context.Movies.Add(movie);
            _context.SaveChanges();


            //Author Table
            var author = new Author { AuthorName = "Stephen King" };
            _context.Authors.Add(author);
            _context.SaveChanges();


            //MoviesAuthors Table
            var moviesAuthors = new MoviesAuthors
            {
                MovieID = movie.MovieID,
                AuthorID = author.AuthorID
            };
            _context.MoviesAuthors.Add(moviesAuthors);
            _context.SaveChanges();

        }
    }
}
