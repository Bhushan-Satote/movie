using System;
using System.Collections.Generic;
using System.Linq;
using MovieBooking.Models;

namespace MovieBooking.Services
{
    public static class MovieDataService
    {
        public static List<Movie> Movies { get; set; } = new List<Movie>();
        public static List<Theater> Theaters { get; set; } = new List<Theater>();

        static MovieDataService()
        {
            // Initialize sample movies
            Movies = new List<Movie>
            {
                new Movie
                {
                    Id = 1,
                    Title = "The Dark Knight",
                    Description = "When the menace known as the Joker wreaks havoc and chaos on the people of Gotham, Batman must accept one of the greatest psychological and physical tests of his ability to fight injustice.",
                    Genre = "Action, Crime, Drama",
                    Duration = 152,
                    Price = 120.00M,                    
                    PosterURL = "/images/movie1.jpg",
                    Language = "English",
                    Rating = 9.0,
                    ReleaseDate = DateTime.Now.AddDays(-30),
                    Director = "Christopher Nolan",
                    Cast = "Christian Bale, Heath Ledger, Aaron Eckhart",
                    IsNowShowing = true
                },
                new Movie
                {
                    Id = 2,
                    Title = "Inception",
                    Description = "A thief who steals corporate secrets through the use of dream-sharing technology is given the inverse task of planting an idea into the mind of a C.E.O.",
                    Genre = "Action, Adventure, Sci-Fi",
                    Duration = 148,
                    Price = 140.00M,
                    PosterURL = "/images/Movie2.jpg",
                    Language = "English",
                    Rating = 8.8,
                    ReleaseDate = DateTime.Now.AddDays(-15),
                    Director = "Christopher Nolan",
                    Cast = "Leonardo DiCaprio, Joseph Gordon-Levitt, Ellen Page",
                    IsNowShowing = false
                }
            };

            // Initialize sample theaters
            Theaters = new List<Theater>
            {
                new Theater
                {
                    Id = 1,
                    Name = "Cineplex Central",
                    Location = "Downtown Mall, City Center",
                    Capacity = 200,
                    ScreenType = "IMAX",
                    IsActive = true
                },
                new Theater
                {
                    Id = 2,
                    Name = "MovieMax Plaza",
                    Location = "West End Shopping Complex",
                    Capacity = 150,
                    ScreenType = "3D",
                    IsActive = true
                }
            };
        }

        // CRUD Operations for Movies
        public static void AddMovie(Movie movie)
        {
            movie.Id = Movies.Count + 1;
            Movies.Add(movie);
        }

        public static void UpdateMovie(Movie movie)
        {
            var existingMovie = Movies.FirstOrDefault(m => m.Id == movie.Id);
            if (existingMovie != null)
            {
                Movies.Remove(existingMovie);
                Movies.Add(movie);
            }
        }

        public static void DeleteMovie(int id)
        {
            var movie = Movies.FirstOrDefault(m => m.Id == id);
            if (movie != null)
            {
                Movies.Remove(movie);
            }
        }

        // CRUD Operations for Theaters
        public static void AddTheater(Theater theater)
        {
            theater.Id = Theaters.Count + 1;
            Theaters.Add(theater);
        }

        public static void UpdateTheater(Theater theater)
        {
            var existingTheater = Theaters.FirstOrDefault(t => t.Id == theater.Id);
            if (existingTheater != null)
            {
                Theaters.Remove(existingTheater);
                Theaters.Add(theater);
            }
        }

        public static void DeleteTheater(int id)
        {
            var theater = Theaters.FirstOrDefault(t => t.Id == id);
            if (theater != null)
            {
                Theaters.Remove(theater);
            }
        }
    }
} 