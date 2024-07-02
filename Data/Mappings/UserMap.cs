using BlogApi.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BlogApi.Data.Mappings
{
    public class UserMap : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable("user");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id)
                .ValueGeneratedOnAdd()
                .UseMySqlIdentityColumn();

            builder.Property(x => x.Name)
                .IsRequired()
                .HasColumnName("name")
                .HasColumnType("VARCHAR")
                .HasMaxLength(80);

            builder.Property(x => x.Bio).HasColumnName("bio").IsRequired(false);

            builder.Property(x => x.Email)
            .IsRequired()
            .HasColumnName("email")
            .HasColumnType("VARCHAR")
            .HasMaxLength(160);

            builder.Property(x => x.Image).HasColumnName("image").IsRequired(false);
            builder.Property(x => x.PasswordHash)
            .IsRequired()
            .HasColumnName("password_hash")
            .HasColumnType("VARCHAR")
            .HasMaxLength(255);

            builder.Property(x => x.Slug)
                .IsRequired()
                .HasColumnName("slug")
                .HasColumnType("VARCHAR")
                .HasMaxLength(80);

            builder.HasIndex(x => x.Slug, "IX_User_Slug")
            .IsUnique();

            builder.HasIndex(x => x.Slug, "IX_User_Email")
            .IsUnique();

            builder.HasMany(x => x.Roles)
                .WithMany(x => x.Users)
                .UsingEntity<Dictionary<string, object>>(
                    "user_role",
                    user => user.HasOne<Role>()
                    .WithMany()
                    .HasForeignKey("role_id")
                    .HasConstraintName("user_role_role_id")
                    .OnDelete(DeleteBehavior.Cascade),
                    role => role.HasOne<User>()
                        .WithMany()
                        .HasForeignKey("user_id")
                        .HasConstraintName("user_role_user_id")
                        .OnDelete(DeleteBehavior.Cascade)
                );
        }
    }
}