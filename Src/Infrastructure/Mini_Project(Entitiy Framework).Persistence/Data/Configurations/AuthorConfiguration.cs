using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Mini_Project_Entitiy_Framework_.Domain.Entities;

namespace OnlineLibrary.Infrastructure.Persistence.Configurations;

public class AuthorConfiguration : IEntityTypeConfiguration<Author>
{
    public void Configure(EntityTypeBuilder<Author> builder)
    {
        builder.ToTable("Authors");
        builder.HasKey(a => a.Id);

        builder.Property(a => a.Name)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(a => a.Surname)
            .HasMaxLength(100);

        builder.Property(a => a.Gender)
            .HasConversion<string>()
            .HasMaxLength(20)
            .IsRequired();
    }
}
