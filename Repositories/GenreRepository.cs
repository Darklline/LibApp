using LibApp.Data;
using LibApp.Models;
using System.Collections.Generic;
using System.Linq;

namespace LibApp.Repositories
{
    public class GenreRepository : IGenreRepository
    {
        private readonly ApplicationDbContext _context;

        public GenreRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public IEnumerable<Genre> GetGenres()
        {
            return _context.Genre.ToList();
        }
    }
}
