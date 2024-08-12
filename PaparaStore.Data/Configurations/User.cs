using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using PaparaStore.Data.Domain;

namespace PaparaStore.Data.Configurations;
public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("User", "dbo");
        builder.HasKey(c => c.Id);
        builder.Property(x => x.InsertDate).IsRequired(true);

        builder.Property(x => x.FirstName).IsRequired(true).HasMaxLength(50);
        builder.Property(x => x.LastName).IsRequired(true).HasMaxLength(50);
        builder.Property(x => x.Email).IsRequired(true).HasMaxLength(50);
        builder.Property(x => x.Role).IsRequired(true).HasMaxLength(10);
        builder.Property(x => x.Password).IsRequired(true).HasMaxLength(50);
        builder.Property(x => x.LastLoginDate).IsRequired(false);
        builder.Property(x => x.Status).IsRequired(true);

        builder.HasIndex(x => new { x.Email }).IsUnique(true);

        builder.HasMany(x => x.UserContacts).WithOne(x => x.User).HasForeignKey(x => x.UserId).IsRequired(true).OnDelete(DeleteBehavior.Cascade);
        builder.HasOne(x => x.Wallet).WithOne(x => x.User).HasForeignKey<Wallet>(x => x.UserId).IsRequired(true).OnDelete(DeleteBehavior.Cascade);
        builder.HasOne(x => x.Cart).WithOne(x => x.User).HasForeignKey<Cart>(x => x.UserId).IsRequired(true).OnDelete(DeleteBehavior.Cascade);
        builder.HasMany(x => x.Orders).WithOne(x => x.User).HasForeignKey(c => c.UserId).OnDelete(DeleteBehavior.NoAction);


    }
}