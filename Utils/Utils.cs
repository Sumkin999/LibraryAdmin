using LibraryAdmin.DataAccess.Models;
using Microsoft.EntityFrameworkCore;

namespace LibraryAdmin.Utils
{
    public static class Utils
    {
        public static readonly DateOnly BirthDateMin = new DateOnly(1900, 1, 1);
        public static async Task<AuthorEntity?> TryFindAuthor(this IQueryable<AuthorEntity> source, string name, DateOnly? birthDate)
        {
            return birthDate==null ? await source.FirstOrDefaultAsync(x=>x.Name == name) : await source.FirstOrDefaultAsync(x => x.Name == name && x.BirthDate == birthDate);
        }
        public static IQueryable<AuthorEntity> IsTrackAuthor(this IQueryable<AuthorEntity> source, bool isTrack)
        {
            return isTrack ? source : source.AsNoTracking();
        }

        public static async Task<BookEntity?> TryFindBook(this IQueryable<BookEntity> source, Guid authorId, string bookName = "", int year = -1)
        {
            var books = await source.Where(x => x.AuthorId == authorId).ToListAsync();
            return year < 0 ? books.FirstOrDefault(x => x.Title == bookName) : books.FirstOrDefault(x => x.Title == bookName && x.Year == year);
        }
    }
}
