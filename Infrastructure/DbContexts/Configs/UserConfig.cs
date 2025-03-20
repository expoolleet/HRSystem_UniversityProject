using Domain.Companies;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.DbContexts.Configs;

public class UserConfig : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {   
        builder
            .HasKey(x => x.Id);

        builder
            .HasOne<Role>()
            .WithMany()
            .HasForeignKey(x => x.RoleId);
        
        builder
            .Property(x => x.Name)
            .IsRequired()
            .HasMaxLength(50);
        
        builder
            .HasIndex(x => x.Name)
            .IsUnique();
        
        builder
            .HasOne<Company>()
            .WithMany()
            .HasForeignKey(x => x.CompanyId);

        builder
            .HasIndex(x => x.CompanyId);

        builder
            .OwnsOne(
                x => x.Password,
                passwordBuilder =>
                {
                    passwordBuilder
                        .Property(p => p.Hash)
                        .HasColumnName("PasswordHash")
                        .IsRequired();

                    passwordBuilder
                        .Property(p => p.Salt)
                        .HasColumnName("PasswordSalt")
                        .IsRequired();
                });
    }
}