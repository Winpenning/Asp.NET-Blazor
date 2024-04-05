using Blog.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Blog.Data.Mappings;

public class PostMap : IEntityTypeConfiguration<Post>
{
    public void Configure(EntityTypeBuilder<Post> builder)
    {
        builder.ToTable("Post");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id)
            .ValueGeneratedOnAdd()
            .UseIdentityColumn();

        builder.Property(x => x.Title)
            .IsRequired()
            .HasColumnType("NVARCHAR")
            .HasMaxLength(100);

        builder.Property(x => x.Summary)
            .HasColumnType("NVARCHAR")
            .HasMaxLength(200);
        builder.Property(x => x.Body)
            .HasColumnType("NVARCHAR")
            .HasMaxLength(2000);
        builder.Property(x => x.Slug)
            .HasColumnType("NVARCHAR")
            .HasMaxLength(200);
        builder
            .HasIndex(x => x.Slug, "IX_User_Slug")
            .IsUnique();

        builder.HasOne(z => z.Author)
            .WithMany(x => x.Posts)
            .HasConstraintName("FK_Post_Author")
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(x => x.Category)
            .WithMany(x => x.Posts)
            .HasConstraintName("FK_Post_Category")
            .OnDelete(DeleteBehavior.Cascade);

        builder
            .HasMany(sdjuh => sdjuh.Tags)
            .WithMany(x => x.Posts)
            .UsingEntity<Dictionary<string, object>>(
                "PostTag",
                post => post
                    .HasOne<Tag>()
                    .WithMany()
                    .HasForeignKey("PostId")
                    .HasConstraintName("FK_PostRole_PostID")
                    .OnDelete(DeleteBehavior.Cascade),
                tag => tag
                    .HasOne<Post>()
                    .WithMany()
                    .HasForeignKey("TagId")
                    .HasConstraintName("FK_PostTag_Tag_Id")
                    .OnDelete(DeleteBehavior.Cascade));
    }
}