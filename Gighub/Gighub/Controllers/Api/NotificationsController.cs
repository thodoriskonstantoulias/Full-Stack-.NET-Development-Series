using Gighub.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Data.Entity;
using Gighub.Dtos;
using AutoMapper;

namespace Gighub.Controllers.Api
{
    [Authorize]
    public class NotificationsController : ApiController
    {
        private readonly ApplicationDbContext _context;
        public NotificationsController()
        {
            _context = new ApplicationDbContext();
        }

        public IEnumerable<NotificationDto> GetNewNotifications()
        {
            var userId = User.Identity.GetUserId();
            var notifications = _context.UserNotifications
                .Where(n => n.UserId == userId)
                .Select(n => n.Notification)
                .Include(n => n.Gig.Artist)
                .ToList();

            //Using auto mapper for mapping the domain model fields with the DTOs
            Mapper.CreateMap<ApplicationUser, UserDto>();
            Mapper.CreateMap<Gig, GigDto>();
            Mapper.CreateMap<Notification, NotificationDto>();

            //return notifications.Select(n => new NotificationDto()
            //{ 
            //    DateTime = n.DateTime,
            //    Gig = new GigDto 
            //    {
            //        Artist = new UserDto 
            //        {
            //            Id = n.Gig.Artist.Id,
            //            Name = n.Gig.Artist.Name
            //        },
            //        DateTime = n.Gig.DateTime,
            //        Id = n.Gig.Id,
            //        IsCancelled = n.Gig.IsCancelled,
            //        Venue = n.Gig.Venue
            //    },
            //    OriginalDateTime = n.OriginalDateTime,
            //    OriginalVenue = n.OriginalVenue,
            //    Type = n.Type
            //});

            //much cleaner with auto mapper
            return notifications.Select(Mapper.Map<Notification, NotificationDto>);
        }
    }
}
