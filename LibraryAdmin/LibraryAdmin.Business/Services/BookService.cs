using LibraryAdmin.API.DtoModels;
using LibraryAdmin.API.ExceptionFilters;
using LibraryAdmin.Business.CustomExceptions;
using LibraryAdmin.Business.Mappers;
using LibraryAdmin.Business.Models;
using LibraryAdmin.DataAccess.Repositories.Contracts;
using LibraryAdmin.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace LibraryAdmin.Business.Services
{
    public class BookService
    {
        IBookRepository _bookRepository;
        IAuthorRepository _authorRepository;

        public BookService(IBookRepository bookRepository, IAuthorRepository authorRepository)
        {
            _bookRepository = bookRepository;
            _authorRepository = authorRepository;
        }

        public async Task<long?> CreateBook(CancellationToken cancellationToken, BookRequestModel bookDto)
        {
            try
            {
                var book = MapperRequestModels.MapToBook(bookDto);
                if (!Validator.IsValidBook(book, out var validationErrors))
                {
                    throw new InvalidBookException(validationErrors);
                }
                cancellationToken.ThrowIfCancellationRequested();

                var authorEntity = await _authorRepository.GetEntityById(cancellationToken, book.AuthorId, false);
                if (authorEntity == null)
                {
                    throw new AuthorNotFoundException(bookDto.AuthorId);
                }

                cancellationToken.ThrowIfCancellationRequested();

                var id = await _bookRepository.CreateBookEntity(cancellationToken, MapperDataAccess.MapToBookEntity(book));

                cancellationToken.ThrowIfCancellationRequested();

                return id;
            }
            catch (OperationCanceledException operationCancelled)
            {
                throw operationCancelled;
            }
            catch (InvalidBookException bookException)
            {
                throw bookException;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public async Task UpdateBook(CancellationToken cancellationToken, BookRequestModel bookDto)
        {
            try
            {
                cancellationToken.ThrowIfCancellationRequested();
                var bookEntity = await _bookRepository.GetEntityById(cancellationToken, bookDto.Id, true);
                if (bookEntity == null) 
                { 
                    throw new BookNotFoundException(bookDto.Id);
                }
                bool isUpdate = false;
                if (!string.IsNullOrEmpty(bookDto.Title) && bookDto.Title != bookEntity.Title)
                {
                    bookEntity.Title = bookDto.Title;
                    isUpdate = true;
                }
                if (bookDto.Year != null && bookDto.Year != bookEntity.Year)
                {
                    bookEntity.Year = bookDto.Year;
                    isUpdate = true;
                }
                if (isUpdate)
                {
                    //Добавить валидацию на Entity?
                    var book = MapperDataAccess.MapToBook(bookEntity);
                    if (!Validator.IsValidBook(book, out var validationErrors))
                    {
                        throw new InvalidBookException(validationErrors);
                    }

                    cancellationToken.ThrowIfCancellationRequested();
                    await _bookRepository.UpdateBookEntity(cancellationToken, bookEntity);

                    cancellationToken.ThrowIfCancellationRequested();
                }
            }
            catch (OperationCanceledException operationCancelled)
            {
                throw operationCancelled;
            }
            catch (EntityNotFoundException entityNotFoundException)
            {
                throw entityNotFoundException;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task ChangeBookAmount(CancellationToken cancellationToken, long id, int delta)
        {
            try
            {
                if (delta == 0) return;

                cancellationToken.ThrowIfCancellationRequested();

                var bookEntity = await _bookRepository.GetEntityById(cancellationToken, id, true);
                if (bookEntity == null)
                {
                    throw new BookNotFoundException(id);
                }
                if (delta<0 && bookEntity.BooksAmount<Math.Abs(delta))
                {
                    throw new NegativeBooksAmountException(id, bookEntity.BooksAmount, delta);
                }

                bookEntity.BooksAmount += delta;

                cancellationToken.ThrowIfCancellationRequested();
                await _bookRepository.ChangeBookAmount(cancellationToken, bookEntity, 10);

                cancellationToken.ThrowIfCancellationRequested();
            }
            catch (OperationCanceledException operationCancelled)
            {
                throw operationCancelled;
            }
            catch (NullReferenceException)
            {
                throw new BookNotFoundException(id);
            }
            catch (BookNotFoundException bookNotFoundException)
            {
                throw bookNotFoundException;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<Book> GetById(CancellationToken cancellationToken, long? id)
        {
            try
            {
                cancellationToken.ThrowIfCancellationRequested();

                var bookEntity = await _bookRepository.GetEntityById(cancellationToken, id, false);
                cancellationToken.ThrowIfCancellationRequested();

                if (bookEntity == null)
                {
                    throw new BookNotFoundException(id);
                }

                var book = MapperDataAccess.MapToBook(bookEntity);
                return book;
            }
            catch (OperationCanceledException operationCancelled)
            {
                throw operationCancelled;
            }
            catch (BookNotFoundException bookNotFoundException)
            {
                throw bookNotFoundException;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<Book>> GetAll(CancellationToken cancellationToken)
        {
            try
            {
                cancellationToken.ThrowIfCancellationRequested();

                var bookEntities = await _bookRepository.GetAll(cancellationToken);
                cancellationToken.ThrowIfCancellationRequested();

                var books = new List<Book>();

                foreach (var bookEntity in bookEntities)
                {
                    cancellationToken.ThrowIfCancellationRequested();

                    var author = MapperDataAccess.MapToBook(bookEntity);
                    books.Add(author);
                }

                return books;
            }
            catch (OperationCanceledException operationCancelled)
            {
                throw operationCancelled;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
