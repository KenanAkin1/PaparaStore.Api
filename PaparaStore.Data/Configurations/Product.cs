using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PaparaStore.Data.Domain;

namespace PaparaStore.Data.Configurations;

public class ProductConfiguration : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        builder.HasKey(p => p.Id);
        builder.Property(x => x.InsertDate).IsRequired(true);

        builder.Property(x => x.LastPrice).IsRequired(true).HasPrecision(10, 2);
        builder.Property(x => x.Price).IsRequired(true).HasPrecision(10, 2);
        builder.Property(x => x.Name).IsRequired(true).HasMaxLength(50);
        builder.Property(x => x.StockQuantity).IsRequired(true).HasMaxLength(5);
        builder.Property(x => x.RewardPercantage).HasMaxLength(2);
        builder.Property(x => x.MaxRewardAmount).HasMaxLength(4).HasPrecision(4, 2);
        builder.Property(x => x.Description).HasMaxLength(250);
        builder.Property(x => x.ImageUrl).HasMaxLength(120);
        builder.Property(x => x.IsActive).IsRequired(true);


        builder
            .HasMany(p => p.CategoryProducts)
            .WithOne(cp => cp.Product)
            .HasForeignKey(cp => cp.ProductId)
            .OnDelete(DeleteBehavior.Cascade); ;
        builder
            .HasMany(p => p.ProductCodes)
            .WithOne(pc => pc.Product)
            .HasForeignKey(pc => pc.ProductId)
            .OnDelete(DeleteBehavior.Cascade); 
    }

            
}
