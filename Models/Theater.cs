using System.ComponentModel.DataAnnotations;

namespace MovieBooking.Models
{
    public class Theater
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;

        [Required]
        public string Location { get; set; } = string.Empty;

        [Required]
        [Range(50, 500)]
        public int Capacity { get; set; }

        public string ScreenType { get; set; } = string.Empty; // 2D, 3D, IMAX

        public bool IsActive { get; set; }
    }
} 