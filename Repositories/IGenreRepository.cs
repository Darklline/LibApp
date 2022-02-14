using LibApp.Models;
using System.Collections.Generic;

namespace LibApp.Repositories
{
    public interface IGenreRepository
    {
        IEnumerable<Genre> GetGenres();
    }
}