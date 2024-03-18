using FluentValidation.Results;
using System.Text;

namespace LibraryAdmin.API.ExceptionFilters
{
    public class InvalidAuthorException : InvalidEntityException
    {
        protected override string GetErrorHeader()
        {
            return "Author validation exception:\n";
        }
        public InvalidAuthorException(List<ValidationFailure>? validateErrors) : base(validateErrors)
        {
        }
    }

}
