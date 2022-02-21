using LibApp.Data;
using LibApp.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace LibApp.Repositories
{
    public class BooksRepository : IBooksRepository
    {
        private readonly ApplicationDbContext _context;

        public BooksRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public IEnumerable<Book> GetBooksWithGenre()
        {
            return _context.Books
                 .Include(b => b.Genre)
                 .ToList();
        }

        public Book GetBookWithGenre(int id)
        {
            return _context.Books
                .Include(b => b.Genre)
                .SingleOrDefault(b => b.Id == id);
        }

        public Book GetBookOrDefault(int id)
        {
            return _context.Books.SingleOrDefault(b => b.Id == id);
        }

        public Book GetBook(int id)
        {
            return _context.Books.Single(b => b.Id == id);
        }

        public void AddBook(Book book)
        {
            _context.Books.Add(book);
        }

        public void SaveChanges()
        {
            _context.SaveChanges();
        }

        public void RemoveBook(int id)
        {
            _context.Remove(GetBook(id));
        }
    }
}
