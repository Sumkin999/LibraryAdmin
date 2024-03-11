using LibraryAdmin.DataAccess.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LibraryAdmin.DataAccess.Configurations
{
    public class BookEntityConfig: IEntityTypeConfiguration<BookEntity>
    {
        public void Configure(EntityTypeBuilder<BookEntity> builder) 
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Title).HasMaxLength(256);
            //builder.Property(x => x.BooksAmount).HasAnnotation("Range", new int[0, Int32.MaxValue]);
            builder.HasOne(a=>a.AuthorEntity).WithMany(x=>x.Books).HasForeignKey(x=>x.AuthorId).IsRequired();
            builder.HasAlternateKey(p => new { p.Title, p.Year, p.AuthorId });
        }
    }
}
