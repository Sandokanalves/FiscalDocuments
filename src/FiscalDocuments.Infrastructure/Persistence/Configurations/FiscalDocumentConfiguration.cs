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

        builder.Property(f => f.Series)
            .HasMaxLength(10);

        builder.Property(f => f.IssuerCnpj)
            .HasMaxLength(14)
            .IsRequired();

        builder.OwnsOne(f => f.IssuerAddress, addressBuilder =>
        {
            addressBuilder.Property(a => a.Street).HasColumnName("IssuerStreet");
            addressBuilder.Property(a => a.Number).HasColumnName("IssuerNumber");
            addressBuilder.Property(a => a.District).HasColumnName("IssuerDistrict");
            addressBuilder.Property(a => a.CityCode).HasColumnName("IssuerCityCode");
            addressBuilder.Property(a => a.CityName).HasColumnName("IssuerCityName");
            addressBuilder.Property(a => a.State).HasColumnName("IssuerState");
            addressBuilder.Property(a => a.ZipCode).HasColumnName("IssuerZipCode");
        });

        builder.OwnsOne(f => f.RecipientAddress, addressBuilder =>
        {
            addressBuilder.Property(a => a.Street).HasColumnName("RecipientStreet");
            addressBuilder.Property(a => a.Number).HasColumnName("RecipientNumber");
            addressBuilder.Property(a => a.District).HasColumnName("RecipientDistrict");
            addressBuilder.Property(a => a.CityCode).HasColumnName("RecipientCityCode");
            addressBuilder.Property(a => a.CityName).HasColumnName("RecipientCityName");
            addressBuilder.Property(a => a.State).HasColumnName("RecipientState");
            addressBuilder.Property(a => a.ZipCode).HasColumnName("RecipientZipCode");
        });

        builder.Property(f => f.TotalAmount)
            .HasColumnType("decimal(18,2)");

        builder.Property(f => f.TotalProducts)
            .HasColumnType("decimal(18,2)");

        builder.HasMany(f => f.Items)
            .WithOne(i => i.FiscalDocument)
            .HasForeignKey(i => i.FiscalDocumentId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}



