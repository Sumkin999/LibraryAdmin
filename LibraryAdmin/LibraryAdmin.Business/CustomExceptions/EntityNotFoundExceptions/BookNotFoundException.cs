namespace LibraryAdmin.API.ExceptionFilters
{
    public class BookNotFoundException : EntityNotFoundException
    {
        protected override string GetErrorHeader()
        {
            return "Book with Id {0} not found";
        }
        public BookNotFoundException(long? id) : base(id)
        {
        }
    }
}
