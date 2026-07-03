using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Mini_Project_Entitiy_Framework_.Domain.Entities;

namespace Mini_Project_Entitiy_Framework_.Persistence.Data.Configurations;

public class ReservedItemConfiguration : IEntityTypeConfiguration<ReservedItem>
{
    public void Configure(EntityTypeBuilder<ReservedItem> builder)
    {
        builder.ToTable("ReservedItems");
        builder.HasKey(r => r.Id);

        builder.Property(r => r.FinCode)
            .IsRequired()
            .HasMaxLength(7);

        builder.Property(r => r.StartDate)
            .IsRequired();

        builder.Property(r => r.EndDate)
            .IsRequired();

        builder.Property(r => r.Status)
            .HasConversion<string>()
            .HasMaxLength(20)
            .IsRequired();

        builder.HasOne(r => r.Book)
            .WithMany(b => b.ReservedItems)
            .HasForeignKey(r => r.BookId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
