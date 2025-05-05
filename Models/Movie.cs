using System;
using System.ComponentModel.DataAnnotations;

namespace MovieBooking.Models
{
    public class Movie
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Title { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        public string Genre { get; set; }

        [Required]
        public int Duration { get; set; } // in minutes

        [Required]
        [Range(0, 10000)]
        public decimal Price { get; set; }

        public string PosterURL { get; set; }

        public string Language { get; set; }

        [Range(0, 10)]
        public double Rating { get; set; }

        public DateTime ReleaseDate { get; set; }

        public string Director { get; set; }

        public string Cast { get; set; }

        public bool IsNowShowing { get; set; }
    }
} 