using System;
using System.Collections.Generic;

namespace MovieBooking.Models
{
    public class BookingDetails
    {
        public string MovieTitle { get; set; }
        public string TheaterName { get; set; }
        public DateTime Showtime { get; set; }
        public List<string> Seats { get; set; }
        public decimal TotalAmount { get; set; }
    }
}