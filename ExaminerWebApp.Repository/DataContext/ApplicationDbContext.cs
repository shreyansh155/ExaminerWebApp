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

    public virtual DbSet<Applicant> Applicants { get; set; }

    public virtual DbSet<ApplicantType> ApplicantTypes { get; set; }

    public virtual DbSet<Examiner> Examiners { get; set; }

    public virtual DbSet<ExaminerType> ExaminerTypes { get; set; }

    public virtual DbSet<Status> Statuses { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            var connectionString = _configuration.GetConnectionString("DefaultConnection");
            optionsBuilder.UseNpgsql(connectionString);
        }
    }

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

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
