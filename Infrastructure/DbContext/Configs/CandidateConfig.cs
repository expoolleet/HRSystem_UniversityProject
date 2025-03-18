using Domain.Candidates;
using Domain.Vacancies;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.DbContext.Configs;

public class CandidateConfig : IEntityTypeConfiguration<Candidate>
{
    public void Configure(EntityTypeBuilder<Candidate> builder)
    {
        builder
            .HasKey(c => c.Id);
        
        builder
            .HasOne<Vacancy>()
            .WithMany()
            .HasForeignKey(c => c.VacancyId)
            .OnDelete(DeleteBehavior.Restrict);

        builder
            .OwnsOne(
                c => c.Workflow,
                workflowBuilder =>
                {
                    workflowBuilder
                        .WithOwner()
                        .HasForeignKey("CandidateId");

                    workflowBuilder
                        .HasKey("CandidateId");

                    workflowBuilder
                        .OwnsOne(
                            c => c.Steps,
                            stepBuilder =>
                            {
                                stepBuilder
                                    .Property<Guid>("Id");
                                
                                stepBuilder
                                    .HasKey("Id");
                                
                                stepBuilder
                                    .WithOwner()
                                    .HasForeignKey("CandidateWorkflowId");
                            });
                    
                    workflowBuilder
                        .ToTable("CandidateWorkflow");
                });
    }
}