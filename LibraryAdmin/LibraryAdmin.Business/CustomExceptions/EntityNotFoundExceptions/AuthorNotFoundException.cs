namespace LibraryAdmin.API.ExceptionFilters
{
    public class AuthorNotFoundException : EntityNotFoundException
    {
        protected override string GetErrorHeader()
        {
            return "Author with Id {0} not found";
        }
        public AuthorNotFoundException(long? id) : base(id)
        {
        }
    }
}
