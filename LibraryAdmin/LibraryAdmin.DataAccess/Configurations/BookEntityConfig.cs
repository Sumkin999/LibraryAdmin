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

            builder.Property(x => x.AuthorId).IsRequired();

            builder.HasOne<AuthorEntity>()
                .WithMany()
                .HasForeignKey(x => x.AuthorId);

            builder.HasIndex(p => new { p.Title, p.Year, p.AuthorId })
                .IsUnique()
                .HasDatabaseName("ux_book_title_year_author_id");

            builder.Property(x => x.BooksAmount).IsConcurrencyToken();
        }
    }
}
