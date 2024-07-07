using EntityFrameworkConsoleApp.Models;
using Microsoft.EntityFrameworkCore;

namespace EntityFrameworkConsoleApp
{
    public class AppDbContext: DbContext
    {
        public DbSet<Book> Books { get; set; }
        public DbSet<Library> Libraries { get; set; }
        public DbSet<Author> Authors { get; set; }
        public DbSet<BookAuthors> BookAuthors { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=localhost;Database=PB201Ef;Trusted_Connection=True;Encrypt=False;");
        }
        
    }
}
