﻿using Gighub.Models;
using Gighub.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Gighub.Persistence
{
    public class UnitOfWork
    {
        private readonly ApplicationDbContext _context;
        public GigRepository Gigs { get; private set; }
        public AttendanceRepository Attendances { get; private set; }
        public FollowingRepository Followings { get; private set; }
        public GenreRepository Genres { get; private set; }

        public UnitOfWork(ApplicationDbContext context)
        {
            _context = context;
            Gigs = new GigRepository(_context);
            Attendances = new AttendanceRepository(_context);
            Followings = new FollowingRepository(_context);
            Genres = new GenreRepository(_context);
        }
        public void Complete()
        {
            _context.SaveChanges();
        }
    }
}