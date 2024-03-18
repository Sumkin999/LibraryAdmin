using System.Text;

namespace LibraryAdmin.API.ExceptionFilters
{
    public class InvalidEntityException : Exception
    {
        public readonly string errorStack = "";
        protected virtual string GetErrorHeader()
        {
            return "Validation exception:\n";
        }
        public InvalidEntityException(List<FluentValidation.Results.ValidationFailure>? validateErrors)
        {
            var sb = new StringBuilder();
            sb.AppendLine(GetErrorHeader());

            if (validateErrors == null)
            {
                sb.AppendLine("Error: validation errors is null!");
            }
            else
            {
                foreach (var error in validateErrors)
                {
                    sb.AppendLine(error.ErrorMessage);
                }
            }

            errorStack = sb.ToString();
        }

        public InvalidEntityException(string message)
            : base(message)
        {
        }

        public InvalidEntityException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
    
}
