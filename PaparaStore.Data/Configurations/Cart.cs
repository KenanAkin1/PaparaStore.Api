using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PaparaStore.Data.Domain;

namespace PaparaStore.Data.Configuration
{
    public class CartConfiguration : IEntityTypeConfiguration<Cart>
    {
        public void Configure(EntityTypeBuilder<Cart> builder)
        {
            builder.HasKey(c => c.Id);
            builder.Property(c => c.UserId).IsRequired().HasMaxLength(50);
            builder.Property(c => c.ProductAmount).IsRequired().HasMaxLength(2);
            builder.Property(x => x.TotalPrice).IsRequired(true).HasPrecision(12, 2);
            builder.Property(x => x.TotalRewardPoints).IsRequired(true).HasPrecision(12, 2);

            builder.HasMany(c => c.CartProducts)
                   .WithOne(cp => cp.Cart)
                   .HasForeignKey(cp => cp.CartId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
