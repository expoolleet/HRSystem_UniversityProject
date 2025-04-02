﻿// <auto-generated />
using System;
using Infrastructure.DbContexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace Infrastructure.Migrations
{
    [DbContext(typeof(MainDbContext))]
    [Migration("20250402172641_InitialCreate")]
    partial class InitialCreate
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "9.0.3");

            modelBuilder.Entity("Domain.Candidates.Candidate", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<Guid?>("ReferralId")
                        .HasColumnType("TEXT");

                    b.Property<Guid>("VacancyId")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("VacancyId");

                    b.ToTable("Candidates");
                });

            modelBuilder.Entity("Domain.Companies.Company", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Company");
                });

            modelBuilder.Entity("Domain.Companies.Role", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Role");
                });

            modelBuilder.Entity("Domain.Companies.User", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<Guid>("CompanyId")
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("TEXT");

                    b.Property<Guid>("RoleId")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("CompanyId");

                    b.HasIndex("Name")
                        .IsUnique();

                    b.HasIndex("RoleId");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("Domain.Vacancies.Vacancy", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<Guid>("CompanyId")
                        .HasColumnType("TEXT");

                    b.Property<string>("Description")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Vacancies");
                });

            modelBuilder.Entity("Domain.Candidates.Candidate", b =>
                {
                    b.HasOne("Domain.Vacancies.Vacancy", null)
                        .WithMany()
                        .HasForeignKey("VacancyId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.OwnsOne("Domain.Candidates.CandidateDocument", "Document", b1 =>
                        {
                            b1.Property<Guid>("CandidateId")
                                .HasColumnType("TEXT");

                            b1.Property<string>("Name")
                                .IsRequired()
                                .HasColumnType("TEXT");

                            b1.Property<string>("Portfolio")
                                .HasColumnType("TEXT");

                            b1.Property<int>("WorkExperience")
                                .HasColumnType("INTEGER");

                            b1.HasKey("CandidateId");

                            b1.ToTable("Candidates");

                            b1.WithOwner()
                                .HasForeignKey("CandidateId");
                        });

                    b.OwnsOne("Domain.Candidates.CandidateWorkflow", "Workflow", b1 =>
                        {
                            b1.Property<Guid>("CandidateId")
                                .HasColumnType("TEXT");

                            b1.Property<DateTime>("CreationTime")
                                .HasColumnType("TEXT");

                            b1.HasKey("CandidateId");

                            b1.ToTable("CandidateWorkflow", (string)null);

                            b1.WithOwner()
                                .HasForeignKey("CandidateId");

                            b1.OwnsMany("Domain.Candidates.CandidateWorkflowStep", "Steps", b2 =>
                                {
                                    b2.Property<Guid>("Id")
                                        .ValueGeneratedOnAdd()
                                        .HasColumnType("TEXT");

                                    b2.Property<Guid>("CandidateWorkflowId")
                                        .HasColumnType("TEXT");

                                    b2.Property<string>("Feedback")
                                        .HasColumnType("TEXT");

                                    b2.Property<DateTime?>("FeedbackDate")
                                        .HasColumnType("TEXT");

                                    b2.Property<int>("Number")
                                        .HasColumnType("INTEGER");

                                    b2.Property<Guid?>("RoleId")
                                        .HasColumnType("TEXT");

                                    b2.Property<int>("Status")
                                        .HasColumnType("INTEGER");

                                    b2.Property<Guid?>("UserId")
                                        .HasColumnType("TEXT");

                                    b2.HasKey("Id");

                                    b2.HasIndex("CandidateWorkflowId");

                                    b2.ToTable("CandidateWorkflowStep");

                                    b2.WithOwner()
                                        .HasForeignKey("CandidateWorkflowId");
                                });

                            b1.Navigation("Steps");
                        });

                    b.Navigation("Document")
                        .IsRequired();

                    b.Navigation("Workflow")
                        .IsRequired();
                });

            modelBuilder.Entity("Domain.Companies.User", b =>
                {
                    b.HasOne("Domain.Companies.Company", null)
                        .WithMany()
                        .HasForeignKey("CompanyId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Domain.Companies.Role", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.OwnsOne("Domain.Companies.Password", "Password", b1 =>
                        {
                            b1.Property<Guid>("UserId")
                                .HasColumnType("TEXT");

                            b1.PrimitiveCollection<string>("Hash")
                                .IsRequired()
                                .HasColumnType("TEXT")
                                .HasColumnName("PasswordHash");

                            b1.PrimitiveCollection<string>("Salt")
                                .IsRequired()
                                .HasColumnType("TEXT")
                                .HasColumnName("PasswordSalt");

                            b1.HasKey("UserId");

                            b1.ToTable("Users");

                            b1.WithOwner()
                                .HasForeignKey("UserId");
                        });

                    b.Navigation("Password")
                        .IsRequired();
                });

            modelBuilder.Entity("Domain.Vacancies.Vacancy", b =>
                {
                    b.OwnsOne("Domain.Vacancies.VacancyWorkflow", "Workflow", b1 =>
                        {
                            b1.Property<Guid>("VacancyId")
                                .HasColumnType("TEXT");

                            b1.Property<DateTime>("CreationTime")
                                .HasColumnType("TEXT");

                            b1.HasKey("VacancyId");

                            b1.ToTable("VacancyWorkflow", (string)null);

                            b1.WithOwner()
                                .HasForeignKey("VacancyId");

                            b1.OwnsMany("Domain.Vacancies.VacancyWorkflowStep", "Steps", b2 =>
                                {
                                    b2.Property<Guid>("Id")
                                        .ValueGeneratedOnAdd()
                                        .HasColumnType("TEXT");

                                    b2.Property<string>("Description")
                                        .IsRequired()
                                        .HasColumnType("TEXT");

                                    b2.Property<int>("Number")
                                        .HasColumnType("INTEGER");

                                    b2.Property<Guid?>("RoleId")
                                        .HasColumnType("TEXT");

                                    b2.Property<Guid?>("UserId")
                                        .HasColumnType("TEXT");

                                    b2.Property<Guid>("VacancyWorkflowId")
                                        .HasColumnType("TEXT");

                                    b2.HasKey("Id");

                                    b2.HasIndex("VacancyWorkflowId");

                                    b2.ToTable("VacancyWorkflowStep");

                                    b2.WithOwner()
                                        .HasForeignKey("VacancyWorkflowId");
                                });

                            b1.Navigation("Steps");
                        });

                    b.Navigation("Workflow")
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
