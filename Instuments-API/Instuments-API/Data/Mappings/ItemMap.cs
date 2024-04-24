using Instuments_API.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Instuments_API.Data.Mappings;

public class ItemMap : IEntityTypeConfiguration<Item>
{
    public void Configure(EntityTypeBuilder<Item> builder)
    {
        builder.ToTable("Item");
        builder.HasKey(x=> x.Id);

        builder.Property(x => x.Model)
            .IsRequired()
            .HasColumnType("NVARCHAR")
            .HasColumnName("Model");

        builder.Property(x => x.Brand)
            .IsRequired()
            .HasColumnType("NVARCHAR")
            .HasColumnName("Brand");

        builder.Property(x => x.USDPrice)
            .IsRequired()
            .HasColumnType("Money")
            .HasColumnName("Price");

        builder
            .Property(X => X.Type)
            .IsRequired()
            .HasColumnType("NVARCHAR")
            .HasColumnName("Type");
    }
}