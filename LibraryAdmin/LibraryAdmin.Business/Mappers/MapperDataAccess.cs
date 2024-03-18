using LibraryAdmin.Business.Models;
using LibraryAdmin.DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryAdmin.Business.Mappers
{
    public static class MapperDataAccess
    {
        public static BookEntity MapToBookEntity(Book book)
        {
            var bookEntity = new BookEntity()
            {
                Id = book.Id,
                Title = book.Title,
                Year = book.Year,
                AuthorId = book.AuthorId,
                BooksAmount = book.BooksAmount,
                CreatedDate = DefaultDateProvider.Now()
            };
            return bookEntity;
        }
        public static Book MapToBook(BookEntity bookEntity)
        {
            var book = new Book()
            {
                Id=bookEntity.Id,
                Title = bookEntity.Title,
                Year=bookEntity.Year,
                AuthorId=bookEntity.AuthorId,
                BooksAmount=bookEntity.BooksAmount,
                //CreatedDate = bookEntity.CreatedDate
            };
            return book;
        }

        public static AuthorEntity MapToAuthorEntity(Author author) 
        {
            var authorEntity = new AuthorEntity()
            {
                Id = author.Id,
                Name = author.Name,
                BirthDate = author.BirthDate,
                Genre = author.Genre,
                CreatedDate = DefaultDateProvider.Now()
            };
            return authorEntity;
        }
        public static Author MapToAuthor(AuthorEntity authorEntity)
        {
            var author = new Author()
            {
                Id = authorEntity.Id,
                Name = authorEntity.Name,
                BirthDate=authorEntity.BirthDate,
                Genre=authorEntity.Genre
                //CreatedDate=authorEntity.CreatedDate
            };
            return author;
        }
    }
}
