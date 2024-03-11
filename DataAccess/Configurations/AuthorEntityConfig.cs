using LibraryAdmin.DataAccess.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LibraryAdmin.DataAccess.Configurations
{
    public class AuthorEntityConfig : IEntityTypeConfiguration<AuthorEntity>
    {
        public void Configure(EntityTypeBuilder<AuthorEntity> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Name).HasMaxLength(128);

            // Что не сработало :(
            //builder.ToTable(t => t.HasCheckConstraint("ValidDate", "AuthorEntity.BirthDate > 1900-01-01"));
            builder.HasAlternateKey(p=>new {p.BirthDate, p.Name});

            /*Убрал, тк при изменении автора удалялась и книга
             * Этот момент описан в AuthorRepository.EditAuthor
            */
            //builder.HasMany(x=>x.Books).WithOne(x=>x.AuthorEntity).OnDelete(DeleteBehavior.Cascade);
        }
    }
}
