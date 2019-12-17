using System.Collections.Generic;
using System.Linq;
using Gighub.Models;

namespace Gighub.ViewModels
{
    public class HomeViewModel
    {
        public IEnumerable<Gig> UpcomingGigs { get; set; }
        public bool ShowActions { get; set; }
    }
}