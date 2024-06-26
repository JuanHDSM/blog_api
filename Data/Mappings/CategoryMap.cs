using Blog.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Blog.Data.Mappings
{
    public class CategoryMap : IEntityTypeConfiguration<Category>
    {
        public void Configure(EntityTypeBuilder<Category> builder)
        {
            builder.ToTable("category");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id)
                .ValueGeneratedOnAdd()
                .UseMySqlIdentityColumn();

            builder.Property(x => x.Name)
                .IsRequired()
                .HasColumnName("name")
                .HasColumnType("VARCHAR")
                .HasMaxLength(80);

            builder.Property(x => x.Slug)
                .IsRequired()
                .HasColumnName("slug")
                .HasColumnType("VARCHAR")
                .HasMaxLength(80);

            builder.HasIndex(x => x.Slug, "IX_Catgory_Slug")
                .IsUnique();
        }
    }
}