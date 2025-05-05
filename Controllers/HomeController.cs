using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using MovieBooking.Models;
using MovieBooking.Services;
using System.Text.Json;

namespace MovieBooking.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    public IActionResult Index()
    {
        var nowShowingMovies = MovieDataService.Movies.Where(m => m.IsNowShowing).ToList();
        var comingSoonMovies = MovieDataService.Movies.Where(m => !m.IsNowShowing).ToList();
        
        ViewBag.NowShowingMovies = nowShowingMovies;
        ViewBag.ComingSoonMovies = comingSoonMovies;
        return View();
    }

    public IActionResult MovieDetails(int id)
    {
        var movie = MovieDataService.Movies.FirstOrDefault(m => m.Id == id);
        if (movie == null)
        {
            return NotFound();
        }
        
        ViewBag.Theaters = MovieDataService.Theaters.Where(t => t.IsActive).ToList();
        ViewBag.LoginSuccess = TempData["LoginSuccess"]?.ToString();
        return View(movie);
    }

    public IActionResult SelectShowtime(int movieId, int theaterId)
    {
        var movie = MovieDataService.Movies.FirstOrDefault(m => m.Id == movieId);
        var theater = MovieDataService.Theaters.FirstOrDefault(t => t.Id == theaterId);
        
        if (movie == null || theater == null)
        {
            return NotFound();
        }

        ViewBag.Movie = movie;
        ViewBag.Theater = theater;
        // Generate sample showtimes for the next 7 days
        var showtimes = new List<DateTime>();
        for (int i = 0; i < 7; i++)
        {
            var date = DateTime.Today.AddDays(i);
            showtimes.Add(date.AddHours(10)); // 10 AM
            showtimes.Add(date.AddHours(13)); // 1 PM
            showtimes.Add(date.AddHours(16)); // 4 PM
            showtimes.Add(date.AddHours(19)); // 7 PM
        }
        ViewBag.Showtimes = showtimes;
        
        return View();
    }

    public IActionResult SeatSelection(int movieId, int theaterId, DateTime showtime)
    {
        var movie = MovieDataService.Movies.FirstOrDefault(m => m.Id == movieId);
        var theater = MovieDataService.Theaters.FirstOrDefault(t => t.Id == theaterId);
        
        if (movie == null || theater == null)
        {
            return NotFound();
        }

        ViewBag.Movie = movie;
        ViewBag.Theater = theater;
        ViewBag.Showtime = showtime;
        
        // Generate sample seat layout
        var seatLayout = new List<string>();
        string[] rows = { "A", "B", "C", "D", "E", "F", "G", "H" };
        foreach (var row in rows)
        {
            for (int i = 1; i <= 10; i++)
            {
                seatLayout.Add($"{row}{i}");
            }
        }
        ViewBag.SeatLayout = seatLayout;
        
        return View();
    }

    [HttpPost]
    public IActionResult BookTickets(int movieId, int theaterId, DateTime showtime, List<string> selectedSeats)
    {
        if (selectedSeats == null || !selectedSeats.Any())
        {
            return BadRequest("Please select at least one seat.");
        }

        var movie = MovieDataService.Movies.FirstOrDefault(m => m.Id == movieId);
        var theater = MovieDataService.Theaters.FirstOrDefault(t => t.Id == theaterId);
        
        if (movie == null || theater == null)
        {
            return NotFound();
        }

        // Create a BookingDetails object instead of anonymous type
        var bookingDetails = new BookingDetails
        {
            MovieTitle = movie.Title,
            TheaterName = theater.Name,
            Showtime = showtime,
            Seats = selectedSeats,
            TotalAmount = selectedSeats.Count * movie.Price
        };

        // Store the BookingDetails object in TempData
        TempData["BookingDetails"] = System.Text.Json.JsonSerializer.Serialize(bookingDetails);

        return RedirectToAction("BookingConfirmation");
    }

    public IActionResult BookingConfirmation()
    {
        if (TempData["BookingDetails"] == null)
        {
            return RedirectToAction("Index");
        }
        
        // Deserialize the BookingDetails object from TempData
        var bookingDetailsJson = TempData["BookingDetails"]?.ToString();
        var bookingDetails = System.Text.Json.JsonSerializer.Deserialize<BookingDetails>(bookingDetailsJson);
        ViewBag.BookingDetails = bookingDetails;
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }

    [HttpGet]
    public IActionResult Login(int? movieId)
    {
        if (TempData["Users"] != null)
        {
            TempData.Keep("Users");
        }
        
        var user = new User();
        if (TempData["AutoFillPhone"] != null)
        {
            user.PhoneNumber = TempData["AutoFillPhone"].ToString();
        }
        
        ViewBag.MovieId = movieId;
        ViewBag.Message = TempData["RegistrationSuccess"]?.ToString();
        ViewBag.IsSuccess = !string.IsNullOrEmpty(ViewBag.Message);
        return View(user);
    }

    [HttpPost]
    public IActionResult Login([FromForm] User user, int? movieId)
    {
        var usersJson = TempData["Users"] as string;
        TempData.Keep("Users");
    
        if (!string.IsNullOrEmpty(usersJson))
        {
            var users = JsonSerializer.Deserialize<List<User>>(usersJson);
            var matchedUser = users.FirstOrDefault(u => 
                u.PhoneNumber == user.PhoneNumber && 
                u.Password == user.Password);
    
            if (matchedUser != null)
            {
                HttpContext.Session.SetString("CurrentUser", matchedUser.Username);
                
                if (movieId.HasValue)
                {
                    TempData["LoginSuccess"] = "Login successful!";
                    return RedirectToAction("MovieDetails", new { id = movieId.Value });
                }
                return RedirectToAction("Index");
            }
        }
    
        ViewBag.IsSuccess = false;
        ViewBag.Message = "Invalid phone number or password.";
        ViewBag.MovieId = movieId;
        return View(user);
    }

    [HttpGet]
    public IActionResult Registration(int? movieId)
    {
        ViewBag.MovieId = movieId;
        return View(new User());
    }

    [HttpPost]
    public IActionResult Registration(User user, int? movieId)
    {
        if (ModelState.IsValid)
        {
            var usersJson = TempData["Users"] as string;
            var users = string.IsNullOrEmpty(usersJson) 
                ? new List<User>() 
                : JsonSerializer.Deserialize<List<User>>(usersJson);
    
            users.Add(user);
            TempData["Users"] = JsonSerializer.Serialize(users);
            TempData.Keep("Users");
            
            // Store success message and automatically fill login form
            TempData["RegistrationSuccess"] = "Registration successful! Please login to continue.";
            TempData["AutoFillPhone"] = user.PhoneNumber;
            
            return RedirectToAction("Login", new { movieId });
        }
    
        ViewBag.MovieId = movieId;
        return View(user);
    }

    [HttpGet]
    public IActionResult GetMovieDetails(int id)
    {
        var movie = MovieDataService.Movies.FirstOrDefault(m => m.Id == id);
        if (movie == null)
        {
            return NotFound();
        }
        ViewBag.Theaters = MovieDataService.Theaters.Where(t => t.IsActive).ToList();
        return PartialView("_MovieDetailsPartial", movie);
    }
}
