using Library_web.Models.DTO;
using Microsoft.AspNetCore.Mvc;
using System.Text;
using System.Text.Json;

namespace Library_web.Controllers
{
    public class BooksController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly string _apiBaseUrl = "https://localhost:7298";

        // Tùy chọn đọc JSON không phân biệt chữ hoa/chữ thường (camelCase -> PascalCase)
        private readonly JsonSerializerOptions _jsonOptions = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };

        public BooksController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<IActionResult> Index(string filterOn = null, string filterQuery = null,
                                               string sortBy = null, bool isAscending = true,
                                               int pageNumber = 1, int pageSize = 100)
        {
            List<BookDTO> response = new();
            try
            {
                // Dùng ApiClient đã bỏ qua SSL trong Program.cs
                var client = _httpClientFactory.CreateClient("ApiClient");

                // Bổ sung pageNumber & pageSize để API không trả về mảng rỗng []
                var url = $"{_apiBaseUrl}/api/Books/get-all-books?filterOn={filterOn}&filterQuery={filterQuery}&sortBy={sortBy}&isAscending={isAscending}&pageNumber={pageNumber}&pageSize={pageSize}";

                var httpResponse = await client.GetAsync(url);
                httpResponse.EnsureSuccessStatusCode();

                var result = await httpResponse.Content.ReadFromJsonAsync<IEnumerable<BookDTO>>(_jsonOptions);
                if (result != null)
                {
                    response.AddRange(result);
                }
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
            }
            return View(response);
        }

        public async Task<IActionResult> listBook(int id)
        {
            BookDTO response = new();
            try
            {
                var client = _httpClientFactory.CreateClient("ApiClient");
                var httpResponse = await client.GetAsync($"{_apiBaseUrl}/api/Books/get-book-by-id/{id}");
                httpResponse.EnsureSuccessStatusCode();

                var result = await httpResponse.Content.ReadFromJsonAsync<BookDTO>(_jsonOptions);
                if (result != null)
                {
                    response = result;
                }
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
            }
            return View(response);
        }

        [HttpGet]
        public async Task<IActionResult> addBook()
        {
            try
            {
                var client = _httpClientFactory.CreateClient("ApiClient");

                // Lấy danh sách Author
                var responseAu = await client.GetAsync($"{_apiBaseUrl}/api/Authors/get-all-author");
                responseAu.EnsureSuccessStatusCode();
                ViewBag.ListAuthor = await responseAu.Content.ReadFromJsonAsync<List<authorDTO>>(_jsonOptions);

                // Lấy danh sách Publisher
                var responsePu = await client.GetAsync($"{_apiBaseUrl}/api/Publishers/get-all-publisher");
                responsePu.EnsureSuccessStatusCode();
                ViewBag.ListPublisher = await responsePu.Content.ReadFromJsonAsync<List<publisherDTO>>(_jsonOptions);
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
            }

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> addBook(addBookDTO addBookDTO)
        {
            try
            {
                var client = _httpClientFactory.CreateClient("ApiClient");
                var request = new HttpRequestMessage
                {
                    Method = HttpMethod.Post,
                    RequestUri = new Uri($"{_apiBaseUrl}/api/Books/add-book"),
                    Content = new StringContent(
                        JsonSerializer.Serialize(addBookDTO),
                        Encoding.UTF8,
                        "application/json")
                };

                var response = await client.SendAsync(request);
                response.EnsureSuccessStatusCode();

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                return View(addBookDTO);
            }
        }

        [HttpGet]
        public async Task<IActionResult> editBook(int id)
        {
            try
            {
                var client = _httpClientFactory.CreateClient("ApiClient");

                var httpResponse = await client.GetAsync($"{_apiBaseUrl}/api/Books/get-book-by-id/{id}");
                httpResponse.EnsureSuccessStatusCode();
                ViewBag.Book = await httpResponse.Content.ReadFromJsonAsync<BookDTO>(_jsonOptions);

                var responseAu = await client.GetAsync($"{_apiBaseUrl}/api/Authors/get-all-author");
                responseAu.EnsureSuccessStatusCode();
                ViewBag.ListAuthor = await responseAu.Content.ReadFromJsonAsync<List<authorDTO>>(_jsonOptions);

                var responsePu = await client.GetAsync($"{_apiBaseUrl}/api/Publishers/get-all-publisher");
                responsePu.EnsureSuccessStatusCode();
                ViewBag.ListPublisher = await responsePu.Content.ReadFromJsonAsync<List<publisherDTO>>(_jsonOptions);
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
            }

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> editBook(int id, editBookDTO bookDTO)
        {
            try
            {
                var client = _httpClientFactory.CreateClient("ApiClient");
                var request = new HttpRequestMessage
                {
                    Method = HttpMethod.Put,
                    RequestUri = new Uri($"{_apiBaseUrl}/api/Books/update-book-by-id/{id}"),
                    Content = new StringContent(
                        JsonSerializer.Serialize(bookDTO),
                        Encoding.UTF8,
                        "application/json")
                };

                var response = await client.SendAsync(request);
                response.EnsureSuccessStatusCode();

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                return View(bookDTO);
            }
        }

        [HttpGet]
        public async Task<IActionResult> delBook(int id)
        {
            try
            {
                var client = _httpClientFactory.CreateClient("ApiClient");
                var response = await client.DeleteAsync($"{_apiBaseUrl}/api/Books/delete-book-by-id/{id}");
                response.EnsureSuccessStatusCode();
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
            }
            return RedirectToAction("Index");
        }
    }
}