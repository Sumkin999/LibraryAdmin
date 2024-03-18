using FluentValidation;
using LibraryAdmin.Business.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryAdmin.Business
{
    class AuthorValidator : AbstractValidator<Author>
    {
        public AuthorValidator()
        {
            string msg = "Error in field {PropertyName}: value {PropertyValue}";

            RuleFor(x=>x.Name).NotEmpty().WithMessage(msg).
                Must(x=>x!=null && x.Length<=128).WithMessage("Name must be <= 128 characters");

            RuleFor(x => x.BirthDate).NotEmpty().WithMessage(msg);
            RuleFor(x=>x.BirthDate).Must(IsBirthDateValid).WithMessage(x=>$"BirthDate must be >= {DefaultDateProvider.AuthorBirthDateMin}. Current BirthDate: {x.BirthDate}");

            RuleFor(x => x.Genre).NotEmpty().WithMessage(msg);
        }
        private bool IsBirthDateValid(DateOnly? date)
        {
            return date >= DefaultDateProvider.AuthorBirthDateMin;
        }
    }
    class BookValidator : AbstractValidator<Book>
    {
        public BookValidator() 
        {
            string msg = "Error in field {PropertyName}: value {PropertyValue}";

            RuleFor(x => x.Title).NotEmpty().WithMessage(msg);
            RuleFor(x=>x.Title).Must(x => x!=null && x.Length <= 256).WithMessage("Title must be <= 256 characters");

            RuleFor(x => x.Year).Must(x => x >= DefaultDateProvider.BookYearMin).WithMessage("Year must be >= 1900");

            RuleFor(x=>x.AuthorId).NotEmpty().WithMessage(msg);

            RuleFor(x => x.BooksAmount).Must(x => x >= 0).WithMessage("Book amount can't be negative");
        }
    }

    public static class Validator
    {
        private static AuthorValidator _authorValidator = new AuthorValidator();
        private static BookValidator _bookValidator = new BookValidator();

        public static bool IsValidAuthor(Author author, out List<FluentValidation.Results.ValidationFailure>? errors) 
        {
            errors = null;
            var result = _authorValidator.Validate(author);
            if (result.IsValid) { return true; }

            errors = result.Errors;

            return false;
        }
        public static bool IsValidBook(Book book, out List<FluentValidation.Results.ValidationFailure>? errors)
        {
            errors = null;
            var result = _bookValidator.Validate(book);
            if (result.IsValid) { return true; }

            errors = result.Errors;

            return false;
        }
    }
}
