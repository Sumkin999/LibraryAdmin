using LibraryAdmin.DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryAdmin.DataAccess.Repositories.Contracts
{
    /*
     * Ты написал зарегестрировать репозитории через интерфейс. 
     * Возможно, ты имел ввиду что у них имеются почти повторяющиеся методы создания, изменения, получения и получения всех
     * Тогда надо было создать итерфейс или базовый класс для Entity, и переделать контракт на  
     * Task<long?> CreateEntity(CancellationToken cancellationToken, IEntity entity);
     * и т.д.
     * В таком надо для регистрации зарегать как
     * 
     * builder.Services.AddTransient<IRepository, AuthorRepository>();
       builder.Services.AddTransient<IRepository, BookRepository>();
     * 
     * для иньекции надо получать в конструкторе IEnumerable<IRepository> repoServices
     * и далее
     * 
     * AuthorRepository _authorRepository
     * foreach(var irepo in repoServices)
     *  if(irepo is AuthorRepository authorRepository)
     *      _authorRepository = authorRepository
     * 
     * Правильно?
     * 
     * Так же в репозитории книг есть метод на изменения количества. Не понимаю зачем нужен контракт, если всё равно через
     * тип рабоать надо будет
     * 
     * Для удобства сравнения оставил интерфейсы в одном файле
     */
    public interface IAuthorRepository
    {
        Task<long?> CreateAuthorEntity(CancellationToken cancellationToken, AuthorEntity author);
        Task UpdateAuthorEntity(CancellationToken cancellationToken, AuthorEntity author);
        Task<AuthorEntity?> GetEntityById(CancellationToken cancellationToken, long? id, bool isTrack);
        Task<List<AuthorEntity>> GetAll(CancellationToken cancellationToken);
    }
    public interface IBookRepository
    {
        Task<long?> CreateBookEntity(CancellationToken cancellationToken, BookEntity bookEntity);
        Task UpdateBookEntity(CancellationToken cancellationToken, BookEntity bookEntity);
        Task<BookEntity?> GetEntityById(CancellationToken cancellationToken, long? id, bool isTrack);
        Task ChangeBookAmount(CancellationToken cancellationToken, BookEntity bookEntity, int attempts);
        Task<List<BookEntity>> GetAll(CancellationToken cancellationToken);
    }
}
