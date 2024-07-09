using Microsoft.EntityFrameworkCore;
using TheaterEFCore.Models;


namespace TheaterEFCore
{
    public class AppDbContext : DbContext
    {

        public DbSet<Movie> Movies { get; set; }
        public DbSet<Country> Countries { get; set; }
        public DbSet<Author> Authors { get; set; }
        public DbSet<MoviesAuthors> MoviesAuthors { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Data Source=REVAN\\SQLEXPRESS;Initial Catalog= Theater ;Integrated Security=True;Trust Server Certificate=True;");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Country Configuration
            modelBuilder.Entity<Country>()
                .HasKey(c => c.CountryID);

            modelBuilder.Entity<Country>()
                .Property(c => c.CountryName)
                .IsRequired()
                .HasMaxLength(255);

            modelBuilder.Entity<Country>()
                .HasMany(c => c.Movies)
                .WithOne(m => m.Country)
                .HasForeignKey(m => m.CountryID);

            // Author Configuration
            modelBuilder.Entity<Author>()
                .HasKey(a => a.AuthorID);

            modelBuilder.Entity<Author>()
                .Property(a => a.AuthorName)
                .IsRequired()
                .HasMaxLength(255);

            // Movie Configuration
            modelBuilder.Entity<Movie>()
                .HasKey(m => m.MovieID);

            modelBuilder.Entity<Movie>()
                .Property(m => m.MovieTitle)
                .IsRequired()
                .HasMaxLength(50);

            modelBuilder.Entity<Movie>()
                .Property(m => m.ImdbRating)
                .IsRequired();

            modelBuilder.Entity<Movie>()
                .Property(m => m.YearReleased)
                .IsRequired();

            modelBuilder.Entity<Movie>()
                .Property(m => m.Budget)
                .HasPrecision(10, 2);

            modelBuilder.Entity<Movie>()
                .Property(m => m.BoxOffice)
                .HasPrecision(10, 2);

            modelBuilder.Entity<Movie>()
                .Property(m => m.Language)
                .IsRequired()
                .HasMaxLength(50);

            modelBuilder.Entity<Movie>()
                .HasOne(m => m.Country)
                .WithMany(c => c.Movies)
                .HasForeignKey(m => m.CountryID);

            // MoviesAuthors Configuration
            modelBuilder.Entity<MoviesAuthors>()
                .HasKey(ma => new { ma.MovieID, ma.AuthorID });

            modelBuilder.Entity<MoviesAuthors>()
                .HasOne(ma => ma.Movie)
                .WithMany(m => m.MoviesAuthors)
                .HasForeignKey(ma => ma.MovieID);

            modelBuilder.Entity<MoviesAuthors>()
                .HasOne(ma => ma.Author)
                .WithMany(a => a.MoviesAuthors)
                .HasForeignKey(ma => ma.AuthorID);

            base.OnModelCreating(modelBuilder);
        }
    }

}
    

