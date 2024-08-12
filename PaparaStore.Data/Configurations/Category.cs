using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PaparaStore.Data.Domain;

namespace PaparaStore.Data.Configurations;

public class CategoryConfiguration : IEntityTypeConfiguration<Category>
{
    public void Configure(EntityTypeBuilder<Category> builder)
    {
        builder.HasKey(c => c.Id);
        builder.Property(x => x.InsertDate).IsRequired(true);

        builder.Property(x => x.Name).IsRequired(true).HasMaxLength(50);
        builder.Property(x => x.Description).HasMaxLength(250);
        builder.Property(x => x.ImageUrl).HasMaxLength(120);
        builder.Property(e => e.Tags)
                .HasConversion(
                    v => string.Join(',', v),
                    v => v.Split(',', StringSplitOptions.RemoveEmptyEntries).ToList())
                .HasMaxLength(250)
                .IsRequired();


        builder
            .HasMany(c => c.CategoryProducts)
            .WithOne(cp => cp.Category)
            .HasForeignKey(cp => cp.CategoryId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
