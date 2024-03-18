namespace LibraryAdmin.API.DtoModels
{
    public class BookRequestModel
    {
        public long? Id { get; set; }
        public string? Title { get; set; }
        public int? Year { get; set; }
        public long? AuthorId { get; set; }
        public int? Amount { get; set; }
    }
}
