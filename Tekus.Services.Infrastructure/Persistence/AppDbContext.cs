using Microsoft.EntityFrameworkCore;
using Tekus.Services.Domain.Entities;

namespace Tekus.Services.Infrastructure.Persistence
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        public DbSet<Country> Countries => Set<Country>();
        public DbSet<Service> Services => Set<Service>();
        public DbSet<Provider> Providers => Set<Provider>();
        public DbSet<ProviderCustomField> ProviderCustomFields => Set<ProviderCustomField>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // ----------------------
            // Country
            // ----------------------
            modelBuilder.Entity<Country>(entity =>
            {
                entity.ToTable("Countries");

                entity.HasKey(c => c.Id);

                entity.Property(c => c.Name)
                      .IsRequired()
                      .HasMaxLength(100);

                entity.Property(c => c.IsoCode)
                      .IsRequired()
                      .HasMaxLength(10);
            });

            // ----------------------
            // Service
            // ----------------------
            modelBuilder.Entity<Service>(entity =>
            {
                entity.ToTable("Services");

                entity.HasKey(s => s.Id);

                entity.Property(s => s.Name)
                      .IsRequired()
                      .HasMaxLength(100);

                entity.Property(s => s.Description)
                      .HasMaxLength(500);

                entity.Property(s => s.HourlyRate)
                      .HasColumnType("decimal(18,2)");
            });

            // ----------------------
            // Provider
            // ----------------------
            modelBuilder.Entity<Provider>(entity =>
            {
                entity.ToTable("Providers");

                entity.HasKey(p => p.Id);

                entity.Property(p => p.Nit)
                      .IsRequired()
                      .HasMaxLength(50);

                entity.Property(p => p.Name)
                      .IsRequired()
                      .HasMaxLength(150);

                entity.Property(p => p.Email)
                      .HasMaxLength(200);

                entity.Property(p => p.PhoneNumber)
                      .HasMaxLength(50);

                entity.HasOne(p => p.Country)
                      .WithMany()
                      .HasForeignKey(p => p.CountryId)
                      .OnDelete(DeleteBehavior.Restrict);

                // Relación muchos-a-muchos con configuración explícita
                entity
                    .HasMany(p => p.Services)
                    .WithMany()
                    .UsingEntity<Dictionary<string, object>>(
                        "ProviderServices", // nombre de la tabla intermedia
                        j => j
                            .HasOne<Service>()
                            .WithMany()
                            .HasForeignKey("ServiceId")
                            .HasConstraintName("FK_ProviderServices_Services_ServiceId")
                            .OnDelete(DeleteBehavior.Cascade),
                        j => j
                            .HasOne<Provider>()
                            .WithMany()
                            .HasForeignKey("ProviderId")
                            .HasConstraintName("FK_ProviderServices_Providers_ProviderId")
                            .OnDelete(DeleteBehavior.Cascade),
                        j =>
                        {
                            j.HasKey("ProviderId", "ServiceId");
                            j.ToTable("ProviderServices");
                        });
            });

            // ----------------------
            // ProviderCustomField
            // ----------------------
            modelBuilder.Entity<ProviderCustomField>(entity =>
            {
                entity.ToTable("ProviderCustomFields");

                entity.HasKey(f => f.Id);

                entity.Property(f => f.FieldName)
                      .IsRequired()
                      .HasMaxLength(200);

                entity.Property(f => f.FieldValue)
                      .IsRequired()
                      .HasMaxLength(1000);

                entity.HasOne(f => f.Provider)
                      .WithMany(p => p.CustomFields)
                      .HasForeignKey(f => f.ProviderId)
                      .OnDelete(DeleteBehavior.Cascade);
            });
        }
    }
}