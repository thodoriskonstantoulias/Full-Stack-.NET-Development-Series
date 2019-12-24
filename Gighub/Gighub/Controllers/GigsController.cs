using Gighub.Models;
using Gighub.Repositories;
using Gighub.ViewModels;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;

namespace Gighub.Controllers
{
    public class GigsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly AttendanceRepository attendanceRepository;
        private readonly GigRepository gigRepository;
        private readonly FollowingRepository followingRepository;
        private readonly GenreRepository genreRepository;

        public GigsController()
        {
            _context = new ApplicationDbContext();
            attendanceRepository = new AttendanceRepository(_context);
            gigRepository = new GigRepository(_context);
            followingRepository = new FollowingRepository(_context);
            genreRepository = new GenreRepository(_context);
        }

        [Authorize]
        public ActionResult Mine()
        {
            var userId = User.Identity.GetUserId();
            var gigs = gigRepository.GetUpcomingGigsByArtist(userId);

            return View(gigs);
        }

        

        [Authorize]
        public ActionResult Attending()
        {
            var userId = User.Identity.GetUserId();
            var gigs = gigRepository.GetGigsUserAttending(userId);

            var attendances = attendanceRepository.GetFutureAttendances(userId).ToLookup(a => a.GigId);

            var viewModel = new GigViewModel
            {
                UpcomingGigs = gigs,
                ShowActions = User.Identity.IsAuthenticated,
                Heading = "Gigs I'm attending",
                Attendances = attendances
            };

            return View("Gigs", viewModel);
        }

        [Authorize]
        public ActionResult Create()
        {
            var viewModel = new GigFormViewModel()
            {
                Genres = genreRepository.GetGenre(),
                Heading = "Add a Gig"
            };

            return View("GigForm", viewModel);
        }

        

        [Authorize]
        public ActionResult Edit(int id)
        {
            var userId = User.Identity.GetUserId();
            var gig = gigRepository.GetGig(id);

            if (gig == null) return HttpNotFound();
            if (gig.ArtistId != userId) return new HttpUnauthorizedResult();

            var viewModel = new GigFormViewModel()
            {
                Genres = _context.Genres.ToList(),
                Id = gig.Id,
                Date = gig.DateTime.ToString("d MMM yyyy"),
                Time = gig.DateTime.ToString("HH:mm"),
                Venue = gig.Venue,
                Genre = gig.GenreId,
                Heading = "Edit a Gig"
            };

            return View("GigForm", viewModel);
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(GigFormViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                viewModel.Genres = _context.Genres.ToList();
                return View("GigForm", viewModel);
            }

            var gig = new Gig
            {
                ArtistId = User.Identity.GetUserId(),
                DateTime = viewModel.GetDateTime(),
                GenreId = viewModel.Genre,
                Venue = viewModel.Venue
            };

            _context.Gigs.Add(gig);
            _context.SaveChanges();

            return RedirectToAction("Mine", "Gigs");
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Update(GigFormViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                viewModel.Genres = _context.Genres.ToList();
                return View("GigForm", viewModel);
            }

            var userId = User.Identity.GetUserId();
            var gig = gigRepository.GetGigWithAttendees(viewModel.Id);

            if (gig == null) return HttpNotFound();
            if (gig.ArtistId != userId) return new HttpUnauthorizedResult();

            gig.Modify(viewModel.GetDateTime(), viewModel.Venue, viewModel.Genre);

            _context.SaveChanges();

            return RedirectToAction("Mine", "Gigs");
        }

        [HttpPost]
        public ActionResult Search(GigViewModel viewModel)
        {
            return RedirectToAction("Index", "Home", new { query = viewModel.SearchTerm });
        }

        
        public ActionResult Details(int id)
        {
            var gig = gigRepository.GetGig(id);

            if (gig == null) return HttpNotFound();

            var viewModel = new GigDetailsViewModel { Gig = gig };
            if (User.Identity.IsAuthenticated)
            {
                var userId = User.Identity.GetUserId();
                viewModel.IsAttending = attendanceRepository.GetAttendence(gig, userId);
                viewModel.IsFollowing = followingRepository.GetFollowing(gig, userId);
            }

            return View(viewModel);
        }

        


    }
}