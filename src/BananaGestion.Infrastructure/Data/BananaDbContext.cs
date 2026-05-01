using BananaGestion.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace BananaGestion.Infrastructure.Data;

public class BananaDbContext : DbContext
{
    public BananaDbContext(DbContextOptions<BananaDbContext> options) : base(options) { }

    public DbSet<User> Users => Set<User>();
    public DbSet<Lote> Lotes => Set<Lote>();
    public DbSet<TaskConfig> TaskConfigs => Set<TaskConfig>();
    public DbSet<TaskAssignment> TaskAssignments => Set<TaskAssignment>();
    public DbSet<TaskLog> TaskLogs => Set<TaskLog>();
    public DbSet<Product> Products => Set<Product>();
    public DbSet<InventoryMovement> InventoryMovements => Set<InventoryMovement>();
    public DbSet<HarvestCalendar> HarvestCalendars => Set<HarvestCalendar>();
    public DbSet<HarvestRecord> HarvestRecords => Set<HarvestRecord>();
    public DbSet<HarvestCosecha> HarvestCosechas => Set<HarvestCosecha>();
    public DbSet<BoxType> BoxTypes => Set<BoxType>();
    public DbSet<HarvestBoxRecord> HarvestBoxRecords => Set<HarvestBoxRecord>();
    public DbSet<HarvestOrder> HarvestOrders => Set<HarvestOrder>();
    public DbSet<OrderBoxDetail> OrderBoxDetails => Set<OrderBoxDetail>();
    public DbSet<FinancialTransaction> FinancialTransactions => Set<FinancialTransaction>();
    public DbSet<Notification> Notifications => Set<Notification>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<User>(entity =>
        {
            entity.ToTable("users");
            entity.HasKey(e => e.Id);
            entity.HasIndex(e => e.Email).IsUnique();
            entity.Property(e => e.Email).IsRequired().HasMaxLength(256);
            entity.Property(e => e.PasswordHash).IsRequired();
            entity.Property(e => e.Nombre).IsRequired().HasMaxLength(100);
            entity.Property(e => e.Apellido).IsRequired().HasMaxLength(100);
            entity.Property(e => e.Telefono).HasMaxLength(20);
        });

        modelBuilder.Entity<Lote>(entity =>
        {
            entity.ToTable("lotes");
            entity.HasKey(e => e.Id);
            entity.HasIndex(e => e.Codigo).IsUnique();
            entity.Property(e => e.Codigo).IsRequired().HasMaxLength(50);
            entity.Property(e => e.Nombre).IsRequired().HasMaxLength(200);
            entity.Property(e => e.Hectareas).HasPrecision(10, 4);
        });

