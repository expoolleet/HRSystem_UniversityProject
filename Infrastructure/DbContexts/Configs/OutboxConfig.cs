using Infrastructure.Outbox;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.DbContexts.Configs;

public class OutboxConfig : IEntityTypeConfiguration<OutboxEvent>
{
    public void Configure(EntityTypeBuilder<OutboxEvent> builder)
    {
        builder
            .HasKey(e => e.Id);
        
        builder
            .Property(e => e.Content)
            .IsRequired();
        
        builder
            .Property(e => e.Type)
            .IsRequired();
    }
}