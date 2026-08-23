using DoctorService.Models;
using Microsoft.EntityFrameworkCore;

namespace DoctorService.Data;

public class DoctorDbContext : DbContext
{
    public DoctorDbContext(DbContextOptions<DoctorDbContext> options) : base(options) { }

    public DbSet<Doctor> Doctors { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Doctor>(entity =>
        {
            entity.ToTable("doctors");

            // Configure AUTO_INCREMENT Primary Key
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id)
                  .HasColumnName("id")
                  .ValueGeneratedOnAdd(); // BIGINT AUTO_INCREMENT


            // Map Column Names to snake_case
            entity.Property(e => e.UserId).HasColumnName("user_id").IsRequired();
            entity.HasIndex(e => e.UserId).IsUnique();

            entity.Property(e => e.FullName).HasColumnName("full_name").IsRequired().HasMaxLength(100);
            entity.Property(e => e.Specialization).HasColumnName("specialization").IsRequired().HasMaxLength(100);
            entity.Property(e => e.LicenseNumber).HasColumnName("license_number").IsRequired().HasMaxLength(50);
            entity.Property(e => e.Phone).HasColumnName("phone").HasMaxLength(20);
            entity.Property(e => e.ExperienceYears).HasColumnName("experience_years");
            entity.Property(e => e.ConsultationFee).HasColumnName("consultation_fee").HasPrecision(10, 2);
            entity.Property(e => e.IsActive).HasColumnName("is_active").HasDefaultValue(true);


            // FIX: Explicitly set ColumnType to timestamp for CURRENT_TIMESTAMP compatibility
            entity.Property(e => e.CreatedAt)
                  .HasColumnName("created_at")
                  .HasColumnType("timestamp")
                  .HasDefaultValueSql("CURRENT_TIMESTAMP");

            entity.Property(e => e.UpdatedAt)
                  .HasColumnName("updated_at")
                  .HasColumnType("timestamp")
                  .HasDefaultValueSql("CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP");

        });
    }

}