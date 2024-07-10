using System;
using System.Collections.Generic;
using ExaminerWebApp.Repository.DataModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace ExaminerWebApp.Repository.DataContext;

public partial class ApplicationDbContext : DbContext
{
    private readonly IConfiguration _configuration;

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options, IConfiguration configuration)
      : base(options)
    {
        _configuration = configuration;
    }
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            var connectionString = _configuration.GetConnectionString("DefaultConnection");
            optionsBuilder.UseNpgsql(connectionString);
        }
    }

    public virtual DbSet<Applicant> Applicants { get; set; }

    public virtual DbSet<ApplicantType> ApplicantTypes { get; set; }

    public virtual DbSet<ApplicationTypeTemplate> ApplicationTypeTemplates { get; set; }

    public virtual DbSet<ApplicationTypeTemplatePhase> ApplicationTypeTemplatePhases { get; set; }

    public virtual DbSet<Examiner> Examiners { get; set; }

    public virtual DbSet<ExaminerType> ExaminerTypes { get; set; }

    public virtual DbSet<Phase> Phases { get; set; }

    public virtual DbSet<Status> Statuses { get; set; }

    public virtual DbSet<Step> Steps { get; set; }

    public virtual DbSet<StepType> StepTypes { get; set; }

    public virtual DbSet<TemplatePhaseStep> TemplatePhaseSteps { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Applicant>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("p_key");

            entity.ToTable("applicant");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.ApplicantTypeId).HasColumnName("applicant_type_id");
            entity.Property(e => e.DateOfBirth)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("date_of_birth");
            entity.Property(e => e.Email)
                .HasColumnType("character varying")
                .HasColumnName("email");
            entity.Property(e => e.FileName)
                .HasColumnType("character varying")
                .HasColumnName("file_name");
            entity.Property(e => e.FilePath)
                .HasColumnType("character varying")
                .HasColumnName("file_path");
            entity.Property(e => e.FirstName)
                .HasColumnType("character varying")
                .HasColumnName("first_name");
            entity.Property(e => e.IsDeleted).HasColumnName("is_deleted");
            entity.Property(e => e.LastName)
                .HasColumnType("character varying")
                .HasColumnName("last_name");
            entity.Property(e => e.Phone)
                .HasColumnType("character varying")
                .HasColumnName("phone");

            entity.HasOne(d => d.ApplicantType).WithMany(p => p.Applicants)
                .HasForeignKey(d => d.ApplicantTypeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_setting_id");
        });

        modelBuilder.Entity<ApplicantType>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("Setting_pkey");

            entity.ToTable("applicant_type");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.ApplicantTypeName)
                .HasColumnType("character varying")
                .HasColumnName("applicant_type_name");
        });

        modelBuilder.Entity<ApplicationTypeTemplate>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("application_type_template_pkey");

            entity.ToTable("application_type_template");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CreatedBy)
                .HasColumnType("character varying")
                .HasColumnName("created_by");
            entity.Property(e => e.CreatedDate).HasColumnName("created_date");
            entity.Property(e => e.Description)
                .HasColumnType("character varying")
                .HasColumnName("description");
            entity.Property(e => e.Instruction)
                .HasColumnType("character varying")
                .HasColumnName("instruction");
            entity.Property(e => e.IsDeleted)
                .HasDefaultValueSql("false")
                .HasColumnName("is_deleted");
            entity.Property(e => e.ModifiedBy)
                .HasColumnType("character varying")
                .HasColumnName("modified_by");
            entity.Property(e => e.ModifiedDate).HasColumnName("modified_date");
            entity.Property(e => e.Name)
                .HasColumnType("character varying")
                .HasColumnName("name");
        });

        modelBuilder.Entity<ApplicationTypeTemplatePhase>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("application_type_template_phase_pkey");

            entity.ToTable("application_type_template_phase");

            entity.Property(e => e.Id)
                .UseIdentityAlwaysColumn()
                .HasColumnName("id");
            entity.Property(e => e.IsDeleted).HasColumnName("is_deleted");
            entity.Property(e => e.Ordinal).HasColumnName("ordinal");
            entity.Property(e => e.PhaseId).HasColumnName("phase_id");
            entity.Property(e => e.TemplateId).HasColumnName("template_id");

            entity.HasOne(d => d.Phase).WithMany(p => p.ApplicationTypeTemplatePhases)
                .HasForeignKey(d => d.PhaseId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fkey_phase_id");

            entity.HasOne(d => d.Template).WithMany(p => p.ApplicationTypeTemplatePhases)
                .HasForeignKey(d => d.TemplateId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fkey_template_id");
        });

        modelBuilder.Entity<Examiner>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("examiner_pkey");

            entity.ToTable("examiner");

            entity.Property(e => e.Id)
                .UseIdentityAlwaysColumn()
                .HasColumnName("id");
            entity.Property(e => e.CreatedBy)
                .HasColumnType("character varying")
                .HasColumnName("created_by");
            entity.Property(e => e.CreatedDate)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("created_date");
            entity.Property(e => e.DateOfBirth)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("date_of_birth");
            entity.Property(e => e.Email)
                .HasColumnType("character varying")
                .HasColumnName("email");
            entity.Property(e => e.ExaminerId).HasColumnName("examiner_id");
            entity.Property(e => e.FilePath)
                .HasColumnType("character varying")
                .HasColumnName("file_path");
            entity.Property(e => e.FirstName)
                .HasColumnType("character varying")
                .HasColumnName("first_name");
            entity.Property(e => e.IsDeleted)
                .HasDefaultValueSql("false")
                .HasColumnName("is_deleted");
            entity.Property(e => e.LastName)
                .HasColumnType("character varying")
                .HasColumnName("last_name");
            entity.Property(e => e.ModifiedBy)
                .HasColumnType("character varying")
                .HasColumnName("modified_by");
            entity.Property(e => e.ModifiedDate)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("modified_date");
            entity.Property(e => e.Password)
                .HasColumnType("character varying")
                .HasColumnName("password");
            entity.Property(e => e.Phone)
                .HasColumnType("character varying")
                .HasColumnName("phone");
            entity.Property(e => e.StatusId)
                .HasDefaultValueSql("1")
                .HasColumnName("status_id");

            entity.HasOne(d => d.ExaminerNavigation).WithMany(p => p.Examiners)
                .HasForeignKey(d => d.ExaminerId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_examiner_id");

            entity.HasOne(d => d.Status).WithMany(p => p.Examiners)
                .HasForeignKey(d => d.StatusId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_status_id");
        });

        modelBuilder.Entity<ExaminerType>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("examiner_type_pkey");

            entity.ToTable("examiner_type");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Name)
                .HasColumnType("character varying")
                .HasColumnName("name");
        });

        modelBuilder.Entity<Phase>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("phase_pkey");

            entity.ToTable("phase");

            entity.Property(e => e.Id)
                .UseIdentityAlwaysColumn()
                .HasColumnName("id");
            entity.Property(e => e.CreatedBy)
                .HasColumnType("character varying")
                .HasColumnName("created_by");
            entity.Property(e => e.CreatedDate).HasColumnName("created_date");
            entity.Property(e => e.Description)
                .HasColumnType("character varying")
                .HasColumnName("description");
            entity.Property(e => e.IsDeleted)
                .HasDefaultValueSql("false")
                .HasColumnName("is_deleted");
            entity.Property(e => e.ModifiedBy)
                .HasColumnType("character varying")
                .HasColumnName("modified_by");
            entity.Property(e => e.ModifiedDate).HasColumnName("modified_date");
            entity.Property(e => e.Name)
                .HasColumnType("character varying")
                .HasColumnName("name");
        });

        modelBuilder.Entity<Status>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("status_pkey");

            entity.ToTable("status");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.StatusName)
                .HasColumnType("character varying")
                .HasColumnName("status_name");
        });

        modelBuilder.Entity<Step>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("step_pkey");

            entity.ToTable("step");

            entity.Property(e => e.Id)
                .UseIdentityAlwaysColumn()
                .HasColumnName("id");
            entity.Property(e => e.CreatedBy)
                .HasColumnType("character varying")
                .HasColumnName("created_by");
            entity.Property(e => e.CreatedDate).HasColumnName("created_date");
            entity.Property(e => e.Description)
                .HasColumnType("character varying")
                .HasColumnName("description");
            entity.Property(e => e.Instruction)
                .HasColumnType("character varying")
                .HasColumnName("instruction");
            entity.Property(e => e.IsDeleted)
                .HasDefaultValueSql("false")
                .HasColumnName("is_deleted");
            entity.Property(e => e.ModifiedBy)
                .HasColumnType("character varying")
                .HasColumnName("modified_by");
            entity.Property(e => e.ModifiedDate).HasColumnName("modified_date");
            entity.Property(e => e.Name)
                .HasColumnType("character varying")
                .HasColumnName("name");
            entity.Property(e => e.PhaseId).HasColumnName("phase_id");
            entity.Property(e => e.StepTypeId).HasColumnName("step_type_id");

            entity.HasOne(d => d.Phase).WithMany(p => p.Steps)
                .HasForeignKey(d => d.PhaseId)
                .HasConstraintName("fkey_phase_id");

            entity.HasOne(d => d.StepType).WithMany(p => p.Steps)
                .HasForeignKey(d => d.StepTypeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fkey_step_type_id");
        });

        modelBuilder.Entity<StepType>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("step_type_pkey");

            entity.ToTable("step_type");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Name)
                .HasMaxLength(128)
                .HasColumnName("name");
        });

        modelBuilder.Entity<TemplatePhaseStep>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("template_phase_step_pkey");

            entity.ToTable("template_phase_step");

            entity.Property(e => e.Id)
                .UseIdentityAlwaysColumn()
                .HasColumnName("id");
            entity.Property(e => e.IsDeleted)
                .HasDefaultValueSql("false")
                .HasColumnName("is_deleted");
            entity.Property(e => e.StepId).HasColumnName("step_id");
            entity.Property(e => e.TemplatePhaseId).HasColumnName("template_phase_id");

            entity.HasOne(d => d.Step).WithMany(p => p.TemplatePhaseSteps)
                .HasForeignKey(d => d.StepId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fkey_step_id");

            entity.HasOne(d => d.TemplatePhase).WithMany(p => p.TemplatePhaseSteps)
                .HasForeignKey(d => d.TemplatePhaseId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fkey_template_phase_id");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}