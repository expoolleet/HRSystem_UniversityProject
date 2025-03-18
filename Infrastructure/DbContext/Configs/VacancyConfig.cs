using Domain.Vacancies;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.DbContext.Configs;

public class VacancyConfig : IEntityTypeConfiguration<Vacancy>
{
    public void Configure(EntityTypeBuilder<Vacancy> builder)
    {
        builder
            .HasKey(v => v.Id);

        builder
            .OwnsOne(
                v => v.Workflow,
                workflowBuilder =>
                {
                    workflowBuilder
                        .WithOwner()
                        .HasForeignKey("VacancyId");
                    
                    workflowBuilder
                        .HasKey("VacancyId");

                    workflowBuilder
                        .OwnsOne(
                            v => v.Steps,
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