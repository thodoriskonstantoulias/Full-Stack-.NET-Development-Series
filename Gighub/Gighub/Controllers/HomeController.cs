using Gighub.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using Gighub.ViewModels;
using Microsoft.AspNet.Identity;
using Gighub.Repositories;

namespace Gighub.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly AttendanceRepository attendanceRepository;
        public HomeController()
        {
            _context = new ApplicationDbContext();
            attendanceRepository = new AttendanceRepository(_context);
        } 
        public ActionResult Index(string query = null)
        {
            var upcomingGigs = _context.Gigs.Include(g => g.Artist).Include(g => g.Genre).Where(g => g.DateTime > DateTime.Now && !g.IsCancelled);
            if (!string.IsNullOrWhiteSpace(query))
            {
                upcomingGigs = upcomingGigs.Where(g =>
                               g.Artist.Name.Contains(query) ||
                               g.Genre.Name.Contains(query) ||
                               g.Venue.Contains(query));
            }

            var userId = User.Identity.GetUserId();
            var attendances = attendanceRepository.GetFutureAttendances(userId).ToLookup(a => a.GigId);

            var viewModel = new GigViewModel
            {
                UpcomingGigs = upcomingGigs,
                ShowActions = User.Identity.IsAuthenticated,
                Heading = "Upcoming gigs",
                SearchTerm = query,
                Attendances = attendances
            };

            return View("Gigs",viewModel);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}