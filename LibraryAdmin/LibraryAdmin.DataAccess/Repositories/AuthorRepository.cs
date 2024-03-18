using LibraryAdmin.DataAccess;
using LibraryAdmin.DataAccess.Models;
using LibraryAdmin.DataAccess.Repositories.Contracts;
using LibraryAdmin.Utils;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace LibraryAdmin.Repositories
{
    public class AuthorRepository: IAuthorRepository
    {
        public readonly LibraryAdminDbContext _context;
        public readonly ILogger<AuthorRepository> _logger;

        public AuthorRepository(LibraryAdminDbContext context, ILogger<AuthorRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<long?> CreateAuthorEntity(CancellationToken cancellationToken, AuthorEntity author)
        {
            try
            {
                // Check cancellationToken before starting the database operation
                cancellationToken.ThrowIfCancellationRequested();

                await _context.Authrors.AddAsync(author);
                await _context.SaveChangesAsync();

                // Check cancellationToken after the asynchronous operation
                cancellationToken.ThrowIfCancellationRequested();

                return author.Id;
            }
            catch (OperationCanceledException operationCancelled)
            {
                // Handle cancellation here if needed
                // You can log, clean up, or perform any other necessary actions
                // Re-throw the exception to propagate the cancellation further
                throw operationCancelled;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task UpdateAuthorEntity(CancellationToken cancellationToken, AuthorEntity author)
        {
            try
            {
                cancellationToken.ThrowIfCancellationRequested();

                _context.Update(author);
                await _context.SaveChangesAsync();

                cancellationToken.ThrowIfCancellationRequested();
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

        public async Task<AuthorEntity?> GetEntityById(CancellationToken cancellationToken, long? id, bool isTrack)
        {
            try
            {
                cancellationToken.ThrowIfCancellationRequested();

                return await _context.Authrors.IsTrackAuthor(isTrack).FirstOrDefaultAsync(x => x.Id == id);
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

        public async Task<List<AuthorEntity>> GetAll(CancellationToken cancellationToken)
        {
            try
            {
                cancellationToken.ThrowIfCancellationRequested();

                return await _context.Authrors.AsNoTracking().ToListAsync();
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
