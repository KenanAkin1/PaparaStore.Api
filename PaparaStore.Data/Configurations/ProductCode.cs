using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using PaparaStore.Data.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PaparaStore.Data.Configurations;
public class ProductCodeConfiguration : IEntityTypeConfiguration<ProductCode>
{
    public void Configure(EntityTypeBuilder<ProductCode> builder)
    {
        builder.HasKey(pc => pc.Id);
        builder.Property(x => x.InsertDate).IsRequired(true);

        builder.Property(x => x.Code).IsRequired(true).HasMaxLength(50);
    }


}
