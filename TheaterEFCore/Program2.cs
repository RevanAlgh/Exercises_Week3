using TheaterEFCore.Models;
using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace TheaterEFCore
{
    public class Program
    {
        static void Main(string[] args)
        {
            using (var context = new AppDbContext())
            {
                bool continueProgram = true;

                Console.WriteLine("Welcome Admin!");

                while (continueProgram)
                {
                    Console.WriteLine("\nSelect an operation:");
                    Console.WriteLine("1. Get a specific movie");
                    Console.WriteLine("2. Show movies and authors");
                    Console.WriteLine("3. Show authors and movie [counts]");
                    Console.WriteLine("4. Search Movies by a Movie Title [Keyword]");
                    Console.WriteLine("5. Show top 3 movies released before 2001 (Ascending Order)");
                    Console.WriteLine("6. Show top 3 movies released just before 2001 (Descending Order)");
                    Console.WriteLine("7. Show languages with [average] budget more then 50m");
                    Console.WriteLine("8. Show movies with country name");
                    Console.WriteLine("9. Show movie titles and [count] of spoken languages");
                    Console.WriteLine("10. Get all movies");
                    Console.WriteLine("#. to Exit");

                    var input = Console.ReadLine();

                    switch (input)
                    {
                        case "1":
                            GetMovieFromDatabase(context);
                            break;

                        case "2":
                            var moviesAndAuthors = GetMoviesAndAuthors(context);
                            foreach (var item in moviesAndAuthors)
                            {
                                Console.WriteLine($"Movie: {item.MovieTitle}, Authors: {item.Authors}");
                            }
                            break;

                        case "3":
                            var authorsAndMovieCounts = GetAuthorsAndMovieCounts(context);
                            foreach (var item in authorsAndMovieCounts)
                            {
                                Console.WriteLine($"Author: {item.AuthorName}, Movie Count: {item.MovieCount}");
                            }
                            break;

                        case "4":
                            Console.WriteLine("Enter a keyword to search for in movie titles:");
                            string keyword = Console.ReadLine();

                            var movies = GetMoviesWithTitleKeyword(context, keyword);
                            Console.WriteLine($"Movies with '{keyword}' in title:");
                            DisplayMovies(movies);
                            break;

                        case "5":
                            var topMovies = Get3MoviesBefore2001Asc(context);
                            foreach (var movie in topMovies)
                            {
                                Console.WriteLine($"Movie: {movie.MovieTitle}, IMDb Rating: {movie.ImdbRating}, Year Released: {movie.YearReleased}");
                            }
                            break;

                        case "6":
                            var topMovies2 = Get3MoviesBefore2001Desc(context);
                            foreach (var movie in topMovies2)
                            {
                                Console.WriteLine($"Movie: {movie.MovieTitle}, IMDb Rating: {movie.ImdbRating}, Year Released: {movie.YearReleased}");
                            }
                            break;

                        case "7":
                            var highBudgetLanguages = GetLanguagesWithAverageBudget(context);
                            foreach (var item in highBudgetLanguages)
                            {
                                Console.WriteLine($"Language: {item.Language}, Average Budget: {item.AverageBudget}");
                            }
                            break;

                        case "8":
                            var moviesWithCountry = GetMoviesWithCountryName(context);
                            foreach (var item in moviesWithCountry)
                            {
                                Console.WriteLine($"Movie: {item.MovieTitle}, Country: {item.CountryName}");
                            }
                            break;

                        case "9":
                            var movieLanguageCounts = GetMovieLanguageCounts(context);
                            foreach (var item in movieLanguageCounts)
                            {
                                Console.WriteLine($"Movie: {item.MovieTitle}, Language Count: {item.LanguageCount}");
                            }
                            break;

                        case "10":
                            GetMovies(context);
                            break;

                        case "#":
                            continueProgram = false;
                            break;

                        default:
                            Console.WriteLine("Invalid option. Please try again.");
                            break;
                    }
                }
            }
        }

        //1
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

        //10
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

        //2
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

        //3
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

        //4
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

        //5
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

        //6
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


        //7
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


        //8
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

        //9
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
