using LibraryAdmin.API.ExceptionFilters;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation.Results;

namespace LibraryAdmin.Business.CustomExceptions
{
    public class InvalidBookException : InvalidEntityException
    {
        protected override string GetErrorHeader()
        {
            return "Book validation exception:\n";
        }

        public InvalidBookException(List<ValidationFailure>? validateErrors) : base(validateErrors)
        {
        }
    }
}
