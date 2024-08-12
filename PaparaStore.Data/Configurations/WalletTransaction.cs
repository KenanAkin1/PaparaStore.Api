using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using PaparaStore.Data.Domain;

namespace PaparaStore.Data.Configurations;
public class WalletTransactionConfiguration : IEntityTypeConfiguration<WalletTransaction>
{
    public void Configure(EntityTypeBuilder<WalletTransaction> builder)
    {
        builder.Property(x => x.InsertDate).IsRequired(true);

        builder.Property(x => x.WalletId).IsRequired(true);
        builder.Property(x => x.ReferenceNumber).IsRequired().HasMaxLength(50);
        builder.Property(x => x.InitialAmount).IsRequired().HasPrecision(12, 2);
        builder.Property(x => x.SpentAmount).IsRequired().HasPrecision(12, 2);
        builder.Property(x => x.RemainingAmount).IsRequired().HasPrecision(12, 2);
        builder.Property(x => x.Description).IsRequired().HasMaxLength(50);
        builder.Property(x => x.TransactionDate).IsRequired();

        builder.HasIndex(x => x.WalletId);
    }
}
