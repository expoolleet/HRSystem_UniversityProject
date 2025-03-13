using Domain.Companies;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.DbContext.Configs;

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
            .HasIndex(x => x.Name)
            .IsUnique();
    }
}