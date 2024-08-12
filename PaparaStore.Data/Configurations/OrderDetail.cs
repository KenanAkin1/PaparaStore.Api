using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using PaparaStore.Data.Domain;

namespace PaparaStore.Data.Configurations;
public class OrderDetailConfiguration : IEntityTypeConfiguration<OrderDetail>
{
    public void Configure(EntityTypeBuilder<OrderDetail> builder)
    {
        builder.HasKey(od => od.Id);
        builder.Property(x => x.RewardAmount).IsRequired(true).HasPrecision(12, 2);
        builder.Property(x => x.UnitPrice).IsRequired(true).HasPrecision(12, 2);
        builder.HasOne(od => od.Product)
               .WithMany()
               .HasForeignKey(od => od.ProductId);
    }
}