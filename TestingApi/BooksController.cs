using System;
using Microsoft.AspNetCore.Mvc;
// using T.Models;
// using MyWebAPI.Repositories;
using System.Collections.Generic;

namespace TestingApi
{
    public class BooksController : ControllerBase
    {
        private readonly BookRepository rep = new();

        [HttpGet]
        public ActionResult<List<Book>> Get()
        {
            return rep.GetAllBooks();
        }

        [HttpGet("{id}")]
        public ActionResult<Book> Get(int id)
        {
            var book = rep.GetById(id);
            if (book == null)
            {
                return NotFound();
            }
            return Ok();
        }

        [HttpPost]
        public ActionResult Add(Book book)
        {
            rep.AddBook(book);
            return Ok();
        }

        [HttpDelete("{id}")]
        public ActionResult Delete(int id)
        {
            rep.Remove(id);
            return Ok();
        }
    }
}