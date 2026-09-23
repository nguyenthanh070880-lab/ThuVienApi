using Microsoft.EntityFrameworkCore;
using WebAPI_simple.Data;

var builder = WebApplication.CreateBuilder(args);

// 1. Thêm dịch vụ Controllers
builder.Services.AddControllers();

// 2. Thêm dịch vụ Swagger/OpenAPI
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// 3. Đăng ký AppDbContext kết nối tới SQL Server
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(connectionString));

var app = builder.Build();

// 4. Cấu hình HTTP request pipeline cho Swagger
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();