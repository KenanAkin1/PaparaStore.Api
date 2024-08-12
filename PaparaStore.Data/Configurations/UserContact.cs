using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using PaparaStore.Data.Domain;

namespace PaparaStore.Data.Configurations;
public class UserContactConfiguration : IEntityTypeConfiguration<UserContact>
{
    public void Configure(EntityTypeBuilder<UserContact> builder)
    {
        builder.ToTable("UserContact", "dbo");
        builder.Property(x => x.InsertDate).IsRequired();

        builder.Property(x => x.UserId).IsRequired(true);
        builder.Property(x => x.CountryCode).IsRequired().HasMaxLength(4);
        builder.Property(x => x.Phone).IsRequired().HasMaxLength(10);
        builder.Property(x => x.IsDefault).IsRequired();

        builder.HasIndex(x => new { x.CountryCode, x.Phone }).IsUnique();
    }
}