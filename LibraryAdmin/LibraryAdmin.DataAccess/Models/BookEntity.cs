using System.ComponentModel.DataAnnotations;

namespace LibraryAdmin.DataAccess.Models
{
    public class BookEntity
    {
        public long? Id { get; set; }
        public int? Year { get; set; }
        public string? Title { get; set; }
        public long? AuthorId { get; set; }
        [ConcurrencyCheck]
        public int? BooksAmount { get; set; }
        public DateTimeOffset CreatedDate { get; set; }
    }
}
