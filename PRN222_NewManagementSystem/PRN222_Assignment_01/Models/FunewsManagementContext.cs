﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace PRN222_Assignment_01.Models;

public partial class FUNewsManagementContext : DbContext
{
    public FUNewsManagementContext()
    {
    }

    public FUNewsManagementContext(DbContextOptions<FUNewsManagementContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Category> Categories { get; set; }

    public virtual DbSet<NewsArticle> NewsArticles { get; set; }

    public virtual DbSet<NewsTag> NewsTags { get; set; }

    public virtual DbSet<SystemAccount> SystemAccounts { get; set; }

    public virtual DbSet<Tag> Tags { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Category>(entity =>
        {
            entity.HasOne(d => d.ParentCategory).WithMany(p => p.InverseParentCategory).HasConstraintName("FK_Category_Category");
        });

        modelBuilder.Entity<NewsArticle>(entity =>
        {
            entity.HasOne(d => d.Category).WithMany(p => p.NewsArticles)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_NewsArticle_Category");

            entity.HasOne(d => d.CreatedBy).WithMany(p => p.NewsArticles)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_NewsArticle_SystemAccount");
        });

        modelBuilder.Entity<NewsTag>(entity =>
        {
            entity.HasOne(d => d.NewsArticle).WithMany(p => p.NewsTags)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_NewsTag_NewsArticle");

            entity.HasOne(d => d.Tag).WithMany(p => p.NewsTags)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_NewsTag_Tag");
        });

        modelBuilder.Entity<Tag>(entity =>
        {
            entity.HasKey(e => e.TagID).HasName("PK_HashTag");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}