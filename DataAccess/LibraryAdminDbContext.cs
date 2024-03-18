using LibraryAdmin.DataAccess.Configurations;
using LibraryAdmin.DataAccess.Models;
using Microsoft.EntityFrameworkCore;

namespace LibraryAdmin.DataAccess
{
    public class LibraryAdminDbContext: DbContext
    {
        public LibraryAdminDbContext(DbContextOptions<LibraryAdminDbContext> options) : base(options) { }

        public DbSet<AuthorEntity> Authrors { get; set; }
        public DbSet<BookEntity> Books { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new AuthorEntityConfig());
            modelBuilder.ApplyConfiguration(new BookEntityConfig());

            base.OnModelCreating(modelBuilder);
        }
    }
}
