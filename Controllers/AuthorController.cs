using LibraryAdmin.DataAccess.Models;
using LibraryAdmin.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace LibraryAdmin.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuthorController : ControllerBase
    {
        private readonly ILogger<AuthorController> _logger;
        private readonly AuthorRepository _authorRepository;

        public AuthorController(ILogger<AuthorController> logger, AuthorRepository authorRepository)
        {
            _logger = logger;
            _authorRepository = authorRepository;
        }

        [HttpPost("AddAuthor")]
        public async Task<ActionResult> AddAuthor(string name, string dateBirthStr, string genre)
        {
            if (string.IsNullOrEmpty(name) || !DateOnly.TryParse(dateBirthStr, out DateOnly dateBirth)) return BadRequest();
            
            var author = await _authorRepository.AddAuthor(name, dateBirth, genre);
            return author != null ? Ok() : BadRequest();
        }
        [HttpPost("EditAuthor")]
        public async Task<ActionResult> EditAuthor(string name, string dateBirthStr="", string newname="", string newDateBirthStr="", string newGenre="")
        {
            DateOnly? dateBirth = null;
            if(DateOnly.TryParse(dateBirthStr, out DateOnly dateBirthTemp)) dateBirth = (DateOnly?)dateBirthTemp;

            DateOnly? newDateBirth = null;
            if(DateOnly.TryParse(newDateBirthStr, out DateOnly newDateBirthTemp)) newDateBirth = (DateOnly?)newDateBirthTemp;

            var author = await _authorRepository.EditAuthor(name, dateBirth, newname, newDateBirth, newGenre);
            return author != null ? Ok() : BadRequest();
        }

        [HttpGet("GetAuthor")]
        public async Task<AuthorEntity?> GetAuthor(string guid)
        {
            var author = await _authorRepository.GetAuthorByGuid(new Guid(guid));
            return author;
        }
        [HttpGet("GetAllAuthors")]
        public async Task<List<AuthorEntity>> GetAllAuthors()
        {
            var allAuthors = await _authorRepository.GetAllAuthors();
            return allAuthors;
        }
    }
}