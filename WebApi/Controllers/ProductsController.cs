using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        // GET: api/Products
        [HttpGet]
        public IActionResult GetAll()
        {
            return Ok(new[] { "Sản phẩm 1", "Sản phẩm 2", "Sản phẩm 3" });
        }

        // GET: api/Products/1
        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            return Ok($"Thông tin sản phẩm có ID = {id}");
        }

        // POST: api/Products
        [HttpPost]
        public IActionResult Create([FromBody] string productName)
        {
            return Ok($"Đã thêm sản phẩm: {productName}");
        }

        // PUT: api/Products/1
        [HttpPut("{id}")]
        public IActionResult Update(int id, [FromBody] string productName)
        {
            return Ok($"Đã cập nhật sản phẩm ID = {id} thành {productName}");
        }

        // DELETE: api/Products/1
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            return Ok($"Đã xóa sản phẩm ID = {id}");
        }
    }
}