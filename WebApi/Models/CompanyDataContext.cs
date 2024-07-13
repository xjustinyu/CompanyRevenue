using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace WebApi.Models
{
#pragma warning disable CS1591
    public partial class CompanyDataContext : DbContext
    {
        public CompanyDataContext()
        {
        }

        public CompanyDataContext(DbContextOptions<CompanyDataContext> options)
            : base(options)
        {
        }

        public virtual DbSet<CompanyRevenue> CompanyRevenues { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CompanyRevenue>(entity =>
            {
                entity.ToTable("CompanyRevenue");

                entity.HasIndex(e => new { e.CompanyId, e.DataMonth }, "IX_CompanyIdMothUnique")
                    .IsUnique();

                entity.Property(e => e.CompanyName).HasMaxLength(32);

                entity.Property(e => e.CompanyType).HasMaxLength(16);

                entity.Property(e => e.RevenueCompareLastMonthPercentage).HasColumnType("decimal(18, 10)");

                entity.Property(e => e.RevenueCompareLastPeriodPercentage).HasColumnType("decimal(18, 10)");

                entity.Property(e => e.RevenueCompareMonthLastYearPercentage).HasColumnType("decimal(18, 10)");

                entity.Property(e => e.RevenueLastMonth).HasColumnType("decimal(18, 0)");

                entity.Property(e => e.RevenueThisMonth).HasColumnType("decimal(18, 0)");

                entity.Property(e => e.RevenueThisMonthLastYear).HasColumnType("decimal(18, 0)");

                entity.Property(e => e.RevenueTotalLastYear).HasColumnType("decimal(18, 0)");

                entity.Property(e => e.RevenueTotalThisMonth).HasColumnType("decimal(18, 0)");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
#pragma warning restore CS1591
}

