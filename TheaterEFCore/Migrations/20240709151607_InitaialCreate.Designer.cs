﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using TheaterEFCore;

#nullable disable

namespace TheaterEFCore.Migrations
{
    [DbContext(typeof(AppDbContext))]
    [Migration("20240709151607_InitaialCreate")]
    partial class InitaialCreate
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.7")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("TheaterEF.Models.Author", b =>
                {
                    b.Property<int>("AuthorID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("AuthorID"));

                    b.Property<string>("AuthorName")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.HasKey("AuthorID");

                    b.ToTable("Authors");
                });

            modelBuilder.Entity("TheaterEF.Models.Country", b =>
                {
                    b.Property<int>("CountryID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("CountryID"));

                    b.Property<string>("CountryName")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.HasKey("CountryID");

                    b.ToTable("Countries");
                });

            modelBuilder.Entity("TheaterEF.Models.Movie", b =>
                {
                    b.Property<int>("MovieID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("MovieID"));

                    b.Property<decimal>("BoxOffice")
                        .HasPrecision(18, 2)
                        .HasColumnType("decimal(18,2)");

                    b.Property<decimal>("Budget")
                        .HasPrecision(18, 2)
                        .HasColumnType("decimal(18,2)");

                    b.Property<int>("CountryID")
                        .HasColumnType("int");

                    b.Property<float>("ImdbRating")
                        .HasColumnType("real");

                    b.Property<string>("Language")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<string>("MovieTitle")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<int>("YearReleased")
                        .HasColumnType("int");

                    b.HasKey("MovieID");

                    b.HasIndex("CountryID");

                    b.ToTable("Movies");
                });

            modelBuilder.Entity("TheaterEF.Models.MoviesAuthors", b =>
                {
                    b.Property<int>("MovieID")
                        .HasColumnType("int");

                    b.Property<int>("AuthorID")
                        .HasColumnType("int");

                    b.HasKey("MovieID", "AuthorID");

                    b.HasIndex("AuthorID");

                    b.ToTable("MoviesAuthors");
                });

            modelBuilder.Entity("TheaterEF.Models.Movie", b =>
                {
                    b.HasOne("TheaterEF.Models.Country", "Country")
                        .WithMany("Movies")
                        .HasForeignKey("CountryID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Country");
                });

            modelBuilder.Entity("TheaterEF.Models.MoviesAuthors", b =>
                {
                    b.HasOne("TheaterEF.Models.Author", "Author")
                        .WithMany("MoviesAuthors")
                        .HasForeignKey("AuthorID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("TheaterEF.Models.Movie", "Movie")
                        .WithMany("MoviesAuthors")
                        .HasForeignKey("MovieID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Author");

                    b.Navigation("Movie");
                });

            modelBuilder.Entity("TheaterEF.Models.Author", b =>
                {
                    b.Navigation("MoviesAuthors");
                });

            modelBuilder.Entity("TheaterEF.Models.Country", b =>
                {
                    b.Navigation("Movies");
                });

            modelBuilder.Entity("TheaterEF.Models.Movie", b =>
                {
                    b.Navigation("MoviesAuthors");
                });
#pragma warning restore 612, 618
        }
    }
}
