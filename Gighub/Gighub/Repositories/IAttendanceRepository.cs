using System.Collections.Generic;
using Gighub.Models;

namespace Gighub.Repositories
{
    public interface IAttendanceRepository
    {
        bool GetAttendence(Gig gig, string userId);
        IEnumerable<Attendance> GetFutureAttendances(string userId);
    }
}