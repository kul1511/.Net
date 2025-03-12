using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TestingApi
{
    public class BookRepository
    {
        private readonly List<Book> books = new List<Book>();

        public List<Book> GetAllBooks()
        {
            return books;
        }
        public void AddBook(Book book)
        {
            books.Add(book);
        }
        public void Remove(int id)
        {
            books.RemoveAll(x => x.Id == id);
        }
        public Book GetById(int id)
        {
            return books.FirstOrDefault(x => x.Id == id);
        }
    }
}