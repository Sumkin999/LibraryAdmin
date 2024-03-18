using LibraryAdmin.DataAccess.Models;
using Microsoft.EntityFrameworkCore;

namespace LibraryAdmin.Utils
{
    public static class EntityQueryExtensions
    {
        public static IQueryable<AuthorEntity> IsTrackAuthor(this IQueryable<AuthorEntity> source, bool isTrack)
        {
            return isTrack ? source : source.AsNoTracking();
        }

        public static IQueryable<BookEntity> IsTrackBook(this IQueryable<BookEntity> source, bool isTrack)
        {
            return isTrack ? source : source.AsNoTracking();
        }
    }
}
