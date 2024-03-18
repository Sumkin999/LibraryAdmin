namespace LibraryAdmin.API.ExceptionFilters
{
    public class EntityNotFoundException : Exception
    {
        public readonly string errorStack = "Entity not found";
        protected virtual string GetErrorHeader()
        {
            return "Entity with Id {0} not found";
        }
        public EntityNotFoundException(long? id)
        {
            try
            {
                errorStack = string.Format(GetErrorHeader(), id);
            }
            catch (Exception)
            {
            }
        }
    }
}
