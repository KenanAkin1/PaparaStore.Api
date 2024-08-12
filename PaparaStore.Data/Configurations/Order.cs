using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using PaparaStore.Data.Domain;

namespace PaparaStore.Data.Configurations;
public class OrderConfiguration : IEntityTypeConfiguration<Order>
{
    public void Configure(EntityTypeBuilder<Order> builder)
    {
        builder.HasKey(o => o.Id);
        builder.Property(x => x.OrderNumber).IsRequired(true).HasMaxLength(10);
        builder.Property(x => x.TotalPrice).IsRequired(true).HasPrecision(12, 2);
        builder.Property(x => x.FinalPrice).IsRequired(true).HasPrecision(12, 2);
        builder.Property(x => x.TotalRewardAmount).IsRequired(true).HasPrecision(12, 2);



        builder.HasMany(o => o.OrderDetails)
               .WithOne(od => od.Order)
               .HasForeignKey(od => od.OrderId)
               .OnDelete(DeleteBehavior.Cascade);
        builder.HasOne(o => o.Coupon)
               .WithMany(c => c.Orders)
               .HasForeignKey(o => o.CouponCode)
               .HasPrincipalKey(c => c.Code)
               .IsRequired(false);
    }
}
