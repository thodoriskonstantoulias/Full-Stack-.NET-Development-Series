using FluentAssertions;
using Gighub.Models;
using Gighub.Repositories;
using Gighub.Tests.Extensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gighub.Tests.Persistence.Repositories
{  
    [TestClass]
    public class GigRepositoryTests
    {
        private GigRepository _repository;
        private Mock<DbSet<Gig>> mockGigs;

        [TestInitialize]
        public void TestInitialize()
        {
            mockGigs = new Mock<DbSet<Gig>>();
            var mockContext = new Mock<IApplicationDbContext>();
            mockContext.SetupGet(c => c.Gigs).Returns(mockGigs.Object);

            _repository = new GigRepository(mockContext.Object);
        }

        [TestMethod]
        public void GetUpcomingGigs_GigInThePast_NotReturned()
        {
            var gig = new Gig { DateTime = DateTime.Now.AddDays(-1) , ArtistId = "1"};
            mockGigs.SetSource(new List<Gig> { gig });
            var result = _repository.GetUpcomingGigsByArtist("1");

            result.Should().BeEmpty();
        }

        [TestMethod]
        public void GetUpcomingGigs_GigIsCancelled_NotReturned()
        {
            var gig = new Gig { DateTime = DateTime.Now.AddDays(1), ArtistId = "1" };
            gig.Cancel();

            mockGigs.SetSource(new List<Gig> { gig });
            var result = _repository.GetUpcomingGigsByArtist("1");

            result.Should().BeEmpty();
        }

        [TestMethod]
        public void GetUpcomingGigs_GigIsForDifferentArtist_NotReturned()
        {
            var gig = new Gig { DateTime = DateTime.Now.AddDays(1), ArtistId = "2" };

            mockGigs.SetSource(new List<Gig> { gig });
            var result = _repository.GetUpcomingGigsByArtist("1");

            result.Should().BeEmpty();
        }

        [TestMethod]
        public void GetUpcomingGigs_Valid_ShouldReturned()
        {
            var gig = new Gig { DateTime = DateTime.Now.AddDays(1), ArtistId = "1" };

            mockGigs.SetSource(new List<Gig> { gig });
            var result = _repository.GetUpcomingGigsByArtist("1");

            result.Should().Contain(gig);
        }
    }
}
