using LibApp.Models;
using System.Collections.Generic;

namespace LibApp.Repositories
{
    public interface IBooksRepository
    {
        void SaveChanges();
        void AddBook(Book book);
        Book GetBookOrDefault(int id);
        public Book GetBook(int id);
        IEnumerable<Book> GetBooksWithGenre();
        Book GetBookWithGenre(int id);
        void RemoveBook(int id);
    }
}