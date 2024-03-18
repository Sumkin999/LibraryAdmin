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

            builder.HasIndex(x => new { x.Name, x.BirthDate})
                .IsUnique()
                .HasDatabaseName("ux_author_name_birthdate");
        }
    }
}
