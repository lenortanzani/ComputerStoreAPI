using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.Extensions.Configuration;

#nullable disable

/// <summary>
/// Класс отвечает за доступ к объектам DbSet, доступ к БД, не более, 
/// сформирован автоматически Entity Frameworke Core
/// </summary>
/// 
namespace ComputerStoreAPI
{
    public partial class ComputerStoreDbContext : DbContext
    {
        private readonly IConfiguration _configuration;

        public ComputerStoreDbContext(DbContextOptions<ComputerStoreDbContext> options, 
            IConfiguration configuration)
            : base(options)
        {
            _configuration = configuration;
        }

        public virtual DbSet<Item> Items { get; set; }
        public virtual DbSet<Order> Orders { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "Russian_Kazakhstan.1251");

            modelBuilder.Entity<Item>(entity =>
            {
                entity.ToTable("items");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasDefaultValueSql("nextval('seq_items'::regclass)");

                entity.Property(e => e.Characteristics)
                    .HasColumnType("character varying")
                    .HasColumnName("characteristics");

                entity.Property(e => e.Name)
                    .HasColumnType("character varying")
                    .HasColumnName("name");

                entity.Property(e => e.PictureUrl)
                    .HasColumnType("character varying")
                    .HasColumnName("picture_url");

                entity.Property(e => e.Price).HasColumnName("price");

                entity.Property(e => e.Type)
                    .HasColumnType("character varying")
                    .HasColumnName("type");
            });

            modelBuilder.Entity<Order>(entity =>
            {
                entity.ToTable("orders");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasDefaultValueSql("nextval('seq_orders'::regclass)");

                entity.Property(e => e.Address)
                    .HasColumnType("character varying")
                    .HasColumnName("address");

                entity.Property(e => e.CourierId)
                    .HasColumnType("character varying")
                    .HasColumnName("courier_id");

                entity.Property(e => e.Datetimeorderdelivered).HasColumnName("datetimeorderdelivered");

                entity.Property(e => e.Datetimeorderplaced).HasColumnName("datetimeorderplaced");

                entity.Property(e => e.ItemId).HasColumnName("item_id");

                entity.Property(e => e.Quantity).HasColumnName("quantity");

                entity.Property(e => e.UserId)
                    .IsRequired()
                    .HasColumnType("character varying")
                    .HasColumnName("user_id");

                entity.HasOne(d => d.Item)
                    .WithMany(p => p.Orders)
                    .HasForeignKey(d => d.ItemId)
                    .HasConstraintName("FK_item_id");
            });

            modelBuilder.HasSequence("seq_goods");

            modelBuilder.HasSequence("seq_items");

            modelBuilder.HasSequence("seq_orders");

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
