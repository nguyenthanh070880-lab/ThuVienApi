using WebAPI_simple.Data;

public class LocalImageRepository : IImageRepository
{
    private readonly IWebHostEnvironment _webHostEnvironment;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly AppDbContext _dbContext;

    public LocalImageRepository(
        IWebHostEnvironment webHostEnvironment,
        IHttpContextAccessor httpContextAccessor,
        AppDbContext dbContext)
    {
        _webHostEnvironment = webHostEnvironment;
        _httpContextAccessor = httpContextAccessor;
        _dbContext = dbContext;
    }

    // Service Upload
    public Image Upload(Image image)
    {
        var localFilePath = Path.Combine(
            _webHostEnvironment.ContentRootPath,
            "Images",
            $"{image.FileName}{image.FileExtension}");

        // Upload image to local Path
        using var stream = new FileStream(localFilePath, FileMode.Create);
        image.File.CopyTo(stream);

        // Tạo URL công khai
        var urlFilePath = $"{_httpContextAccessor.HttpContext.Request.Scheme}://" +
                          $"{_httpContextAccessor.HttpContext.Request.Host}" +
                          $"{_httpContextAccessor.HttpContext.Request.PathBase}/Images/" +
                          $"{image.FileName}{image.FileExtension}";

        image.FilePath = urlFilePath;

        // Add Image to the Images table
        _dbContext.Images.Add(image);
        _dbContext.SaveChanges();

        return image;
    }

    // Service Get info
    public List<Image> GetAllInfoImages()
    {
        var allImages = _dbContext.Images.ToList();
        return allImages;
    }
    public (byte[], string, string) DownloadFile(int Id)
    {
        try
        {
            var FileById = _dbContext.Images.Where(x => x.Id == Id).FirstOrDefault();

            var path = Path.Combine(
                _webHostEnvironment.ContentRootPath,
                "Images",
                $"{FileById.FileName}{FileById.FileExtension}");

            var stream = File.ReadAllBytes(path);
            var fileName = FileById.FileName + FileById.FileExtension;

            return (stream, "application/octet-stream", fileName);
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
}