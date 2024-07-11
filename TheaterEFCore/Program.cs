using TheaterEFCore.Models;
using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace TheaterEFCore
{
    public class Program
    {
        static void Main(string[] args)
        {
            using (var _context = new AppDbContext())
            {
                bool continueProgram = true;

                Console.WriteLine("Welcome!\n");

                while (continueProgram)
                {
                    Console.WriteLine("\nSelect an operation:");
                    Console.WriteLine("1. Add a Movie");
                    Console.WriteLine("2. Modify a Movie");
                    Console.WriteLine("3. Delete a Movie");
                    Console.WriteLine("4. Get all Movies");
                    Console.WriteLine("5. Exit");

                    var input = Console.ReadLine();

                    switch (input)
                    {
                        case "1":
                            AddMovie(_context);
                            break;

                        case "2":
                            ModifyMovie(_context);
                            break;

                        case "3":
                            DeleteMovie(_context);
                            break;

                        case "4":
                            GetMovies(_context);
                            break;

                        case "5":
                            continueProgram = false;
                            break;

                        default:
                            Console.WriteLine("Invalid option. Please choose a valid option.");
                            break;
                    }
                }
            }
        }

        public static void AddMovie(AppDbContext context)
        {
            var movie = new Movie();

            Console.WriteLine("Enter movie title:");
            movie.MovieTitle = Console.ReadLine();

            Console.WriteLine("Enter IMDb rating:");
            movie.ImdbRating = float.Parse(Console.ReadLine());

            Console.WriteLine("Enter year released:");
            movie.YearReleased = int.Parse(Console.ReadLine());

            Console.WriteLine("Enter budget:");
            movie.Budget = decimal.Parse(Console.ReadLine());

            Console.WriteLine("Enter box office:");
            movie.BoxOffice = decimal.Parse(Console.ReadLine());

            Console.WriteLine("Enter language:");
            movie.Language = Console.ReadLine();

            movie.CountryID = GetOrAddCountry(context);
            movie.MoviesAuthors = new List<MoviesAuthors> { new MoviesAuthors { AuthorID = GetOrAddAuthor(context) } };

            context.Movies.Add(movie);
            context.SaveChanges();

            Console.WriteLine("\nMovie added successfully.");
        }

        public static void ModifyMovie(AppDbContext context)
        {
            var movie = GetMovieFromDatabase(context);
            if (movie != null)
            {
                bool modifying = true;

                while (modifying)
                {
                    Console.WriteLine("\nSelect an attribute to modify:");
                    Console.WriteLine("1. Movie Title");
                    Console.WriteLine("2. IMDb Rating");
                    Console.WriteLine("3. Year Released");
                    Console.WriteLine("4. Budget");
                    Console.WriteLine("5. Box Office");
                    Console.WriteLine("6. Language");
                    Console.WriteLine("7. Country");
                    Console.WriteLine("8. Author");
                    Console.WriteLine("9. Exit");

                    var input = Console.ReadLine();

                    switch (input)
                    {
                        case "1":
                            Console.WriteLine("Enter new movie title:");
                            movie.MovieTitle = Console.ReadLine();
                            break;

                        case "2":
                            Console.WriteLine("Enter new IMDb rating:");
                            movie.ImdbRating = float.Parse(Console.ReadLine());
                            break;

                        case "3":
                            Console.WriteLine("Enter new year released:");
                            movie.YearReleased = int.Parse(Console.ReadLine());
                            break;

                        case "4":
                            Console.WriteLine("Enter new budget:");
                            movie.Budget = decimal.Parse(Console.ReadLine());
                            break;

                        case "5":
                            Console.WriteLine("Enter new box office:");
                            movie.BoxOffice = decimal.Parse(Console.ReadLine());
                            break;

                        case "6":
                            Console.WriteLine("Enter new language:");
                            movie.Language = Console.ReadLine();
                            break;

                        case "7":
                            movie.CountryID = GetOrAddCountry(context);
                            break;

                        case "8":
                            movie.MoviesAuthors = new List<MoviesAuthors> { new MoviesAuthors { AuthorID = GetOrAddAuthor(context) } };
                            break;

                        case "9":
                            modifying = false;
                            break;

                        default:
                            Console.WriteLine("Invalid option. Please choose a valid option.");
                            break;
                    }
                }

                context.SaveChanges();
                Console.WriteLine("\nMovie modified successfully.");
            }
        }


        public static void DeleteMovie(AppDbContext context)
        {
            var movie = GetMovieFromDatabase(context);
            if (movie != null)
            {
                context.Movies.Remove(movie);
                context.SaveChanges();
                Console.WriteLine("\nMovie deleted successfully.");
            }
        }

        public static void GetMovies(AppDbContext context)
        {
            var movies = context.Movies
                .Include(m => m.Country)
                .Include(m => m.MoviesAuthors).ThenInclude(ma => ma.Author)
                .ToList();

            Console.WriteLine("Movies:");
            foreach (var movie in movies)
            {
                Console.WriteLine($"MovieID: {movie.MovieID}, Title: {movie.MovieTitle}, IMDb Rating: {movie.ImdbRating}, Year: {movie.YearReleased}, Budget: {movie.Budget}, Box Office: {movie.BoxOffice}, Language: {movie.Language}, Country: {movie.Country?.CountryName}");
                foreach (var ma in movie.MoviesAuthors)
                {
                    Console.WriteLine($"AuthorID: {ma.AuthorID}, AuthorName: {ma.Author.AuthorName}\n");
                }
            }
        }

        public static int GetOrAddCountry(AppDbContext context)
        {
            Console.WriteLine("Enter country name:");
            string countryName = Console.ReadLine();
            var country = context.Countries.FirstOrDefault(c => c.CountryName == countryName);
            if (country == null)
            {
                country = new Country { CountryName = countryName };
                context.Countries.Add(country);
                context.SaveChanges();
            }
            return country.CountryID;
        }

        public static int GetOrAddAuthor(AppDbContext context)
        {
            Console.WriteLine("Enter author name:");
            string authorName = Console.ReadLine();
            var author = context.Authors.FirstOrDefault(a => a.AuthorName == authorName);
            if (author == null)
            {
                author = new Author { AuthorName = authorName };
                context.Authors.Add(author);
                context.SaveChanges();
            }
            return author.AuthorID;
        }

        public static Movie GetMovieFromDatabase(AppDbContext context)
        {
            Console.WriteLine("Enter the movie title to search:");
            string movieTitle = Console.ReadLine();
            var movie = context.Movies.FirstOrDefault(m => m.MovieTitle == movieTitle);
            if (movie == null)
            {
                Console.WriteLine("Movie not found.");
            }
            return movie;
        }
    }
}
