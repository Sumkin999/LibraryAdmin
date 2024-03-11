using LibraryAdmin.DataAccess.Models;
using LibraryAdmin.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace LibraryAdmin.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class BookController : ControllerBase
    {
        private readonly ILogger<BookController> _logger;
        private readonly BookRepository _bookRepository;

        public BookController(ILogger<BookController> logger, BookRepository bookRepository)
        {
            _logger = logger;
            _bookRepository = bookRepository;
        }

        [HttpPost("AddBook")]
        public async Task AddBook(string authorId, string bookName, int year, int amount)
        {
            await _bookRepository.AddBook(new Guid(authorId), bookName, year, amount);
        }
        [HttpPost("ChangeBook")]
        public async Task ChangeBook(string authorId, string bookName, int year, string newBookName = "", int newYear = -1)
        {
            await _bookRepository.ChangeBook(new Guid(authorId), bookName, year, newBookName, newYear);
        }

        [HttpGet("GetBookInfo")]
        public async Task<BookEntity?> GetBookInfo(string guid)
        {
            var book = await _bookRepository.GetBookInfoByGuid(new Guid(guid));
            return book;
        }
        [HttpGet("GetAllBooks")]
        public async Task<List<BookEntity>> GetAllBooks()
        {
            var allBooks = await _bookRepository.GetAllBooks();
            return allBooks;
        }

        [HttpPost("GiveBook")]
        public async Task<ActionResult> GiveBook(string guid)
        {
            var book = await _bookRepository.GiveBook(new Guid(guid));
            return book!=null ? Ok() : BadRequest();
        }
        [HttpPost("GetBook")]
        public async Task<ActionResult> GetBook(string guid)
        {
            var book = await _bookRepository.GetBook(new Guid(guid));
            return book != null ? Ok() : BadRequest();
        }
    }
}