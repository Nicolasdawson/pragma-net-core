using Microsoft.EntityFrameworkCore;
using Pragma.Application.Domain.Entities;

namespace Pragma.Application.Infrastructure
{
    public partial class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }

        public DbSet<Usuario> Usuarios { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Usuario>(entity =>
            {
                entity.ToTable("Usuario");

                entity.Property(e => e.Id)
                .HasDefaultValueSql("(newid())");

                entity.Property(e => e.Nombre)
                .HasMaxLength(50)
                .IsUnicode(false)
                .IsRequired();

                entity.Property(e => e.Rut)
                .IsUnicode(false)
                .IsRequired();

                entity.HasIndex(e => e.Rut)
                .IsUnique();

                entity.Property(e => e.Correo)
                .IsUnicode(false)
                .IsRequired(false);

                entity.Property(e => e.FechaNacimiento)
                .IsRequired()
                .HasColumnType("date");

            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
