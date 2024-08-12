using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PaparaStore.Data.Domain;

namespace PaparaStore.Data.Configurations;

public class CategoryProductConfiguration : IEntityTypeConfiguration<CategoryProduct>
{
    public void Configure(EntityTypeBuilder<CategoryProduct> builder)
    {
        builder.HasKey(cp => new { cp.CategoryId, cp.ProductId });

        builder
            .HasOne(cp => cp.Category)
            .WithMany(c => c.CategoryProducts)
            .HasForeignKey(cp => cp.CategoryId);

        builder
            .HasOne(cp => cp.Product)
            .WithMany(p => p.CategoryProducts)
            .HasForeignKey(cp => cp.ProductId);
    }
}
