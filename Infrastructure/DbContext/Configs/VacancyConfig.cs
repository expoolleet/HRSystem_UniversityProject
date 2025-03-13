using Domain.Vacancies;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.DbContext.Configs;

public class VacancyConfig : IEntityTypeConfiguration<Vacancy>
{
    public void Configure(EntityTypeBuilder<Vacancy> builder)
    {
        builder
            .HasKey(x => x.Id);

        builder
            .OwnsOne(
                x => x.Workflow,
                workflowBuilder =>
                {
                    workflowBuilder
                        .WithOwner()
                        .HasForeignKey("VacancyId");
                    
                    workflowBuilder
                        .HasKey("VacancyId");

                    workflowBuilder
                        .OwnsOne(
                            x => x.Steps,
                            stepsBuilder =>
                            {
                                stepsBuilder
                                    .Property<Guid>("Id");
                                
                                stepsBuilder
                                    .HasKey("Id");
                                
                                stepsBuilder
                                    .WithOwner()
                                    .HasForeignKey("VacancyWorkflowId");
                            });
                    workflowBuilder
                        .ToTable("VacancyWorkflow");
                });
    }
}