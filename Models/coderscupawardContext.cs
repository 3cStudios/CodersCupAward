﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace CodersCupAward.Models;

public partial class coderscupawardContext : DbContext
{
    public coderscupawardContext(DbContextOptions<coderscupawardContext> options)
        : base(options)
    {
    }

    public virtual DbSet<ApplicationRoles> ApplicationRoles { get; set; }

    public virtual DbSet<ApplicationSession> ApplicationSession { get; set; }

    public virtual DbSet<ApplicationUser> ApplicationUser { get; set; }

    public virtual DbSet<ApplicationUserActivityLog> ApplicationUserActivityLog { get; set; }

    public virtual DbSet<ApplicationUserPhoto> ApplicationUserPhoto { get; set; }

    public virtual DbSet<ApplicationUserRole> ApplicationUserRole { get; set; }

    public virtual DbSet<CoderPointMetric> CoderPointMetric { get; set; }

    public virtual DbSet<CoderPointTracking> CoderPointTracking { get; set; }

    public virtual DbSet<EmailLog> EmailLog { get; set; }

    public virtual DbSet<HtmlTemplate> HtmlTemplate { get; set; }

    public virtual DbSet<Iteration> Iteration { get; set; }

    public virtual DbSet<Organization> Organization { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ApplicationRoles>(entity =>
        {
            entity.HasKey(e => e.ApplicationRolesId).HasName("PK_ApplicationRoles_ApplicationRolesId");

            entity.Property(e => e.Description).HasMaxLength(500);
            entity.Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(250);
        });

        modelBuilder.Entity<ApplicationSession>(entity =>
        {
            entity.HasKey(e => e.ApplicationSessionId).HasName("PK_ApplicationSession_UserSessionId");

            entity.Property(e => e.ApplicationSessionId).ValueGeneratedNever();
            entity.Property(e => e.CreatedDate).HasDefaultValueSql("(getutcdate())");
        });

        modelBuilder.Entity<ApplicationUser>(entity =>
        {
            entity.HasKey(e => e.ApplicationUserId).HasName("PK_Elev8User");

            entity.Property(e => e.ApplicationUserUniqueId).HasDefaultValueSql("(newid())");
            entity.Property(e => e.CellPhoneNumber).HasMaxLength(255);
            entity.Property(e => e.CreatedBy)
                .IsRequired()
                .HasMaxLength(250);
            entity.Property(e => e.CreatedDate).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.EmailAddress)
                .IsRequired()
                .HasMaxLength(250);
            entity.Property(e => e.EmailAddressVerified).HasDefaultValueSql("((0))");
            entity.Property(e => e.FirstName).HasMaxLength(250);
            entity.Property(e => e.LastName).HasMaxLength(250);
            entity.Property(e => e.ModifiedBy).HasMaxLength(250);
            entity.Property(e => e.PasswordHash).HasMaxLength(250);
            entity.Property(e => e.Username).HasMaxLength(250);

            entity.HasOne(d => d.Organization).WithMany(p => p.ApplicationUser).HasForeignKey(d => d.OrganizationId);
        });

        modelBuilder.Entity<ApplicationUserActivityLog>(entity =>
        {
            entity.HasKey(e => e.ApplicationUserActivityLogId).HasName("PK_ApplicationUserActivityLog_ApplicationUserActivityLogId");

            entity.Property(e => e.Activity).HasMaxLength(250);
            entity.Property(e => e.ActivityDate).HasDefaultValueSql("(getdate())");

            entity.HasOne(d => d.ApplicationUser).WithMany(p => p.ApplicationUserActivityLog).HasForeignKey(d => d.ApplicationUserId);
        });

