using LibraryAdmin.DataAccess;
using LibraryAdmin.DataAccess.Models;
using LibraryAdmin.Utils;
using Microsoft.EntityFrameworkCore;

namespace LibraryAdmin.Repositories
{
    public class BookRepository
    {
        public readonly LibraryAdminDbContext _context;
        public readonly ILogger<BookRepository> _logger;
        public readonly AuthorRepository _authorRepository;

        public BookRepository(LibraryAdminDbContext context, ILogger<BookRepository> logger, AuthorRepository authorRepository)
        {
            _context = context;
            _logger = logger;
            _authorRepository = authorRepository;
        }

        public async Task AddBook(Guid authorId, string bookName, int year, int amount)
        {
            if (string.IsNullOrEmpty(bookName) || amount < 0) return;

            try
            {
                var autrhor = await _authorRepository.GetAuthorByGuid(authorId, true);
                if (autrhor == null) return;

                var newBook = new BookEntity()
                {
                    AuthorEntity = autrhor,
                    AuthorId = autrhor.Id,
                    CreatedDate = DateTime.Now,// Аналогично в создании автора
                    Title = bookName,
                    BooksAmount = amount,
                    Year = year
                };
                autrhor.Books.Add(newBook);
                await _context.AddAsync(newBook);

                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }
        }

        /*
         * Замена книги, самый душный момент оказался
         */
        public async Task ChangeBook(Guid authorId, string bookName, int year, string newBookName = "", int newYear = -1)
        {
            if (authorId==Guid.Empty || (!string.IsNullOrEmpty(bookName) && year<0)) { return; }
            //Сделал Include на автора
            var book = await _context.Books.Include(x=>x.AuthorEntity).TryFindBook(authorId, bookName, year);
            if(book == null) return;

            bool isNewName = !string.IsNullOrEmpty(newBookName) && book.Title != newBookName;
            bool isNewYear = newYear >= 0 && book.Year != newYear;

            if (isNewName || isNewYear)
            {
                using (var transaction = _context.Database.BeginTransaction()) 
                {
                    try
                    {
                        /*
                         * Если не занулить автора, то при сохранении ексепшн на уникальность PK_Authors
                         */
                        var author = book.AuthorEntity;
                        book.AuthorEntity = null;

                        author.Books.Remove(book);
                        _context.Books.Remove(book);
                        await _context.SaveChangesAsync();

                        var newBook = new BookEntity()
                        {
                            Id = book.Id,
                            AuthorEntity = author,
                            AuthorId = author.Id,
                            CreatedDate = book.CreatedDate,
                            Title = isNewName ? newBookName : book.Title,
                            Year = isNewYear ? newYear : book.Year,
                            BooksAmount = book.BooksAmount
                        };

                        await _context.AddAsync(newBook);
                        author.Books.Add(newBook);

                        
                        await _context.SaveChangesAsync();

                        transaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex.Message);
                        transaction.Rollback();
                    }
                }
            }
        }

        public async Task<BookEntity?> GetBookInfoByGuid(Guid guid)
        {
            return await _context.Books.AsNoTracking().FirstOrDefaultAsync(x=>x.Id == guid);
        }
        public async Task<List<BookEntity>> GetAllBooks()
        {
            return await _context.Books.AsNoTracking().ToListAsync();
        }


        public async Task<BookEntity?> GiveBook(Guid guid)
        {
            var book = await _context.Books.FirstOrDefaultAsync(x => x.Id == guid);
            if (book == null || book.BooksAmount <= 0) { return null; }

            /*
             * Для получения и выдачи книги обернул уменьшение/увеличение кол-ва в транзакцию на случай паралельного доступа
             * опасный момент когда кол-во около 0
             * ReaderWriterLock (вроде) не подходит т.к. нет необходимости паралельнго чтения/периодической записи
             * Interlocked не подходит т.к. не значимый тип
             */

            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    book.BooksAmount = book.BooksAmount - 1;
                    _context.Books.Update(book);
                    _context.SaveChanges();

                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex.Message);
                    transaction.Rollback();
                    return null;
                }
            }
            return book;
        }
        public async Task<BookEntity?> GetBook(Guid guid)
        {
            var book = await _context.Books.FirstOrDefaultAsync(x => x.Id == guid);
            if (book == null) { return null; }
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    book.BooksAmount = book.BooksAmount + 1;
                    _context.Books.Update(book);
                    _context.SaveChanges();

                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex.Message);
                    transaction.Rollback();
                    return null;
                }
            }
            return book;
        }
    }
}
