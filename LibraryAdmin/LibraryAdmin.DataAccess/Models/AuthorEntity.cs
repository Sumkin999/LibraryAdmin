using System.ComponentModel.DataAnnotations;

namespace LibraryAdmin.DataAccess.Models
{
    public class AuthorEntity
    {
        public long? Id { get; set; }
        public string? Name { get; set; }
        public DateOnly? BirthDate { get; set; }
        public string? Genre { get; set; }
        public DateTimeOffset CreatedDate { get; set; }
    }
}
