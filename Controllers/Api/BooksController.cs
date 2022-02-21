using AutoMapper;
using LibApp.Data;
using LibApp.Dtos;
using LibApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;

namespace LibApp.Controllers.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class BooksController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        public BooksController(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [HttpGet]
        [Authorize(Roles = "Owner, StoreManager, User")]
        public IEnumerable<BookDto> GetBooks(string query = null)
        {
            var booksQuery = _context.Books.Where(b => b.NumberAvailable > 0);
            var genres = _context.Genre.ToList();
            foreach (var book in booksQuery)
            {
                book.Genre = genres.Where(g => g.Id == book.GenreId).SingleOrDefault();
            }

            if (!String.IsNullOrWhiteSpace(query))
            {
                booksQuery = booksQuery.Where(b => b.Name.Contains(query));
            }

            return booksQuery.ToList().Select(_mapper.Map<Book, BookDto>);
        }

        [HttpGet("details/{id}")]
        [Authorize(Roles = "Owner, StoreManager, User")]
        public IActionResult GetCustomerDetails(int id)
        {
            return Redirect("https://localhost:5001/books/details/" + id);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Owner, StoreManager")]
        public ActionResult<Book> RemoveBook(int id)
        {
            try
            {
                _context.Books.Remove(new Book { Id = id });
                return Ok();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw new HttpRequestException(e.Message, e, HttpStatusCode.BadRequest);
            }
        }
    }
}
