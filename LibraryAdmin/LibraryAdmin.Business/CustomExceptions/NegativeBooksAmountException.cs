using LibraryAdmin.API.ExceptionFilters;
using LibraryAdmin.DataAccess.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryAdmin.Business.CustomExceptions
{
    public class NegativeBooksAmountException : Exception
    {
        public readonly string errorStack = "Not enough books";
        public NegativeBooksAmountException(long id, int? hasAmount, int needAmount) 
        {
            try
            {
                errorStack = string.Format("Book {0} amount is {1}, can't give {2} books", id, hasAmount, needAmount);
            }
            catch (Exception)
            {
            }
        }
    }

    public class NegativeBooksAmountExceptionFilter : ExceptionFilterAttribute
    {
        private readonly ILogger _logger;
        public NegativeBooksAmountExceptionFilter(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<NegativeBooksAmountExceptionFilter>();
        }

        public override void OnException(ExceptionContext context)
        {
            if (context.Exception is NegativeBooksAmountException negativeBookAmountException)
            {
                context.ExceptionHandled = true;
                context.Result = new ContentResult()
                {
                    Content = negativeBookAmountException.errorStack,
                    StatusCode = StatusCodes.Status423Locked
                };

                _logger.LogWarning(negativeBookAmountException.errorStack);
            }
        }
    }
}