        modelBuilder.Entity<ApplicationUserPhoto>(entity =>
        {
            entity.HasKey(e => e.ApplicationUserPhotoId).HasName("PK_ApplicationUserPhoto_ApplicationUserPhoto");

            entity.Property(e => e.CreatedBy)
                .IsRequired()
                .HasMaxLength(50);
            entity.Property(e => e.CreatedDate).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.ModifiedBy).HasMaxLength(50);
            entity.Property(e => e.Photo).IsUnicode(false);

            entity.HasOne(d => d.ApplicationUser).WithMany(p => p.ApplicationUserPhoto).HasForeignKey(d => d.ApplicationUserId);
        });

        modelBuilder.Entity<ApplicationUserRole>(entity =>
        {
            entity.HasKey(e => e.ApplicationUserRoleId).HasName("PK_ApplicationUserRole_ApplicationUserRoleId");

            entity.Property(e => e.CreatedBy)
                .IsRequired()
                .HasMaxLength(50);
            entity.Property(e => e.CreatedDate).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.ModifiedBy).HasMaxLength(50);

            entity.HasOne(d => d.ApplicationRoles).WithMany(p => p.ApplicationUserRole)
                .HasForeignKey(d => d.ApplicationRolesId)
                .OnDelete(DeleteBehavior.ClientSetNull);

            entity.HasOne(d => d.ApplicationUser).WithMany(p => p.ApplicationUserRole).HasForeignKey(d => d.ApplicationUserId);
        });

        modelBuilder.Entity<CoderPointMetric>(entity =>
        {
            entity.HasKey(e => e.CoderPointMetricId).HasName("PK_CoderPointMetric_CoderPointMetricId");

            entity.Property(e => e.CreatedBy)
                .IsRequired()
                .HasMaxLength(250);
            entity.Property(e => e.CreatedDate).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.IsActive)
                .IsRequired()
                .HasDefaultValueSql("((1))");
            entity.Property(e => e.MetricDescription)
                .IsRequired()
                .HasMaxLength(150);
            entity.Property(e => e.ModifiedBy).HasMaxLength(250);

            entity.HasOne(d => d.Organization).WithMany(p => p.CoderPointMetric).HasForeignKey(d => d.OrganizationId);
        });

        modelBuilder.Entity<CoderPointTracking>(entity =>
        {
            entity.HasKey(e => e.CoderPointTrackingId).HasName("PK_CoderPointTracking_CoderPointTrackingId");

            entity.Property(e => e.CreatedBy)
                .IsRequired()
                .HasMaxLength(250);
            entity.Property(e => e.CreatedDate).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.EntryReference)
                .IsRequired()
                .HasMaxLength(150);
            entity.Property(e => e.IsActive)
                .IsRequired()
                .HasDefaultValueSql("((1))");
            entity.Property(e => e.ModifiedBy).HasMaxLength(250);

            entity.HasOne(d => d.ApplicationUser).WithMany(p => p.CoderPointTracking).HasForeignKey(d => d.ApplicationUserId);
        });

        modelBuilder.Entity<EmailLog>(entity =>
        {
            entity.HasKey(e => e.EmailLogId).HasName("PK_EmailLog_EmailLogId");

            entity.Property(e => e.AttachmentName)
                .HasMaxLength(500)
                .IsUnicode(false);
            entity.Property(e => e.AttachmentPath)
                .HasMaxLength(500)
                .IsUnicode(false);
            entity.Property(e => e.Body).IsUnicode(false);
            entity.Property(e => e.EmailCc)
                .HasMaxLength(500)
                .IsUnicode(false);
            entity.Property(e => e.EmailFromAddress)
                .IsRequired()
                .HasMaxLength(500)
                .IsUnicode(false);
            entity.Property(e => e.EmailTo)
                .IsRequired()
                .HasMaxLength(500)
                .IsUnicode(false);
            entity.Property(e => e.InsertDate).HasColumnType("datetime");
            entity.Property(e => e.ReplyTo)
                .HasMaxLength(500)
                .IsUnicode(false);
            entity.Property(e => e.Subject)
                .IsRequired()
                .HasMaxLength(2500)
                .IsUnicode(false);
        });

        modelBuilder.Entity<HtmlTemplate>(entity =>
        {
            entity.HasKey(e => e.HtmlTemplateId).HasName("PK_HtmlTemplate_HtmlTemplateId");

            entity.Property(e => e.CreatedBy).HasMaxLength(50);
            entity.Property(e => e.CreatedDate).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.HTML).IsUnicode(false);
            entity.Property(e => e.ModifiedBy).HasMaxLength(50);
            entity.Property(e => e.TemplateName)
                .HasMaxLength(150)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Iteration>(entity =>
        {
            entity.HasKey(e => e.IterationId).HasName("PK_CodersCupAwardSeason_CodersCupAwardSeasonId");

            entity.Property(e => e.CreatedBy)
                .IsRequired()
                .HasMaxLength(250);
            entity.Property(e => e.CreatedDate).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.IsActive)
                .IsRequired()
                .HasDefaultValueSql("((1))");
            entity.Property(e => e.ModifiedBy).HasMaxLength(250);
            entity.Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(255);

            entity.HasOne(d => d.Organization).WithMany(p => p.Iteration)
                .HasForeignKey(d => d.OrganizationId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<Organization>(entity =>
        {
            entity.HasKey(e => e.OrganizationId).HasName("PK_Organization_OrganizationId");

            entity.Property(e => e.Address).HasMaxLength(350);
            entity.Property(e => e.CreatedBy)
                .IsRequired()
                .HasMaxLength(250);
            entity.Property(e => e.CreatedDate).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.DepartmentTeamName)
                .IsRequired()
                .HasMaxLength(150);
            entity.Property(e => e.IsActive)
                .IsRequired()
                .HasDefaultValueSql("((1))");
            entity.Property(e => e.ModifiedBy).HasMaxLength(250);
            entity.Property(e => e.OrganizationName)
                .IsRequired()
                .HasMaxLength(150);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}