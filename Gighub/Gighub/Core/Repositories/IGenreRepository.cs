using System.Collections.Generic;
using Gighub.Models;

namespace Gighub.Repositories
{
    public interface IGenreRepository
    {
        IEnumerable<Genre> GetGenre();
    }
}