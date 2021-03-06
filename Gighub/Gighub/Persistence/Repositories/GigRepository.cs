﻿using Gighub.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace Gighub.Repositories
{
    public class GigRepository : IGigRepository
    {
        private readonly IApplicationDbContext _context;
        public GigRepository(IApplicationDbContext context)
        {
            _context = context;
        }

        public Gig GetGigWithAttendees(int gigId)
        {
            return _context.Gigs
                .Include(g => g.Attendances.Select(a => a.Attendee))
                .SingleOrDefault(g => g.Id == gigId);
        }

        public IEnumerable<Gig> GetGigsUserAttending(string userId)
        {
            return _context.Attendances.Where(a => a.AttendeeId == userId)
                                           .Select(a => a.Gig)
                                           .Include(g => g.Artist)
                                           .Include(g => g.Genre)
                                           .ToList();
        }

        public Gig GetGig(int id)
        {
            return _context.Gigs.Include(x => x.Artist).Include(x => x.Genre).SingleOrDefault(g => g.Id == id);
        }

        public IEnumerable<Gig> GetUpcomingGigsByArtist(string userId)
        {
            return _context.Gigs.Where(g => g.ArtistId == userId &&
                                                g.DateTime > DateTime.Now &&
                                                !g.IsCancelled)
                                                .Include(g => g.Genre).ToList();
        }

        public void Add(Gig gig)
        {
            _context.Gigs.Add(gig);
        }
    }
}