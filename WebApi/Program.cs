var builder = WebApplication.CreateBuilder(args);

// 1. Thêm dịch vụ OpenAPI & Swagger
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(); // <--- Thêm dòng này

var app = builder.Build();

// 2. Bật middleware hiển thị Swagger UI
// Lưu ý: Đặt phần này ở NGOÀI khối "if (app.Environment.IsDevelopment())" để luôn luôn chạy được
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "My Web API v1");
    c.RoutePrefix = "swagger"; // Đường dẫn truy cập sẽ là /swagger
});

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();