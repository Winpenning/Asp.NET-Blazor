using ItemApi.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ItemApi.Mappings;

public class ItemMap : IEntityTypeConfiguration<Item>
{
    public void Configure(EntityTypeBuilder<Item> builder)
    {
        builder.ToTable("Item");
        builder.HasKey(x=>x.Id);

        builder.Property(x=>x.Name)
            .IsRequired()
            .HasColumnName("Name")
            .HasColumnType("NVARCHAR");
    }
}