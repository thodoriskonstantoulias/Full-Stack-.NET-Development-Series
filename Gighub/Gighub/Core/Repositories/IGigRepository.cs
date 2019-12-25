using System.Collections.Generic;
using Gighub.Models;

namespace Gighub.Repositories
{
    public interface IGigRepository
    {
        void Add(Gig gig);
        Gig GetGig(int id);
        IEnumerable<Gig> GetGigsUserAttending(string userId);
        Gig GetGigWithAttendees(int gigId);
        IEnumerable<Gig> GetUpcomingGigsByArtist(string userId);
    }
}