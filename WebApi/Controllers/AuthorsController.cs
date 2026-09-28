using Microsoft.AspNetCore.Mvc;
using WebApi.CustomActionFilters;
using WebApi.Models.DTO;
using WebApi.Repositories;
using WebAPI_simple.Data;
using WebAPI_simple.Models.Domain;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthorsController : ControllerBase
    {
        private readonly AppDbContext _dbContext;
        private readonly IAuthorRepository _authorRepository;
        public AuthorsController(AppDbContext dbContext, IAuthorRepository
       authorRepository)
        {
            _dbContext = dbContext;
            _authorRepository = authorRepository;
        }
        [HttpGet("get-all-author")]
        public IActionResult GetAllAuthor()
        {
            var allAuthors = _authorRepository.GellAllAuthors();
            return Ok(allAuthors);
        }
        [HttpGet("get-author-by-id/{id}")]
        public IActionResult GetAuthorById(int id)
        {
            var authorWithId = _authorRepository.GetAuthorById(id);
            return Ok(authorWithId);
        }
        [HttpPost("add-author")]
        [ValidateModel]
        public IActionResult AddAuthors([FromBody] AddAuthorRequestDTO addAuthorRequestDTO)
        {
            var authorAdd = _authorRepository.AddAuthor(addAuthorRequestDTO);
            return Ok(authorAdd);
        }
        [HttpPut("update-author-by-id/{id}")]
        public IActionResult UpdateBookById(int id, [FromBody] AuthorNoIdDTO authorDTO)
        {
            var authorUpdate = _authorRepository.UpdateAuthorById(id, authorDTO);
            return Ok(authorUpdate);
        }
        [HttpDelete("delete-author-by-id/{id}")]
        public IActionResult DeleteAuthor(int id)
        {
            var author = _dbContext.Authors.FirstOrDefault(a => a.Id == id);
            if (author == null)
            {
                return NotFound($"Không tìm thấy Tác giả với ID = {id}");
            }
            var hasBooks = _dbContext.Books_Authors.Any(ba => ba.AuthorId == id);
            if (hasBooks)
            {
                return BadRequest("Hãy gỡ liên kết trong Book_Author trước khi xóa.");
            }
            _dbContext.Authors.Remove(author);
            _dbContext.SaveChanges();
            return Ok($"Đã xóa Tác giả ID = {id} thành công.");
        }
    }
}