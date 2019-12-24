using Gighub.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Gighub.Repositories
{
    public class FollowingRepository
    {
        private readonly ApplicationDbContext _context;
        public FollowingRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public bool GetFollowing(Gig gig, string userId)
        {
            return _context.Followings.Any(u => u.FollowerId == userId && u.FolloweeId == gig.ArtistId);
        }
    }
}