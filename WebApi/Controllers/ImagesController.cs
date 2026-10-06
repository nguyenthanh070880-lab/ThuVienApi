using Microsoft.AspNetCore.Mvc;
using WebApi.Models; 
using WebApi.Models.DTO;   
using WebApi.Repositories;  

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImagesController : ControllerBase
    {
        private readonly IImageRepository _imageRepository;

        public ImagesController(IImageRepository imageRepository)
        {
            _imageRepository = imageRepository;
        }
        [HttpPost]
        [Route("Upload")]
        public IActionResult Upload([FromForm] ImageUploadRequestDTO request)
        {
            ValidateFileUpload(request);

            if (ModelState.IsValid)
            {
                var imageDomainModel = new Image
                {
                    File = request.File,
                    FileExtension = Path.GetExtension(request.File.FileName),
                    FileSizeInBytes = request.File.Length,
                    FileName = request.FileName,
                    FileDescription = request.FileDescription,
                };
                _imageRepository.Upload(imageDomainModel);

                return Ok(imageDomainModel);
            }

            return BadRequest(ModelState);
        }
        [HttpGet]
        public IActionResult GetAllInfoImages()
        {
            var allImages = _imageRepository.GetAllInfoImages();
            return Ok(allImages);
        }
        [HttpGet]
        [Route("Download")]
        public IActionResult DownloadImage(int id)
        {
            try
            {
                var result = _imageRepository.DownloadFile(id);
                return File(result.Item1, result.Item2, result.Item3);
            }
            catch
            {
                return NotFound("File not found");
            }
        }
        private void ValidateFileUpload(ImageUploadRequestDTO request)
        {
            var allowExtensions = new string[] { ".jpg", ".jpeg", ".png" };

            if (!allowExtensions.Contains(Path.GetExtension(request.File.FileName).ToLower()))
            {
                ModelState.AddModelError("file", "Unsupported file extension");
            }

            if (request.File.Length > 10400000) // ~10MB
            {
                ModelState.AddModelError("file", "File size too big, please upload file <10M");
            }
        }
    }
}