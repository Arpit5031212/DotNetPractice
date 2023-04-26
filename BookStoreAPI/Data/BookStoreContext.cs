using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography.X509Certificates;

namespace BookStoreAPI.Data
{
    public class BookStoreContext : DbContext
    {
        public BookStoreContext(DbContextOptions<BookStoreContext> options) : base(options) { }

        public DbSet<Books> Books { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //we can use services.DbContext<> in startup.cs to create instance of database
            //optionsBuilder.UseSqlServer("Server=.;Database=BookStoreAPI;Integrated Security=True");
            base.OnConfiguring(optionsBuilder);

            
        }
    }
}
