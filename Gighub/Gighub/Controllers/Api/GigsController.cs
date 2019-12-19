using Gighub.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

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
            var gig = _context.Gigs.Single(g => g.Id == id && g.ArtistId == userId);

            if (gig.IsCancelled) return NotFound();

            gig.IsCancelled = true;

            var notification = new Notification(NotificationType.GigCancelled, gig);

            //We find all users that attend this gig, to send them the notification
            var attendees = _context.Attendances.Where(g => g.GigId == gig.Id).Select(a =>a.Attendee).ToList();
            foreach (var attendee in attendees)
            {
                attendee.Notify(notification);
            }

            _context.SaveChanges();
            return Ok();
        }
    }
}
