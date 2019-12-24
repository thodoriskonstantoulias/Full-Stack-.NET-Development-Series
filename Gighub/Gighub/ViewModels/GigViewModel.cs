using System.Collections.Generic;
using System.Linq;
using Gighub.Models;

namespace Gighub.ViewModels
{
    public class GigViewModel
    {
        public IEnumerable<Gig> UpcomingGigs { get; set; }
        public bool ShowActions { get; set; }
        public string Heading { get; set; }
        public string SearchTerm { get; set; }

    }
}