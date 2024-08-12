using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PaparaStore.Data.Domain;

namespace PaparaStore.Data.Configuration
{
    public class CartProductConfiguration : IEntityTypeConfiguration<CartProduct>
    {
        public void Configure(EntityTypeBuilder<CartProduct> builder)
        {
            builder.HasKey(cp => new { cp.CartId, cp.ProductId });
            builder.Property(cp => cp.Quantity).IsRequired();
            builder.Property(x => x.RewardAmount).IsRequired(true).HasPrecision(12, 2);

            builder.HasOne(cp => cp.Cart)
                   .WithMany(c => c.CartProducts)
                   .HasForeignKey(cp => cp.CartId);

            builder.HasOne(cp => cp.Product)
                   .WithMany(p => p.CartProducts)
                   .HasForeignKey(cp => cp.ProductId);
        }
    }
}
