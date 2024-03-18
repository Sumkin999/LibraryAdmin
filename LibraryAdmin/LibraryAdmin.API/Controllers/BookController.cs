using LibraryAdmin.API.DtoModels;
using LibraryAdmin.API.ExceptionFilters;
using LibraryAdmin.Business.CustomExceptions;
using LibraryAdmin.Business.Models;
using LibraryAdmin.Business.Services;
using Microsoft.AspNetCore.Mvc;

namespace LibraryAdmin.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class BookController : ControllerBase
    {
        private readonly ILogger<BookController> _logger;
        private readonly BookService _bookService;

        private const string _exceptionMessage = "Exception while {0} book: {1}";

        public BookController(ILogger<BookController> logger, BookService bookService)
        {
            _logger = logger;
            _bookService = bookService;
        }

        private string TryFormatException(string method, Exception exception)
        {
            try
            {
                return string.Format(_exceptionMessage, method, exception.Message);
            }
            catch (Exception)
            {
                return exception.Message;
            }
        }

        [HttpPost("CreateBook")]
        public async Task CreateBook(CancellationToken cancellationToken, [FromBody] BookRequestModel book)
        {
            try
            {
                cancellationToken.ThrowIfCancellationRequested();

                await _bookService.CreateBook(cancellationToken, book);

                cancellationToken.ThrowIfCancellationRequested();
            }
            catch (InvalidBookException bookException)
            {
                throw bookException;
            }
            catch (EntityNotFoundException entityNotFoundException)
            {
                throw entityNotFoundException;
            }
            catch (OperationCanceledException operationCancelled)
            {
                throw operationCancelled;
            }
            catch (Exception ex)
            {
                _logger.LogError(TryFormatException("Creating", ex));
                throw;
            }
        }

        [HttpPatch("UpdateBook")]
        public async Task UpdateBook(CancellationToken cancellationToken, [FromBody] BookRequestModel book)
        {
            try
            {
                cancellationToken.ThrowIfCancellationRequested();

                await _bookService.UpdateBook(cancellationToken, book);

                cancellationToken.ThrowIfCancellationRequested();
            }
            catch (InvalidBookException bookException)
            {
                throw bookException;
            }
            catch (OperationCanceledException operationCancelled)
            {
                throw operationCancelled;
            }
            catch (EntityNotFoundException entityNotFoundException)
            {
                throw entityNotFoundException;
            }
            catch (Exception ex)
            {
                _logger.LogError(TryFormatException("Editing", ex));
                throw;
            }
        }


        [HttpPatch("GiveBook")]
        public async Task GiveBook(CancellationToken cancellationToken, long id)
        {
            try
            {
                cancellationToken.ThrowIfCancellationRequested();

                await _bookService.ChangeBookAmount(cancellationToken, id, -1);

                cancellationToken.ThrowIfCancellationRequested();
            }
            catch (InvalidBookException bookException)
            {
                throw bookException;
            }
            catch (OperationCanceledException operationCancelled)
            {
                throw operationCancelled;
            }
            catch (EntityNotFoundException entityNotFoundException)
            {
                throw entityNotFoundException;
            }
            catch (NegativeBooksAmountException negativeBooksAmount)
            {
                throw negativeBooksAmount;
            }
            catch (Exception ex)
            {
                _logger.LogError(TryFormatException("Give book", ex));
                throw;
            }
        }

        [HttpPatch("GetBook")]
        public async Task GetBook(CancellationToken cancellationToken, long id)
        {
            try
            {
                cancellationToken.ThrowIfCancellationRequested();

                await _bookService.ChangeBookAmount(cancellationToken, id, 1);

                cancellationToken.ThrowIfCancellationRequested();
            }
            catch (InvalidBookException bookException)
            {
                throw bookException;
            }
            catch (OperationCanceledException operationCancelled)
            {
                throw operationCancelled;
            }
            catch (EntityNotFoundException entityNotFoundException)
            {
                throw entityNotFoundException;
            }
            catch (Exception ex)
            {
                _logger.LogError(TryFormatException("Get book", ex));
                throw;
            }
        }

        [HttpGet("GetById")]
        public async Task<Book> GetById(CancellationToken cancellationToken, long id)
        {
            try
            {
                cancellationToken.ThrowIfCancellationRequested();

                return await _bookService.GetById(cancellationToken, id);
            }
            catch (OperationCanceledException operationCancelled)
            {
                throw operationCancelled;
            }
            catch (BookNotFoundException bookNotFoundException)
            {
                throw bookNotFoundException;
            }
            catch (Exception ex)
            {
                _logger.LogError(TryFormatException("Getting book by Id", ex));
                throw;
            }
        }

        [HttpGet("GetAll")]
        public async Task<List<Book>> GetAll(CancellationToken cancellationToken)
        {
            try
            {
                cancellationToken.ThrowIfCancellationRequested();

                return await _bookService.GetAll(cancellationToken);
            }
            catch (OperationCanceledException operationCancelled)
            {
                throw operationCancelled;
            }
            catch (Exception ex)
            {
                _logger.LogError(TryFormatException("Getting all books", ex));
                throw;
            }
        }
    }
}