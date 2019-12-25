using Gighub.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Data.Entity;
using Gighub.Persistence;

namespace Gighub.Controllers.Api
{
    [Authorize]
    public class GigsController : ApiController
    {
        private readonly IUnitOfWork unitOfWork;

        public GigsController(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }
        
        [HttpDelete] 
        public IHttpActionResult Cancel(int id)
        {
            var userId = User.Identity.GetUserId();
            var gig = unitOfWork.Gigs.GetGigWithAttendees(id);

            if (gig == null) return NotFound();
            if (gig.IsCancelled) return NotFound();
            if (gig.ArtistId != userId) return Unauthorized();

            //We find all users that attend this gig, to send them the notification of cancellation
            gig.Cancel();

            unitOfWork.Complete();

            return Ok();
        }
    }
}
