using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace FruitStore.Models.Entities;

public partial class FruteriashopContext : DbContext
{
    public FruteriashopContext()
    {
    }

    public FruteriashopContext(DbContextOptions<FruteriashopContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Categorias> Categorias { get; set; }

    public virtual DbSet<Productos> Productos { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseMySql("server=localhost;user=root;database=fruteriashop;password=root", Microsoft.EntityFrameworkCore.ServerVersion.Parse("8.0.30-mysql"));

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .UseCollation("utf8mb3_general_ci")
            .HasCharSet("utf8mb3");

        modelBuilder.Entity<Categorias>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity
                .ToTable("categorias")
                .HasCharSet("latin1")
                .UseCollation("latin1_swedish_ci");

            entity.HasIndex(e => e.Nombre, "NombreGrupo");

            entity.Property(e => e.Nombre).HasMaxLength(50);
        });

        modelBuilder.Entity<Productos>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity
                .ToTable("productos")
                .HasCharSet("latin1")
                .UseCollation("latin1_swedish_ci");

            entity.HasIndex(e => e.IdCategoria, "GruposProductos");

            entity.HasIndex(e => e.Id, "IdProducto");

            entity.Property(e => e.Descripcion).HasColumnType("text");
            entity.Property(e => e.Nombre).HasMaxLength(50);
            entity.Property(e => e.Precio).HasPrecision(19, 4);
            entity.Property(e => e.UnidadMedida).HasMaxLength(45);

            entity.HasOne(d => d.IdCategoriaNavigation).WithMany(p => p.Productos)
                .HasForeignKey(d => d.IdCategoria)
                .HasConstraintName("fk_categorias");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
