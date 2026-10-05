using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using WebAPI_simple.Data;

namespace LTWebAPi.Data
{
    public class BookAuthDbContextFactory : IDesignTimeDbContextFactory<BookAuthDbContext>
    {
        public BookAuthDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<BookAuthDbContext>();
            optionsBuilder.UseSqlServer(
                "Data Source=.\\SQLEXPRESS;Database=bookAPI;Integrated Security=True;Pooling=False;TrustServerCertificate=True;");
            return new BookAuthDbContext(optionsBuilder.Options);
        }
    }
}