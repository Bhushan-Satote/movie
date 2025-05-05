using Microsoft.AspNetCore.Mvc;
using MovieBooking.Models;
using MovieBooking.Services;

namespace MovieBooking.Controllers
{
    public class AdminController : Controller
    {
        public IActionResult Index()
        {
            ViewBag.MovieCount = MovieDataService.Movies.Count;
            ViewBag.TheaterCount = MovieDataService.Theaters.Count;
            ViewData["PageTitle"] = "Admin Dashboard";
            TempData["AdminMessage"] = "Welcome to the Admin Panel";
            
            return View();
        }

        public IActionResult Movies()
        {
            ViewBag.Movies = MovieDataService.Movies;
            ViewData["PageTitle"] = "Manage Movies";
            return View();
        }

        public IActionResult Theaters()
        {
            ViewBag.Theaters = MovieDataService.Theaters;
            ViewData["PageTitle"] = "Manage Theaters";
            return View();
        }

        [HttpGet]
        public IActionResult AddMovie()
        {
            ViewData["Action"] = "Add";
            return View("MovieForm");
        }

        [HttpPost]
        public IActionResult AddMovie(Movie movie)
        {
            if (ModelState.IsValid)
            {
                MovieDataService.AddMovie(movie);
                TempData["SuccessMessage"] = "Movie added successfully!";
                return RedirectToAction("Movies");
            }
            ViewData["Action"] = "Add";
            return View("MovieForm", movie);
        }

        [HttpGet]
        public IActionResult EditMovie(int id)
        {
            var movie = MovieDataService.Movies.FirstOrDefault(m => m.Id == id);
            if (movie == null)
            {
                return NotFound();
            }
            ViewData["Action"] = "Edit";
            return View("MovieForm", movie);
        }

        [HttpPost]
        public IActionResult EditMovie(Movie movie)
        {
            if (ModelState.IsValid)
            {
                MovieDataService.UpdateMovie(movie);
                TempData["SuccessMessage"] = "Movie updated successfully!";
                return RedirectToAction("Movies");
            }
            ViewData["Action"] = "Edit";
            return View("MovieForm", movie);
        }

        [HttpPost]
        public IActionResult DeleteMovie(int id)
        {
            MovieDataService.DeleteMovie(id);
            TempData["SuccessMessage"] = "Movie deleted successfully!";
            return RedirectToAction("Movies");
        }

        [HttpGet]
        public IActionResult AddTheater()
        {
            ViewData["Action"] = "Add";
            return View("TheaterForm");
        }

        [HttpPost]
        public IActionResult AddTheater(Theater theater)
        {
            if (ModelState.IsValid)
            {
                MovieDataService.AddTheater(theater);
                TempData["SuccessMessage"] = "Theater added successfully!";
                return RedirectToAction("Theaters");
            }
            ViewData["Action"] = "Add";
            return View("TheaterForm", theater);
        }

        [HttpGet]
        public IActionResult EditTheater(int id)
        {
            var theater = MovieDataService.Theaters.FirstOrDefault(t => t.Id == id);
            if (theater == null)
            {
                return NotFound();
            }
            ViewData["Action"] = "Edit";
            return View("TheaterForm", theater);
        }

        [HttpPost]
        public IActionResult EditTheater(Theater theater)
        {
            if (ModelState.IsValid)
            {
                MovieDataService.UpdateTheater(theater);
                TempData["SuccessMessage"] = "Theater updated successfully!";
                return RedirectToAction("Theaters");
            }
            ViewData["Action"] = "Edit";
            return View("TheaterForm", theater);
        }

        [HttpPost]
        public IActionResult DeleteTheater(int id)
        {
            MovieDataService.DeleteTheater(id);
            TempData["SuccessMessage"] = "Theater deleted successfully!";
            return RedirectToAction("Theaters");
        }
    }
} 