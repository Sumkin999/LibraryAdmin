using System.ComponentModel.DataAnnotations;

namespace LibraryAdmin.DataAccess.Models
{
    public class BookEntity
    {
        public Guid Id { get; set; }
        public int Year { get; set; }
        public required string Title { get; set; }

        // В требованиях не было, проявил наказуемую инициативу
        public required AuthorEntity AuthorEntity { get; set; }
        public Guid AuthorId { get; set; }

        //Так же аннтотация не срабатывает, в базу пишется всякое
        [Range(0, int.MaxValue)]
        public int BooksAmount { get; set; }
        public required DateTime CreatedDate { get; set; }
    }
}
