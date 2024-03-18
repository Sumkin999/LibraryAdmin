using LibraryAdmin.DataAccess;
using LibraryAdmin.DataAccess.Models;
using LibraryAdmin.DataAccess.Repositories.Contracts;
using LibraryAdmin.Utils;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace LibraryAdmin.Repositories
{
    public class BookRepository : IBookRepository
    {
        public readonly LibraryAdminDbContext _context;
        public readonly ILogger<BookRepository> _logger;
        public readonly IAuthorRepository _authorRepository;

        public BookRepository(LibraryAdminDbContext context, ILogger<BookRepository> logger, IAuthorRepository authorRepository)
        {
            _context = context;
            _logger = logger;
            _authorRepository = authorRepository;
        }

        public async Task<long?> CreateBookEntity(CancellationToken cancellationToken, BookEntity bookEntity)
        {
            try
            {
                cancellationToken.ThrowIfCancellationRequested();

                await _context.Books.AddAsync(bookEntity);
                await _context.SaveChangesAsync();

                cancellationToken.ThrowIfCancellationRequested();

                return bookEntity.Id;
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

        public async Task UpdateBookEntity(CancellationToken cancellationToken, BookEntity bookEntity)
        {
            try
            {
                cancellationToken.ThrowIfCancellationRequested();

                _context.Update(bookEntity);
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

        public async Task<BookEntity?> GetEntityById(CancellationToken cancellationToken, long? id, bool isTrack)
        {
            try
            {
                cancellationToken.ThrowIfCancellationRequested();

                return await _context.Books.IsTrackBook(isTrack).FirstOrDefaultAsync(x => x.Id == id);
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

        public async Task ChangeBookAmount(CancellationToken cancellationToken, BookEntity bookEntity, int attempts)
        {
            try
            {
                cancellationToken.ThrowIfCancellationRequested();

                if (attempts <= 0)
                {
                    throw new Exception("Too many requests");
                }

                _context.Update(bookEntity);

                cancellationToken.ThrowIfCancellationRequested();
                await _context.SaveChangesAsync();

                cancellationToken.ThrowIfCancellationRequested();
            }
            catch (DbUpdateConcurrencyException)
            {
                await ChangeBookAmount(cancellationToken, bookEntity, attempts - 1);
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

        public async Task<List<BookEntity>> GetAll(CancellationToken cancellationToken)
        {
            try
            {
                cancellationToken.ThrowIfCancellationRequested();

                return await _context.Books.AsNoTracking().ToListAsync();
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
