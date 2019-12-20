using Gighub.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Data.Entity;

namespace Gighub.Controllers.Api
{
    [Authorize]
    public class GigsController : ApiController
    {
        private readonly ApplicationDbContext _context;
        public GigsController()
        {
            _context = new ApplicationDbContext();
        }
        
        [HttpDelete] 
        public IHttpActionResult Cancel(int id)
        {
            var userId = User.Identity.GetUserId();
            var gig = _context.Gigs.Include(c => c.Attendances.Select(a =>a.Attendee)).Single(g => g.Id == id && g.ArtistId == userId);

            if (gig.IsCancelled) return NotFound();

            //We find all users that attend this gig, to send them the notification of cancellation
            gig.Cancel();           

            _context.SaveChanges();
            return Ok();
        }
    }
}
