using AutoMapper;
using LibApp.Dtos;
using LibApp.Models;
using LibApp.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;

namespace LibApp.Controllers.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class BooksController : ControllerBase
    {


        private readonly IBooksRepository _booksRepository;
        private readonly IMapper _mapper;
        public BooksController(IBooksRepository booksRepository, IMapper mapper)
        {
            _booksRepository = booksRepository;
            _mapper = mapper;
        }

        [HttpGet]
        [Authorize(Roles = "Owner, StoreManager, User")]
        public ActionResult<IEnumerable<BookDto>> GetBooks()
        {
            var books = _booksRepository.GetBooksWithGenre();


            return Ok(_mapper.Map<IEnumerable<BookDto>>(books));
        }

        [HttpGet("details/{id}")]
        [Authorize(Roles = "Owner, StoreManager, User")]
        public IActionResult GetBookDetails(int id)
        {
            return Redirect("https://localhost:5001/books/details/" + id);
        }


        [HttpDelete("{id}")]
        [Authorize(Roles = "Owner, StoreManager")]
        public ActionResult<Book> RemoveBook(int id)
        {
            try
            {
                _booksRepository.RemoveBook(id);
                _booksRepository.SaveChanges();
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
