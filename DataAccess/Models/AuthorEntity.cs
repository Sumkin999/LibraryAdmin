using System.ComponentModel.DataAnnotations;

namespace LibraryAdmin.DataAccess.Models
{
    public class AuthorEntity
    {
        public Guid Id { get; set; }
        public required string Name { get; set; }


        /* Аннотация на рейндж не срабатывает, так же не осилил как правильно в конфигурации сущности .ToTable(t => t.HasCheckConstraint .
         Интересно так жеузнать какое обыно правило на подобное, в БД или можно в коде (в данном случае)
        */
        [Range(typeof(DateOnly), "1900-01-01", "9999-12-31")]
        public required DateOnly BirthDate { get; set; }
        public required string Genre { get; set; }
        public required DateTime CreatedDate { get; set; }
        public List<BookEntity> Books { get; set; } = new List<BookEntity> { };
    }
}
