using LibraryAdmin.DataAccess;
using LibraryAdmin.DataAccess.Models;
using LibraryAdmin.Utils;
using Microsoft.EntityFrameworkCore;

namespace LibraryAdmin.Repositories
{
    public class AuthorRepository
    {
        public readonly LibraryAdminDbContext _context;
        public readonly ILogger<AuthorRepository> _logger;

        public AuthorRepository(LibraryAdminDbContext context, ILogger<AuthorRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        // Создание автора
        public async Task<AuthorEntity?> AddAuthor(string name, DateOnly birtDate, string genre)
        {
            if (birtDate < Utils.Utils.BirthDateMin) return null;
            var newAuthor = new AuthorEntity()
            {
                BirthDate = birtDate,
                Name = name,
                Genre = genre,
                CreatedDate = DateTime.Now // У тебя в задачке с собеса было что дату лучше брать из абстракции. Насколько здесь это нужно?
            };
            try
            {
                await _context.Authrors.AddAsync(newAuthor);
                await _context.SaveChangesAsync();

                _context.ChangeTracker.Clear();
                return newAuthor;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return null;
            }
        }
        /*
         * Редактирование автора. Мы можем изменить имя, дату рождения, жанр
         * Тк имя + жанр = alternate key, то при изменении чего из этого нужно пересоздать сущность автора
         */
        public async Task<AuthorEntity?> EditAuthor(string name, DateOnly? birtDate = null, string newName = "", DateOnly? newBirtDate = null, string genre = "")
        {
            try
            {
                AuthorEntity? author = await _context.Authrors.Include(x=>x.Books).TryFindAuthor(name, birtDate);
                if (author == null) { _logger.LogWarning($"Can't find author {name}"); return null; }

                bool isNewName = !string.IsNullOrEmpty(newName) && author.Name != newName;
                bool isNewBirthdate = newBirtDate != null && author.BirthDate != newBirtDate;
                bool isNewGenre = !string.IsNullOrEmpty(genre) && author.Genre != genre;

                if (isNewName || isNewBirthdate)
                {
                    /*
                     * Тк удаление + создание, оформил в транзакцию
                     * Так же вроде нет необходимости обновлять AuthorEntity в книге
                     */
                    using (var transaction = _context.Database.BeginTransaction()) 
                    {
                        try
                        {
                            var newAuthor = new AuthorEntity()
                            {
                                Id = author.Id,
                                Name = isNewName ? newName : author.Name,
                                BirthDate = (DateOnly)(isNewBirthdate ? newBirtDate : author.BirthDate),
                                CreatedDate = author.CreatedDate,
                                Genre = isNewGenre ? genre : author.Genre,
                                Books = author.Books
                            };
                            _context.Authrors.Remove(author);
                            await _context.SaveChangesAsync();

                            await _context.Authrors.AddAsync(newAuthor);
                            await _context.SaveChangesAsync();

                            transaction.Commit();
                        }
                        catch (Exception ex)
                        {
                            _logger.LogError(ex.Message);
                            transaction.Rollback();
                            return null;
                        }
                    }
                }
                else if(isNewGenre)
                {
                    author.Genre = genre;
                    await _context.SaveChangesAsync();
                }
                
                _context.ChangeTracker.Clear();

                return author;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return null;
            }
        }

        public async Task<AuthorEntity?> GetAuthorByGuid(Guid guid, bool isTrack=false)
        {
            return await _context.Authrors.IsTrackAuthor(isTrack).FirstOrDefaultAsync(x=>x.Id == guid);
        }

        public async Task<List<AuthorEntity>> GetAllAuthors()
        {
            return await _context.Authrors.AsNoTracking().ToListAsync();
        }
    }
}
