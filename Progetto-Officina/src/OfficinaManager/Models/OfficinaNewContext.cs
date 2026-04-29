using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Officina.API.Models;

public partial class OfficinaNewContext : DbContext
{
    public OfficinaNewContext()
    {
    }

    // il Context deve avere un costruttore specifico che accetta le opzioni dall'esterno, in modo da poter essere configurato correttamente nel Program.cs
    public OfficinaNewContext(DbContextOptions<OfficinaNewContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Cliente> Clientes { get; set; }

    public virtual DbSet<Intervento> Interventos { get; set; }

    public virtual DbSet<Utente> Utentes { get; set; }

    public virtual DbSet<Veicolo> Veicolos { get; set; }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Cliente>(entity =>
        {
            entity.HasKey(e => e.ClienteId).HasName("PK__Cliente__71ABD0A790113097");

            entity.ToTable("Cliente");

            entity.HasIndex(e => e.CodicePub, "UQ__Cliente__36AB492682F8AA60").IsUnique();

            entity.HasIndex(e => e.Email, "UQ__Cliente__A9D105348E04AAFF").IsUnique();

            entity.Property(e => e.ClienteId).HasColumnName("ClienteID");
            entity.Property(e => e.CodicePub)
                .HasMaxLength(36)
                .IsUnicode(false)
                .HasDefaultValueSql("(newid())")
                .IsFixedLength();
            entity.Property(e => e.Cognome)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Email)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.Indirizzo)
                .HasMaxLength(500)
                .IsUnicode(false);
            entity.Property(e => e.Nome)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Telefono)
                .HasMaxLength(30)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Intervento>(entity =>
        {
            entity.HasKey(e => e.InterventoId).HasName("PK__Interven__3B0A01964ECABB9D");

            entity.ToTable("Intervento");

            entity.HasIndex(e => e.CodicePub, "UQ__Interven__36AB4926AAAAC07F").IsUnique();

            entity.Property(e => e.InterventoId).HasColumnName("InterventoID");
            entity.Property(e => e.CodicePub)
                .HasMaxLength(36)
                .IsUnicode(false)
                .HasDefaultValueSql("(newid())")
                .IsFixedLength();
            entity.Property(e => e.DataFine).HasColumnType("datetime");
            entity.Property(e => e.DataIngresso)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Prezzo).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.Stato)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.VeicoloId).HasColumnName("VeicoloID");

            entity.HasOne(d => d.Veicolo).WithMany(p => p.Interventos)
                .HasForeignKey(d => d.VeicoloId)
                .HasConstraintName("FK_Intervento_Veicolo");
        });

        modelBuilder.Entity<Utente>(entity =>
        {
            entity.HasKey(e => e.UtenteId).HasName("PK__Utente__489EA72ABC96110A");

            entity.ToTable("Utente");

            entity.HasIndex(e => e.CodicePub, "UQ__Utente__36AB49268B749E3C").IsUnique();

            entity.HasIndex(e => e.Username, "UQ__Utente__536C85E421D5D630").IsUnique();

            entity.Property(e => e.UtenteId).HasColumnName("UtenteID");
            entity.Property(e => e.CodicePub)
                .HasMaxLength(36)
                .IsUnicode(false)
                .HasDefaultValueSql("(newid())")
                .IsFixedLength();
            entity.Property(e => e.PasswordHash).IsUnicode(false);
            entity.Property(e => e.Ruolo)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Username)
                .HasMaxLength(100)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Veicolo>(entity =>
        {
            entity.HasKey(e => e.VeicoloId).HasName("PK__Veicolo__93D07E1489734F75");

            entity.ToTable("Veicolo");

            entity.HasIndex(e => e.CodicePub, "UQ__Veicolo__36AB4926F425219A").IsUnique();

            entity.HasIndex(e => e.Targa, "UQ__Veicolo__6C5E0D6BE111919D").IsUnique();

            entity.Property(e => e.VeicoloId).HasColumnName("VeicoloID");
            entity.Property(e => e.ClienteId).HasColumnName("ClienteID");
            entity.Property(e => e.CodicePub)
                .HasMaxLength(36)
                .IsUnicode(false)
                .HasDefaultValueSql("(newid())")
                .IsFixedLength();
            entity.Property(e => e.Marca)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Modello)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Targa)
                .HasMaxLength(20)
                .IsUnicode(false);

            entity.HasOne(d => d.Cliente).WithMany(p => p.Veicolos)
                .HasForeignKey(d => d.ClienteId)
                .HasConstraintName("FK_Veicolo_Cliente");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
