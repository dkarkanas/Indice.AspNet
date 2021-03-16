using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Indice.AspNetCore.Fido.EntityFrameworkCore
{
    internal class DbFidoPublicKeyCredentialMap : IEntityTypeConfiguration<DbFidoPublicKeyCredential>
    {
        public void Configure(EntityTypeBuilder<DbFidoPublicKeyCredential> builder) {
            // Configure table name and schema.
            builder.ToTable("PublicKeyCredential", "fido");
            // Configure primary key.
            builder.HasKey(x => x.Id);
            // Configure fields.
            builder.Property(x => x.UserId).IsRequired();
            builder.Property(x => x.DeviceFriendlyName);
            builder.Property(x => x.Id).HasConversion(x => Convert.ToBase64String(x), x => Convert.FromBase64String(x)).IsRequired();
            builder.Property(x => x.UserHandle).HasConversion(x => Convert.ToBase64String(x), x => Convert.FromBase64String(x)).IsRequired();
            // Configure indexes.
            builder.HasIndex(x => x.UserId).IsUnique(false);
        }
    }
}
