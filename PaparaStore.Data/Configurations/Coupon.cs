using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using PaparaStore.Data.Domain;

namespace PaparaStore.Data.Configurations;
public class CouponConfiguration : IEntityTypeConfiguration<Coupon>
{
    public void Configure(EntityTypeBuilder<Coupon> builder)
    {

            builder.HasKey(c => c.Id);
            builder.Property(x => x.InsertDate).IsRequired();

            builder.Property(x => x.Code).IsRequired().HasMaxLength(50);
            builder.Property(x => x.MinValue).HasPrecision(18, 2);
            builder.Property(x => x.DiscountAmount).IsRequired().HasPrecision(18, 2);
            builder.Property(x => x.EndDate).IsRequired();
            builder.Property(x => x.CouponQuantity).IsRequired();

            builder.HasMany(c => c.Orders)
                   .WithOne(o => o.Coupon)
                   .HasForeignKey(o => o.CouponCode)
                   .HasPrincipalKey(c => c.Code);
        
    }
}


