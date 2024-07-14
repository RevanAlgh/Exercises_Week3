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
                    Console.WriteLine("4. Get a specific movie");
                    Console.WriteLine("5. Show movies and authors");
                    Console.WriteLine("6. Show authors and movie [counts]");
                    Console.WriteLine("7. Search Movies by a Movie Title [Keyword]");
                    Console.WriteLine("8. Show top 3 movies released before 2001 (Ascending Order)");
                    Console.WriteLine("9. Show top 3 movies released just before 2001 (Descending Order)");
                    Console.WriteLine("10. Show languages with [average] budget more then 50m");
                    Console.WriteLine("11. Show movies with country name");
                    Console.WriteLine("12. Show movie titles and [count] of spoken languages");
                    Console.WriteLine("13. Get all movies");
                    Console.WriteLine("#. to Exit");

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
                            GetMovieFromDatabase(_context);
                            break;

                        case "5":
                            var moviesAndAuthors = GetMoviesAndAuthors(_context);
                            foreach (var item in moviesAndAuthors)
                            {
                                Console.WriteLine($"Movie: {item.MovieTitle}, Authors: {item.Authors}");
                            }
                            break;

                        case "6":
                            var authorsAndMovieCounts = GetAuthorsAndMovieCounts(_context);
                            foreach (var item in authorsAndMovieCounts)
                            {
                                Console.WriteLine($"Author: {item.AuthorName}, Movie Count: {item.MovieCount}");
                            }
                            break;

                        case "7":
                            Console.WriteLine("Enter a keyword to search for in movie titles:");
                            string keyword = Console.ReadLine();

                            var movies = GetMoviesWithTitleKeyword(_context, keyword);
                            Console.WriteLine($"Movies with '{keyword}' in title:");
                            DisplayMovies(movies);
                            break;

                        case "8":
                            var topMovies = Get3MoviesBefore2001Asc(_context);
                            foreach (var movie in topMovies)
                            {
                                Console.WriteLine($"Movie: {movie.MovieTitle}, IMDb Rating: {movie.ImdbRating}, Year Released: {movie.YearReleased}");
                            }
                            break;

                        case "9":
                            var topMovies2 = Get3MoviesBefore2001Desc(_context);
                            foreach (var movie in topMovies2)
                            {
                                Console.WriteLine($"Movie: {movie.MovieTitle}, IMDb Rating: {movie.ImdbRating}, Year Released: {movie.YearReleased}");
                            }
                            break;

                        case "10":
                            var highBudgetLanguages = GetLanguagesWithAverageBudget(_context);
                            foreach (var item in highBudgetLanguages)
                            {
                                Console.WriteLine($"Language: {item.Language}, Average Budget: {item.AverageBudget}");
                            }
                            break;

                        case "11":
                            var moviesWithCountry = GetMoviesWithCountryName(_context);
                            foreach (var item in moviesWithCountry)
                            {
                                Console.WriteLine($"Movie: {item.MovieTitle}, Country: {item.CountryName}");
                            }
                            break;

                        case "12":
                            var movieLanguageCounts = GetMovieLanguageCounts(_context);
                            foreach (var item in movieLanguageCounts)
                            {
                                Console.WriteLine($"Movie: {item.MovieTitle}, Language Count: {item.LanguageCount}");
                            }
                            break;

                        case "13":
                            GetMovies(_context);
                            break;

                        case "#":
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

            context.Movies.Add(movie); //Method 1
            context.SaveChanges();

            Console.WriteLine("\nMovie added successfully.");
        }

        public static void ModifyMovie(AppDbContext context)
        {
            var movie = GetMovieFromDatabase(context);
            //string.IsNullOrEmpty(movie);
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

                //context.Update<Movie>(movie); //context.UpdateRange //method 2

                context.SaveChanges();
                Console.WriteLine("\nMovie modified successfully.");
            }
        }


        public static void DeleteMovie(AppDbContext context)
        {
            var movie = GetMovieFromDatabase(context);
            if (movie != null)
            {
                context.Movies.Remove(movie); //context.Remove //method 3
                context.SaveChanges();
                Console.WriteLine("\nMovie deleted successfully.");
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

            var movie = context.Movies
                .Include(m => m.Country)
                .Include(m => m.MoviesAuthors).ThenInclude(ma => ma.Author)
                .FirstOrDefault(m => m.MovieTitle == movieTitle);

            if (movie != null)
            {
                Console.WriteLine($"MovieID: {movie.MovieID}, Title: {movie.MovieTitle}, IMDb Rating: {movie.ImdbRating}, Year: {movie.YearReleased}, Budget: {movie.Budget}, Box Office: {movie.BoxOffice}, Language: {movie.Language}, Country: {movie.Country?.CountryName}");
                foreach (var ma in movie.MoviesAuthors)
                {
                    Console.WriteLine($"AuthorID: {ma.AuthorID}, AuthorName: {ma.Author.AuthorName}\n");
                }
            }
            else
            {
                Console.WriteLine("Movie not found.");
            }

            return movie;
        }

        public static void GetMovies(AppDbContext context)
        {
            var movies = context.Movies
                .Include(m => m.Country)
                .Include(m => m.MoviesAuthors).ThenInclude(ma => ma.Author)
                .ToList(); //.FirstOrDefault(m => m.MovieTitle == movieTitle); //ToArray();


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

        //5
        public static List<(string MovieTitle, string Authors)> GetMoviesAndAuthors(AppDbContext context)
        {
            var result = context.Movies
                .Select(m => new
                {
                    m.MovieTitle,
                    Authors = string.Join(", ", m.MoviesAuthors.Select(ma => ma.Author.AuthorName)) // STRING_AGG
                })
                .OrderBy(ma => ma.MovieTitle)
                .ToList()
                .Select(ma => (ma.MovieTitle, ma.Authors))
                .ToList();

            return result;
        }

        //6
        public static List<(string AuthorName, int MovieCount)> GetAuthorsAndMovieCounts(AppDbContext context)
        {
            var result = context.Authors
                .Select(a => new
                {
                    a.AuthorName,
                    MovieCount = a.MoviesAuthors.Count // COUNT
                })
                .OrderBy(amc => amc.AuthorName)
                .ToList()
                .Select(amc => (amc.AuthorName, amc.MovieCount))
            .ToList();

            return result;
        }

        //7
        public static List<Movie> GetMoviesWithTitleKeyword(AppDbContext context, string keyword)
        {
            return context.Movies
                .Where(m => EF.Functions.Like(m.MovieTitle, $"%{keyword}%")) //LIKE 
                .Select(m => new Movie
                {
                    MovieTitle = m.MovieTitle,
                    ImdbRating = m.ImdbRating,
                    YearReleased = m.YearReleased
                })
                .ToList();
        }

        public static void DisplayMovies(List<Movie> movies)
        {
            foreach (var movie in movies)
            {
                Console.WriteLine($"Title: {movie.MovieTitle}, IMDb Rating: {movie.ImdbRating}, Year Released: {movie.YearReleased}");
            }
        }

        //8
        public static List<Movie> Get3MoviesBefore2001Asc(AppDbContext context)
        {
            return context.Movies
                .Where(m => m.YearReleased < 2001)
                .OrderBy(m => m.YearReleased) // ASC
                .Take(3) // TOP 3
                .Select(m => new Movie
                {
                    MovieTitle = m.MovieTitle,
                    ImdbRating = m.ImdbRating,
                    YearReleased = m.YearReleased
                })
                .ToList();
        }

        //9
        public static List<Movie> Get3MoviesBefore2001Desc(AppDbContext context)
        {
            return context.Movies
                .Where(m => m.YearReleased < 2001)
                .OrderByDescending(m => m.YearReleased) // DESC
                .Take(3) // TOP 3
                .Select(m => new Movie
                {
                    MovieTitle = m.MovieTitle,
                    ImdbRating = m.ImdbRating,
                    YearReleased = m.YearReleased
                })
                .ToList();
        }


        //10
        public static List<(string Language, decimal AverageBudget)> GetLanguagesWithAverageBudget(AppDbContext context)
        {
            return context.Movies
                .GroupBy(m => m.Language) //GROUP BY
                .Select(g => new
                {
                    Language = g.Key,
                    AverageBudget = g.Average(m => m.Budget) //AVG
                })
                .Where(g => g.AverageBudget > 50) //50,000,000
                .OrderByDescending(g => g.AverageBudget) // DESC
                .ToList()
                .Select(g => (g.Language, g.AverageBudget))
                .ToList();
        }


        //11
        public static List<(string MovieTitle, string CountryName)> GetMoviesWithCountryName(AppDbContext context)
        {
            return context.Movies
                .Include(m => m.Country) // JOIN 
                .Select(m => new
                {
                    MovieTitles = m.MovieTitle,
                    CountryNames = m.Country.CountryName
                })
                .ToList()
                .Select(m => (m.MovieTitles, m.CountryNames))
                .ToList();
        }

        //12
        public static List<(string MovieTitle, int LanguageCount)> GetMovieLanguageCounts(AppDbContext context)
        {
            var moviesWithLanguages = context.Movies
                .Select(m => new
                {
                    MovieTitles = m.MovieTitle,
                    Languages = m.Language
                })
                .ToList();

            var result = moviesWithLanguages
                .Select(m =>
                {
                    var languages = m.Languages?.Split(',', StringSplitOptions.RemoveEmptyEntries); // STRING_SPLIT FUNCTION
                    int languageCount = languages?.Length ?? 0;
                    return (m.MovieTitles, LanguageCount: languageCount);
                })
                .ToList();

            return result;
        }
    }
}
