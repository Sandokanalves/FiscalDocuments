using FiscalDocuments.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FiscalDocuments.Infrastructure.Persistence.Configurations;

public class FiscalDocumentConfiguration : IEntityTypeConfiguration<FiscalDocument>
{
    public void Configure(EntityTypeBuilder<FiscalDocument> builder)
    {
        builder.ToTable("FiscalDocuments");

        builder.HasKey(f => f.Id);

        builder.Property(f => f.AccessKey)
            .HasMaxLength(44)
            .IsRequired();

        builder.HasIndex(f => f.AccessKey)
            .IsUnique();

        builder.Property(f => f.IssuerCnpj)
            .HasMaxLength(14)
            .IsRequired();

        builder.Property(f => f.RecipientCnpj)
            .HasMaxLength(14)
            .IsRequired();

        builder.Property(f => f.TotalAmount)
            .HasColumnType("decimal(18,2)")
            .IsRequired();
    }
}

