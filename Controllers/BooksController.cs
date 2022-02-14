using LibApp.Models;
using LibApp.Repositories;
using LibApp.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;

namespace LibApp.Controllers
{
    public class BooksController : Controller
    {
        private readonly IBooksRepository _booksRepository;
        private readonly IGenreRepository _genreRepository;

        public BooksController(IBooksRepository booksRepository, IGenreRepository genreRepository)
        {
            _booksRepository = booksRepository;
            _genreRepository = genreRepository;
        }

        public IActionResult Index()
        {
            var books = _booksRepository.GetBooksWithGenre();

            return View(books);
        }

        public IActionResult Details(int id)
        {
            var book = _booksRepository.GetBookWithGenre(id);

            if (book == null)
            {
                return Content("Book not found");
            }

            return View(book);
        }

        public IActionResult Edit(int id)
        {
            var book = _booksRepository.GetBookOrDefault(id);
            if (book == null)
            {
                return NotFound();
            }

            var viewModel = new BookFormViewModel
            {
                Book = book,
                Genres = _genreRepository.GetGenres()
            };

            return View("BookForm", viewModel);
        }

        public IActionResult New()
        {
            var viewModel = new BookFormViewModel
            {
                Genres = _genreRepository.GetGenres()
            };

            return View("BookForm", viewModel);
        }

        [HttpPost]
        public IActionResult Save(Book book)
        {
            var errorViewModel = new BookFormViewModel
            {
                Book = book,
                Genres = _genreRepository.GetGenres()
            };

            if (!ModelState.IsValid)
            {
                return View("BookForm", errorViewModel);
            }

            if (book.Id == 0)
            {
                book.DateAdded = DateTime.Now;
                _booksRepository.AddBook(book);
            }
            else
            {
                var bookInDb = _booksRepository.GetBook(book.Id);
                bookInDb.Name = book.Name;
                bookInDb.AuthorName = book.AuthorName;
                bookInDb.GenreId = book.GenreId;
                bookInDb.NumberInStock = book.NumberInStock;
                bookInDb.ReleaseDate = book.ReleaseDate;
                bookInDb.DateAdded = book.DateAdded;
            }

            try
            {
                _booksRepository.SaveChanges();
            }
            catch (DbUpdateException e)
            {
                Console.WriteLine(e);
            }

            return RedirectToAction("Index", "Books");
        }



    }
}
