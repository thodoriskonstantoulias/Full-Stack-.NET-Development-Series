using System;
using System.Security.Claims;
using System.Security.Principal;
using System.Web.Http.Results;
using FluentAssertions;
using Gighub.Controllers.Api;
using Gighub.Models;
using Gighub.Persistence;
using Gighub.Repositories;
using Gighub.Tests.Extensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Gighub.Tests.Controllers.Api
{
    [TestClass]
    public class GigsControllerTests
    {
        private GigsController controller;
        private Mock<IGigRepository> mockRepository;

        [TestInitialize]
        public void TestInitialize()
        {
            mockRepository = new Mock<IGigRepository>();
            var mockUoW = new Mock<IUnitOfWork>();
            mockUoW.SetupGet(u => u.Gigs).Returns(mockRepository.Object);

            controller = new GigsController(mockUoW.Object);
            controller.MockCurrentUser("1", "user1@domain.com");
        }
        [TestMethod]
        public void Cancel_NoGivenId_NotFoundMessage()
        {
            var result = controller.Cancel(1);

            result.Should().BeOfType<NotFoundResult>(); 
        }

        [TestMethod]
        public void Cancel_CancelledGig_NotFoundMessage()
        {
            var gig = new Gig();
            gig.Cancel();

            mockRepository.Setup(r => r.GetGigWithAttendees(1)).Returns(gig);

            var result = controller.Cancel(1);

            result.Should().BeOfType<NotFoundResult>();
        }

        [TestMethod]
        public void Cancel_UserCancellingOthersGid_UnauthorizedMessage()
        {
            var gig = new Gig { ArtistId = "2"};

            mockRepository.Setup(r => r.GetGigWithAttendees(1)).Returns(gig);

            var result = controller.Cancel(1);

            result.Should().BeOfType<UnauthorizedResult>();
        }

        [TestMethod]
        public void Cancel_ValidRequest_ReturnsOk()
        {
            var gig = new Gig { ArtistId = "1" };

            mockRepository.Setup(r => r.GetGigWithAttendees(1)).Returns(gig);

            var result = controller.Cancel(1);

            result.Should().BeOfType<OkResult>();
        }
    }
}
