using CleanArchitecture.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CleanArchitecture.Infrastructure.Data.Configurations;

public class PaymentConfiguration : IEntityTypeConfiguration<Payment>
{
    public void Configure(EntityTypeBuilder<Payment> builder)
    {
        builder.OwnsOne(p => p.Amount);
        builder.Property(p => p.PaymentDate).IsRequired();
        builder.HasOne(p => p.Order)
            .WithMany()
            .HasForeignKey(p => p.OrderId);
    }
} 