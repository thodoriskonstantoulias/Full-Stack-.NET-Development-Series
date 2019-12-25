using Gighub.Models;

namespace Gighub.Repositories
{
    public interface IFollowingRepository
    {
        bool GetFollowing(Gig gig, string userId);
    }
}