using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using PaparaStore.Data.Domain;

namespace PaparaStore.Data.Configurations;
public class WalletConfiguration : IEntityTypeConfiguration<Wallet>
{
    public void Configure(EntityTypeBuilder<Wallet> builder)
    {
        builder.ToTable("Wallet", "dbo");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.InsertDate).IsRequired();
        builder.Property(x => x.IsActive).IsRequired();

        builder.Property(x => x.UserId).IsRequired(true);
        builder.Property(x => x.Balance).IsRequired(true).HasMaxLength(12).HasPrecision(12, 2);
        builder.Property(x => x.RewardPoints).IsRequired(true).HasMaxLength(12).HasPrecision(12, 2);
        builder.Property(x => x.IsActive).IsRequired(true);

        builder.HasMany(x => x.WalletTransactions)
            .WithOne(x => x.Wallet)
            .HasForeignKey(x => x.WalletId)
            .IsRequired(true)
            .OnDelete(DeleteBehavior.Cascade);
    }

}
