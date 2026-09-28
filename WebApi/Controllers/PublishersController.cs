using Microsoft.AspNetCore.Mvc;
using WebApi.CustomActionFilters;
using WebApi.Models.DTO;
using WebApi.Repositories;
using WebAPI_simple.Data;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PublishersController : ControllerBase
    {
        private readonly AppDbContext _dbContext;
        private readonly IPublisherRepository _publisherRepository;

        public PublishersController(AppDbContext dbContext, IPublisherRepository publisherRepository)
        {
            _dbContext = dbContext;
            _publisherRepository = publisherRepository;
        }

        [HttpGet("get-all-publisher")]
        public IActionResult GetAllPublisher()
        {
            var allPublishers = _publisherRepository.GetAllPublishers();
            return Ok(allPublishers);
        }

        [HttpGet("get-publisher-by-id/{id}")]
        public IActionResult GetPublisherById(int id)
        {
            var publisherWithId = _publisherRepository.GetPublisherById(id);
            return Ok(publisherWithId);
        }

        [HttpPost("add-publisher")]
        [ValidateModel]
        public IActionResult AddPublisher([FromBody] AddPublisherRequestDTO addPublisherRequestDTO)
        {
            var isDuplicate = _dbContext.Publishers.Any(p => p.Name.ToLower() == addPublisherRequestDTO.Name.ToLower());

            if (isDuplicate)
            {
                ModelState.AddModelError("Name", "Tên nhà xuất bản đã trùng lặp!");
                return BadRequest(ModelState);
            }

            var publisherAdd = _publisherRepository.AddPublisher(addPublisherRequestDTO);
            return Ok(publisherAdd);
        }
        [HttpPut("update-publisher-by-id/{id}")]
        public IActionResult UpdatePublisherById(int id, [FromBody] PublisherNoIdDTO publisherDTO)
        {
            var publisherUpdate = _publisherRepository.UpdatePublisherById(id, publisherDTO);
            return Ok(publisherUpdate);
        }

        [HttpDelete("delete-publisher-by-id/{id}")]
        public IActionResult DeletePublisher(int id)
        {
            var publisher = _dbContext.Publishers.FirstOrDefault(p => p.Id == id);
            if (publisher == null)
            {
                return NotFound($"Không tìm thấy Nhà xuất bản với ID = {id}");
            }
            var hasBooks = _dbContext.Books.Any(b => b.PublisherID == id);
            if (hasBooks)
            {
                return BadRequest($"Không thể xóa Nhà xuất bản này vì vẫn còn các cuốn sách đang tham chiếu tới!");
            }
            _dbContext.Publishers.Remove(publisher);
            _dbContext.SaveChanges();
            return Ok($"Đã xóa Nhà xuất bản ID = {id} thành công.");
        }
    }
}