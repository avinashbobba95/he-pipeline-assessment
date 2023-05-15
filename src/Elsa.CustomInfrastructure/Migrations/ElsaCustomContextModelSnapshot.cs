﻿// <auto-generated />
using System;
using Elsa.CustomInfrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace Elsa.CustomInfrastructure.Migrations
{
    [DbContext(typeof(ElsaCustomContext))]
    partial class ElsaCustomContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.9")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("Elsa.CustomModels.Answer", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnOrder(0);

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("AnswerText")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("CreatedDateTime")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("LastModifiedDateTime")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("PeriodEnd")
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("datetime2")
                        .HasColumnName("PeriodEnd");

                    b.Property<DateTime>("PeriodStart")
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("datetime2")
                        .HasColumnName("PeriodStart");

                    b.Property<int?>("QuestionChoiceId")
                        .HasColumnType("int");

                    b.Property<int>("QuestionId")
                        .HasMaxLength(450)
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("QuestionChoiceId");

                    b.HasIndex("QuestionId");

                    b.ToTable("Answer");

                    b.ToTable(tb => tb.IsTemporal(ttb =>
                        {
                            ttb
                                .HasPeriodStart("PeriodStart")
                                .HasColumnName("PeriodStart");
                            ttb
                                .HasPeriodEnd("PeriodEnd")
                                .HasColumnName("PeriodEnd");
                        }
                    ));
                });

            modelBuilder.Entity("Elsa.CustomModels.CustomActivityNavigation", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnOrder(0);

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("ActivityId")
                        .IsRequired()
                        .HasMaxLength(450)
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ActivityType")
                        .IsRequired()
                        .HasMaxLength(450)
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("CorrelationId")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("CreatedDateTime")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("LastModifiedDateTime")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("PeriodEnd")
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("datetime2")
                        .HasColumnName("PeriodEnd");

                    b.Property<DateTime>("PeriodStart")
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("datetime2")
                        .HasColumnName("PeriodStart");

                    b.Property<string>("PreviousActivityId")
                        .IsRequired()
                        .HasMaxLength(450)
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("PreviousActivityType")
                        .IsRequired()
                        .HasMaxLength(450)
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("WorkflowInstanceId")
                        .IsRequired()
                        .HasMaxLength(450)
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.ToTable("CustomActivityNavigation");

                    b.ToTable(tb => tb.IsTemporal(ttb =>
                        {
                            ttb
                                .HasPeriodStart("PeriodStart")
                                .HasColumnName("PeriodStart");
                            ttb
                                .HasPeriodEnd("PeriodEnd")
                                .HasColumnName("PeriodEnd");
                        }
                    ));
                });

            modelBuilder.Entity("Elsa.CustomModels.PotScoreOption", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnOrder(0);

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<DateTime>("CreatedDateTime")
                        .HasColumnType("datetime2");

                    b.Property<bool>("IsActive")
                        .HasColumnType("bit");

                    b.Property<DateTime?>("LastModifiedDateTime")
                        .HasColumnType("datetime2");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<DateTime>("PeriodEnd")
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("datetime2")
                        .HasColumnName("PeriodEnd");

                    b.Property<DateTime>("PeriodStart")
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("datetime2")
                        .HasColumnName("PeriodStart");

                    b.HasKey("Id");

                    b.ToTable("PotScoreOption");

                    b.ToTable(tb => tb.IsTemporal(ttb =>
                        {
                            ttb
                                .HasPeriodStart("PeriodStart")
                                .HasColumnName("PeriodStart");
                            ttb
                                .HasPeriodEnd("PeriodEnd")
                                .HasColumnName("PeriodEnd");
                        }
                    ));
                });

            modelBuilder.Entity("Elsa.CustomModels.Question", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnOrder(0);

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("ActivityId")
                        .IsRequired()
                        .HasMaxLength(450)
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Comments")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("CorrelationId")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("CreatedDateTime")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("LastModifiedDateTime")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("PeriodEnd")
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("datetime2")
                        .HasColumnName("PeriodEnd");

                    b.Property<DateTime>("PeriodStart")
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("datetime2")
                        .HasColumnName("PeriodStart");

                    b.Property<int?>("QuestionDataDictionaryId")
                        .HasColumnType("int");

                    b.Property<string>("QuestionId")
                        .HasMaxLength(450)
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("QuestionText")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("QuestionType")
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<decimal?>("Score")
                        .HasColumnType("decimal(18,2)");

                    b.Property<double?>("Weighting")
                        .HasColumnType("float");

                    b.Property<string>("WorkflowInstanceId")
                        .IsRequired()
                        .HasMaxLength(450)
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("QuestionDataDictionaryId");

                    b.ToTable("Question");

                    b.ToTable(tb => tb.IsTemporal(ttb =>
                        {
                            ttb
                                .HasPeriodStart("PeriodStart")
                                .HasColumnName("PeriodStart");
                            ttb
                                .HasPeriodEnd("PeriodEnd")
                                .HasColumnName("PeriodEnd");
                        }
                    ));
                });

            modelBuilder.Entity("Elsa.CustomModels.QuestionChoice", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnOrder(0);

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("Answer")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("CreatedDateTime")
                        .HasColumnType("datetime2");

                    b.Property<string>("Identifier")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("IsExclusiveToQuestion")
                        .HasColumnType("bit");

                    b.Property<bool>("IsPrePopulated")
                        .HasColumnType("bit");

                    b.Property<bool>("IsSingle")
                        .HasColumnType("bit");

                    b.Property<DateTime?>("LastModifiedDateTime")
                        .HasColumnType("datetime2");

                    b.Property<decimal?>("NumericScore")
                        .HasColumnType("decimal(18,2)");

                    b.Property<DateTime>("PeriodEnd")
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("datetime2")
                        .HasColumnName("PeriodEnd");

                    b.Property<DateTime>("PeriodStart")
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("datetime2")
                        .HasColumnName("PeriodStart");

                    b.Property<string>("PotScoreCategory")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("QuestionChoiceGroupId")
                        .HasColumnType("int");

                    b.Property<int>("QuestionId")
                        .HasMaxLength(450)
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("QuestionChoiceGroupId");

                    b.HasIndex("QuestionId");

                    b.ToTable("QuestionChoice");

                    b.ToTable(tb => tb.IsTemporal(ttb =>
                        {
                            ttb
                                .HasPeriodStart("PeriodStart")
                                .HasColumnName("PeriodStart");
                            ttb
                                .HasPeriodEnd("PeriodEnd")
                                .HasColumnName("PeriodEnd");
                        }
                    ));
                });

            modelBuilder.Entity("Elsa.CustomModels.QuestionChoiceGroup", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnOrder(0);

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<DateTime>("CreatedDateTime")
                        .HasColumnType("datetime2");

                    b.Property<string>("GroupIdentifier")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<DateTime?>("LastModifiedDateTime")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("PeriodEnd")
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("datetime2")
                        .HasColumnName("PeriodEnd");

                    b.Property<DateTime>("PeriodStart")
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("datetime2")
                        .HasColumnName("PeriodStart");

                    b.HasKey("Id");

                    b.ToTable("QuestionChoiceGroup");

                    b.ToTable(tb => tb.IsTemporal(ttb =>
                        {
                            ttb
                                .HasPeriodStart("PeriodStart")
                                .HasColumnName("PeriodStart");
                            ttb
                                .HasPeriodEnd("PeriodEnd")
                                .HasColumnName("PeriodEnd");
                        }
                    ));
                });

            modelBuilder.Entity("Elsa.CustomModels.QuestionDataDictionary", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnOrder(0);

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<DateTime>("CreatedDateTime")
                        .HasColumnType("datetime2");

                    b.Property<string>("Description")
                        .HasMaxLength(450)
                        .HasColumnType("nvarchar(450)");

                    b.Property<DateTime?>("LastModifiedDateTime")
                        .HasColumnType("datetime2");

                    b.Property<string>("LegacyName")
                        .HasMaxLength(450)
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(450)
                        .HasColumnType("nvarchar(450)");

                    b.Property<DateTime>("PeriodEnd")
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("datetime2")
                        .HasColumnName("PeriodEnd");

                    b.Property<DateTime>("PeriodStart")
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("datetime2")
                        .HasColumnName("PeriodStart");

                    b.Property<int>("QuestionDataDictionaryGroupId")
                        .HasColumnType("int");

                    b.Property<string>("Type")
                        .HasMaxLength(30)
                        .HasColumnType("nvarchar(30)");

                    b.HasKey("Id");

                    b.HasIndex("QuestionDataDictionaryGroupId");

                    b.ToTable("QuestionDataDictionary");

                    b.ToTable(tb => tb.IsTemporal(ttb =>
                        {
                            ttb
                                .HasPeriodStart("PeriodStart")
                                .HasColumnName("PeriodStart");
                            ttb
                                .HasPeriodEnd("PeriodEnd")
                                .HasColumnName("PeriodEnd");
                        }
                    ));
                });

            modelBuilder.Entity("Elsa.CustomModels.QuestionDataDictionaryGroup", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<DateTime>("CreatedDateTime")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("LastModifiedDateTime")
                        .HasColumnType("datetime2");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(450)
                        .HasColumnType("nvarchar(450)");

                    b.Property<DateTime>("PeriodEnd")
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("datetime2")
                        .HasColumnName("PeriodEnd");

                    b.Property<DateTime>("PeriodStart")
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("datetime2")
                        .HasColumnName("PeriodStart");

                    b.HasKey("Id");

                    b.ToTable("QuestionDataDictionaryGroup");

                    b.ToTable(tb => tb.IsTemporal(ttb =>
                        {
                            ttb
                                .HasPeriodStart("PeriodStart")
                                .HasColumnName("PeriodStart");
                            ttb
                                .HasPeriodEnd("PeriodEnd")
                                .HasColumnName("PeriodEnd");
                        }
                    ));
                });

            modelBuilder.Entity("Elsa.CustomModels.QuestionWorkflowInstance", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnOrder(0);

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("CorrelationId")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<DateTime>("CreatedDateTime")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("LastModifiedDateTime")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("PeriodEnd")
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("datetime2")
                        .HasColumnName("PeriodEnd");

                    b.Property<DateTime>("PeriodStart")
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("datetime2")
                        .HasColumnName("PeriodStart");

                    b.Property<string>("Result")
                        .HasMaxLength(450)
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Score")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("WorkflowDefinitionId")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("WorkflowInstanceId")
                        .IsRequired()
                        .HasMaxLength(450)
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("WorkflowName")
                        .IsRequired()
                        .HasMaxLength(450)
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.ToTable("QuestionWorkflowInstance");

                    b.ToTable(tb => tb.IsTemporal(ttb =>
                        {
                            ttb
                                .HasPeriodStart("PeriodStart")
                                .HasColumnName("PeriodStart");
                            ttb
                                .HasPeriodEnd("PeriodEnd")
                                .HasColumnName("PeriodEnd");
                        }
                    ));
                });

            modelBuilder.Entity("Microsoft.AspNetCore.DataProtection.EntityFrameworkCore.DataProtectionKey", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("FriendlyName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Xml")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("DataProtectionKeys");
                });

            modelBuilder.Entity("Elsa.CustomModels.Answer", b =>
                {
                    b.HasOne("Elsa.CustomModels.QuestionChoice", "Choice")
                        .WithMany()
                        .HasForeignKey("QuestionChoiceId");

                    b.HasOne("Elsa.CustomModels.Question", "Question")
                        .WithMany("Answers")
                        .HasForeignKey("QuestionId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Choice");

                    b.Navigation("Question");
                });

            modelBuilder.Entity("Elsa.CustomModels.Question", b =>
                {
                    b.HasOne("Elsa.CustomModels.QuestionDataDictionary", "QuestionDataDictionary")
                        .WithMany()
                        .HasForeignKey("QuestionDataDictionaryId");

                    b.Navigation("QuestionDataDictionary");
                });

            modelBuilder.Entity("Elsa.CustomModels.QuestionChoice", b =>
                {
                    b.HasOne("Elsa.CustomModels.QuestionChoiceGroup", "QuestionChoiceGroup")
                        .WithMany("QuestionGroupChoices")
                        .HasForeignKey("QuestionChoiceGroupId");

                    b.HasOne("Elsa.CustomModels.Question", "Question")
                        .WithMany("Choices")
                        .HasForeignKey("QuestionId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Question");

                    b.Navigation("QuestionChoiceGroup");
                });

            modelBuilder.Entity("Elsa.CustomModels.QuestionDataDictionary", b =>
                {
                    b.HasOne("Elsa.CustomModels.QuestionDataDictionaryGroup", "Group")
                        .WithMany("QuestionDataDictionaryList")
                        .HasForeignKey("QuestionDataDictionaryGroupId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Group");
                });

            modelBuilder.Entity("Elsa.CustomModels.Question", b =>
                {
                    b.Navigation("Answers");

                    b.Navigation("Choices");
                });

            modelBuilder.Entity("Elsa.CustomModels.QuestionChoiceGroup", b =>
                {
                    b.Navigation("QuestionGroupChoices");
                });

            modelBuilder.Entity("Elsa.CustomModels.QuestionDataDictionaryGroup", b =>
                {
                    b.Navigation("QuestionDataDictionaryList");
                });
#pragma warning restore 612, 618
        }
    }
}
