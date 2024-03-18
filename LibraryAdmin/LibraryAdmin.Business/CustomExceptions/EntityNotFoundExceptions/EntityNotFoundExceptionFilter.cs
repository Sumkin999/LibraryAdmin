using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace LibraryAdmin.API.ExceptionFilters
{
    public class EntityNotFoundExceptionFilter : ExceptionFilterAttribute
    {
        private readonly ILogger _logger;
        public EntityNotFoundExceptionFilter(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<EntityNotFoundExceptionFilter>();
        }

        public override void OnException(ExceptionContext context)
        {
            if (context.Exception is EntityNotFoundException entityNotFoundException)
            {
                context.ExceptionHandled = true;
                context.Result = new ContentResult()
                {
                    Content = entityNotFoundException.errorStack,
                    StatusCode = StatusCodes.Status409Conflict
                };

                _logger.LogWarning(entityNotFoundException.errorStack);
            }
        }
    }
}