        modelBuilder.Entity<TaskConfig>(entity =>
        {
            entity.ToTable("task_configs");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Nombre).IsRequired().HasMaxLength(200);
            entity.Property(e => e.InsumoCantidad).HasPrecision(10, 2);
            entity.HasOne(e => e.Insumo)
                  .WithMany()
                  .HasForeignKey(e => e.InsumoId)
                  .OnDelete(DeleteBehavior.SetNull);
        });

        modelBuilder.Entity<TaskAssignment>(entity =>
        {
            entity.ToTable("task_assignments");
            entity.HasKey(e => e.Id);
            entity.HasOne(e => e.TaskConfig)
                  .WithMany(t => t.TaskAssignments)
                  .HasForeignKey(e => e.TaskConfigId)
                  .OnDelete(DeleteBehavior.Restrict);
            entity.HasOne(e => e.User)
                  .WithMany(u => u.TaskAssignments)
                  .HasForeignKey(e => e.UserId)
                  .OnDelete(DeleteBehavior.Restrict);
            entity.HasOne(e => e.Lote)
                  .WithMany(l => l.TaskAssignments)
                  .HasForeignKey(e => e.LoteId)
                  .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<TaskLog>(entity =>
        {
            entity.ToTable("task_logs");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Latitud).HasPrecision(10, 8);
            entity.Property(e => e.Longitud).HasPrecision(11, 8);
            entity.Property(e => e.CantidadInsumoUtilizado).HasPrecision(10, 2);
            entity.HasOne(e => e.TaskAssignment)
                  .WithOne(t => t.TaskLog)
                  .HasForeignKey<TaskLog>(e => e.TaskAssignmentId)
                  .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<Product>(entity =>
        {
            entity.ToTable("products");
            entity.HasKey(e => e.Id);
            entity.HasIndex(e => e.Codigo).IsUnique();
            entity.Property(e => e.Codigo).IsRequired().HasMaxLength(50);
            entity.Property(e => e.Nombre).IsRequired().HasMaxLength(200);
            entity.Property(e => e.Unidad).IsRequired().HasMaxLength(50);
            entity.Property(e => e.StockActual).HasPrecision(10, 2);
            entity.Property(e => e.StockMinimo).HasPrecision(10, 2);
        });

        modelBuilder.Entity<InventoryMovement>(entity =>
        {
            entity.ToTable("inventory_movements");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Cantidad).HasPrecision(10, 2);
            entity.Property(e => e.StockAnterior).HasPrecision(10, 2);
            entity.Property(e => e.StockNuevo).HasPrecision(10, 2);
            entity.Property(e => e.Tipo).IsRequired().HasMaxLength(20);
            entity.HasOne(e => e.Product)
                  .WithMany(p => p.InventoryMovements)
                  .HasForeignKey(e => e.ProductId)
                  .OnDelete(DeleteBehavior.Restrict);
            entity.HasOne(e => e.Lote)
                  .WithMany(l => l.InventoryMovements)
                  .HasForeignKey(e => e.LoteId)
                  .OnDelete(DeleteBehavior.SetNull);
            entity.HasOne(e => e.User)
                  .WithMany()
                  .HasForeignKey(e => e.UserId)
                  .OnDelete(DeleteBehavior.SetNull);
        });

        modelBuilder.Entity<HarvestCalendar>(entity =>
        {
            entity.ToTable("harvest_calendars");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.ColorCinta).IsRequired().HasMaxLength(50);
            entity.Property(e => e.ColorNombre).HasMaxLength(50).HasColumnName("color_nombre");
            entity.HasIndex(e => new { e.Semana, e.Ano }).IsUnique();
        });

        modelBuilder.Entity<HarvestRecord>(entity =>
        {
            entity.ToTable("harvest_records");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.ColorCinta).IsRequired().HasMaxLength(50);
            entity.Property(e => e.SemanaEncinte).HasColumnName("semana_encinte");
            entity.Property(e => e.AnoEncinte).HasColumnName("ano_encinte");
            entity.HasOne(e => e.HarvestCalendar)
                  .WithMany(h => h.HarvestRecords)
                  .HasForeignKey(e => e.HarvestCalendarId)
                  .OnDelete(DeleteBehavior.SetNull);
            entity.HasOne(e => e.Lote)
                  .WithMany(l => l.HarvestRecords)
                  .HasForeignKey(e => e.LoteId)
                  .OnDelete(DeleteBehavior.Restrict);
            entity.HasOne(e => e.User)
                  .WithMany(u => u.HarvestRecords)
                  .HasForeignKey(e => e.UserId)
                  .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<HarvestCosecha>(entity =>
        {
            entity.ToTable("harvest_cosechas");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.ColorCinta).IsRequired().HasMaxLength(50);
            entity.Property(e => e.Estado).HasConversion<int>();
            entity.HasOne(e => e.HarvestCalendar)
                  .WithMany()
                  .HasForeignKey(e => e.HarvestCalendarId)
                  .OnDelete(DeleteBehavior.SetNull);
            entity.HasOne(e => e.Lote)
                  .WithMany()
                  .HasForeignKey(e => e.LoteId)
                  .OnDelete(DeleteBehavior.Restrict);
            entity.HasOne(e => e.User)
                  .WithMany()
                  .HasForeignKey(e => e.UserId)
                  .OnDelete(DeleteBehavior.SetNull);
        });

        modelBuilder.Entity<BoxType>(entity =>
        {
            entity.ToTable("box_types");
            entity.HasKey(e => e.Id);
            entity.HasIndex(e => e.Codigo).IsUnique();
            entity.Property(e => e.Codigo).IsRequired().HasMaxLength(50);
            entity.Property(e => e.Descripcion).HasMaxLength(500);
        });

        modelBuilder.Entity<HarvestBoxRecord>(entity =>
        {
            entity.ToTable("harvest_box_records");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.PesoTotal).HasPrecision(10, 2);
            entity.HasOne(e => e.HarvestRecord)
                  .WithMany(h => h.BoxRecords)
                  .HasForeignKey(e => e.HarvestRecordId)
                  .OnDelete(DeleteBehavior.Restrict);
            entity.HasOne(e => e.BoxType)
                  .WithMany(b => b.HarvestBoxRecords)
                  .HasForeignKey(e => e.BoxTypeId)
                  .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<HarvestOrder>(entity =>
        {
            entity.ToTable("harvest_orders");
            entity.HasKey(e => e.Id);
            entity.HasIndex(e => e.NumeroOrden).IsUnique();
            entity.Property(e => e.NumeroOrden).IsRequired().HasMaxLength(100);
            entity.Property(e => e.Cliente).HasMaxLength(200);
        });

        modelBuilder.Entity<OrderBoxDetail>(entity =>
        {
            entity.ToTable("order_box_details");
            entity.HasKey(e => e.Id);
            entity.HasOne(e => e.HarvestOrder)
                  .WithMany(h => h.BoxDetails)
                  .HasForeignKey(e => e.HarvestOrderId)
                  .OnDelete(DeleteBehavior.Restrict);
            entity.HasOne(e => e.BoxType)
                  .WithMany()
                  .HasForeignKey(e => e.BoxTypeId)
                  .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<FinancialTransaction>(entity =>
        {
            entity.ToTable("financial_transactions");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Tipo).IsRequired().HasMaxLength(20);
            entity.Property(e => e.Categoria).IsRequired().HasMaxLength(100);
            entity.Property(e => e.Monto).HasPrecision(12, 2);
            entity.Property(e => e.Descripcion).HasMaxLength(500);
        });

        modelBuilder.Entity<Notification>(entity =>
        {
            entity.ToTable("notifications");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Titulo).IsRequired().HasMaxLength(200);
            entity.Property(e => e.Mensaje).IsRequired();
            entity.Property(e => e.Tipo).IsRequired().HasMaxLength(50);
            entity.HasOne(e => e.User)
                  .WithMany()
                  .HasForeignKey(e => e.UserId)
                  .OnDelete(DeleteBehavior.Cascade);
        });
    }
}
