using LibraryAdmin.API.DtoModels;
using LibraryAdmin.Business.Models;
using LibraryAdmin.DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryAdmin.Business.Mappers
{
    public static class MapperRequestModels
    {
        public static Author MapToAuthor(AuthorRequestModel authorDto) 
        {
            var author = new Author()
            {
                Id = authorDto.Id,
                Name = authorDto.Name,
                BirthDate = authorDto.BirthDate,
                Genre = authorDto.Genre
            };
            return author;
        } 
        public static Book MapToBook(BookRequestModel bookDto) 
        {
            var book = new Book()
            {
                Id = bookDto.Id,
                Title = bookDto.Title,
                AuthorId = bookDto.AuthorId,
                Year = bookDto.Year,
                BooksAmount = bookDto.Amount
            };
            return book;
        }
    }
}
