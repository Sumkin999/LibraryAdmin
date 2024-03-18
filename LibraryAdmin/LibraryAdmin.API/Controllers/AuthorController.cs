using LibraryAdmin.API.DtoModels;
using LibraryAdmin.API.ExceptionFilters;
using LibraryAdmin.Business;
using LibraryAdmin.Business.Models;
using LibraryAdmin.Business.Services;
using LibraryAdmin.Repositories;
using Microsoft.AspNetCore.Mvc;
using System.Threading;

namespace LibraryAdmin.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuthorController : ControllerBase
    {
        private readonly ILogger<AuthorController> _logger;
        private readonly AuthorService _authorService;

        private const string _exceptionMessage = "Exception while {0} author: {1}";

        public AuthorController(ILogger<AuthorController> logger, AuthorService authorRepository)
        {
            _logger = logger;
            _authorService = authorRepository;
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

        [HttpPost("CreateAuthor")]
        public async Task CreateAuthor(CancellationToken cancellationToken, [FromBody] AuthorRequestModel author)
        {
            // Check cancellationToken before performing any asynchronous operation
            try
            {
                cancellationToken.ThrowIfCancellationRequested();
                await _authorService.CreateAuthor(cancellationToken, author);

                cancellationToken.ThrowIfCancellationRequested();
            }
            catch (InvalidAuthorException authorException)
            {
                throw authorException;
            }
            catch (OperationCanceledException operationCancelled)
            {
                // Handle cancellation here if needed
                // You can log, clean up, or perform any other necessary actions
                // Re-throw the exception to propagate the cancellation further
                throw operationCancelled;
            }
            catch (Exception ex)
            {
                _logger.LogError(TryFormatException("Creating", ex));
                throw;
            }
        }

        [HttpPatch("UpdateAuthor")]
        public async Task UpdateAuthor(CancellationToken cancellationToken, [FromBody] AuthorRequestModel author)
        {
            try 
            {
                cancellationToken.ThrowIfCancellationRequested();
                
                await _authorService.UpdateAuthor(cancellationToken, author);

                cancellationToken.ThrowIfCancellationRequested();
            }
            catch (InvalidAuthorException authorException)
            {
                throw authorException;
            }
            catch (OperationCanceledException operationCancelled)
            {
                throw operationCancelled;
            }
            catch (AuthorNotFoundException authorNotFoundException)
            {
                throw authorNotFoundException;
            }
            catch (Exception ex)
            {
                _logger.LogError(TryFormatException("Editing", ex));
                throw;
            }
        }

        [HttpGet("GetById")]
        public async Task<Author> GetById(CancellationToken cancellationToken, long id)
        {
            try
            {
                cancellationToken.ThrowIfCancellationRequested();

                return await _authorService.GetById(cancellationToken, id);
            }
            catch (OperationCanceledException operationCancelled)
            {
                throw operationCancelled;
            }
            catch (AuthorNotFoundException authorNotFoundException)
            {
                throw authorNotFoundException;
            }
            catch (Exception ex)
            {
                _logger.LogError(TryFormatException("Getting author by Id", ex));
                throw;
            }
        }

        [HttpGet("GetAll")]
        public async Task<List<Author>> GetAll(CancellationToken cancellationToken)
        {
            try
            {
                cancellationToken.ThrowIfCancellationRequested();

                return await _authorService.GetAll(cancellationToken);
            }
            catch (OperationCanceledException operationCancelled)
            {
                throw operationCancelled;
            }
            catch (Exception ex)
            {
                _logger.LogError(TryFormatException("Getting all authors", ex));
                throw;
            }
        }

    }
}