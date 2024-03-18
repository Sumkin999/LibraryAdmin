using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using LibraryAdmin.Business.CustomExceptions;

namespace LibraryAdmin.API.ExceptionFilters
{
    public class InvalidEntityExceptionFilter : ExceptionFilterAttribute
    {
        private readonly ILogger _logger;
        public InvalidEntityExceptionFilter(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<InvalidEntityExceptionFilter>();
        }

        public override void OnException(ExceptionContext context)
        {
            if (context.Exception is InvalidEntityException inalidEntityException)
            {
                context.ExceptionHandled = true;
                context.Result = new ContentResult()
                {
                    Content = inalidEntityException.errorStack,
                    StatusCode = StatusCodes.Status422UnprocessableEntity
                };

                _logger.LogWarning(inalidEntityException.errorStack);
            }
        }
    }
    
}
