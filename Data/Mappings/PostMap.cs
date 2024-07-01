using BlogApi.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BlogApi.Data.Mappings
{
    public class PostMap : IEntityTypeConfiguration<Post>
    {
        public void Configure(EntityTypeBuilder<Post> builder)
        {
            builder.ToTable("post");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id)
                .ValueGeneratedOnAdd()
                .UseMySqlIdentityColumn();

            builder.Property(x => x.Title)
                .IsRequired()
                .HasColumnName("title")
                .HasColumnType("VARCHAR(160)");

            builder.Property(x => x.Summary)
                .IsRequired()
                .HasColumnName("summary")
                .HasColumnType("VARCHAR(255)");

            builder.Property(x => x.Body)
                .IsRequired()
                .HasColumnName("body")
                .HasColumnType("TEXT");

            builder.Property(x => x.Slug)
                .IsRequired()
                .HasColumnName("slug")
                .HasColumnType("VARCHAR(80)");

            builder.Property(x => x.CreateDate)
                .IsRequired()
                .HasColumnName("create_date")
                .HasColumnType("DATETIME")
                .HasDefaultValueSql("CURRENT_TIMESTAMP");

            builder.Property(x => x.LastUpdateDate)
                .IsRequired()
                .HasColumnName("last_update_date")
                .HasColumnType("DATETIME")
                .HasDefaultValueSql("CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP");

            builder.HasIndex(x => x.Slug, "IX_Post_Slug")
                .IsUnique();

            builder.HasOne(x => x.Author)
                .WithMany(x => x.Posts)
                .HasForeignKey("author_id")
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(x => x.Category)
                .WithMany(x => x.Posts)
                .HasForeignKey("category_id")
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(x => x.Tags).WithMany(x => x.Posts)
                .UsingEntity<Dictionary<string, object>>(
                "post_tag",
                post => post.HasOne<Tag>()
                    .WithMany()
                    .HasForeignKey("tag_id")
                    .HasConstraintName("post_tag_tag_id")
                    .OnDelete(DeleteBehavior.Cascade),
                tag => tag.HasOne<Post>()
                    .WithMany()
                    .HasForeignKey("post_id")
                    .HasConstraintName("post_tag_post_id")
                    .OnDelete(DeleteBehavior.Cascade)
                );
        }
    }
}
