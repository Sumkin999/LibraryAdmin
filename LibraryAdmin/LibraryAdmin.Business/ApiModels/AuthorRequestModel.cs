namespace LibraryAdmin.API.DtoModels
{
    public class AuthorRequestModel
    {
        public long? Id { get; set; }
        public string? Name { get; set; }
        public DateOnly? BirthDate { get; set; }
        public string? Genre { get; set; }
    }
}
