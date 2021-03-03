using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace StoreDL.Entities
{
    public partial class P0DatabaseContext : DbContext
    {
        public P0DatabaseContext()
        {
        }

        public P0DatabaseContext(DbContextOptions<P0DatabaseContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Car> Cars { get; set; }
        public virtual DbSet<Cart> Carts { get; set; }
        public virtual DbSet<Customer> Customers { get; set; }
        public virtual DbSet<Item> Items { get; set; }
        public virtual DbSet<LocationTable> LocationTables { get; set; }
        public virtual DbSet<Manager> Managers { get; set; }
        public virtual DbSet<OrderTable> OrderTables { get; set; }
        public virtual DbSet<Product> Products { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "SQL_Latin1_General_CP1_CI_AS");

            modelBuilder.Entity<Car>(entity =>
            {
                entity.ToTable("cars");

                entity.HasIndex(e => e.CarModel, "idx_AnyModels");

                entity.HasIndex(e => e.CarModel, "idx_models")
                    .IsUnique();

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.CarMake)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnName("carMake");

                entity.Property(e => e.CarModel)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnName("carModel");

                entity.Property(e => e.CarYear).HasColumnName("carYear");

                entity.Property(e => e.Price).HasColumnName("price");
            });

            modelBuilder.Entity<Cart>(entity =>
            {
                entity.ToTable("cart");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.CustomerId).HasColumnName("customer_id");

                entity.Property(e => e.ItemId).HasColumnName("item_id");

                entity.Property(e => e.LocationId).HasColumnName("location_id");

                entity.Property(e => e.Quantity).HasColumnName("quantity");

                entity.Property(e => e.Total).HasColumnName("total");

                entity.HasOne(d => d.Customer)
                    .WithMany(p => p.Carts)
                    .HasForeignKey(d => d.CustomerId)
                    .HasConstraintName("FK__cart__customer_i__15DA3E5D");

                entity.HasOne(d => d.Item)
                    .WithMany(p => p.Carts)
                    .HasForeignKey(d => d.ItemId)
                    .HasConstraintName("FK__cart__item_id__17C286CF");

                entity.HasOne(d => d.Location)
                    .WithMany(p => p.Carts)
                    .HasForeignKey(d => d.LocationId)
                    .HasConstraintName("FK__cart__location_i__16CE6296");
            });

            modelBuilder.Entity<Customer>(entity =>
            {
                entity.ToTable("customers");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.EmailAddress)
                    .HasMaxLength(100)
                    .HasColumnName("emailAddress");

                entity.Property(e => e.FirstName)
                    .HasMaxLength(100)
                    .HasColumnName("firstName");

                entity.Property(e => e.LastName)
                    .HasMaxLength(100)
                    .HasColumnName("lastName");
            });

            modelBuilder.Entity<Item>(entity =>
            {
                entity.ToTable("item");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.LocationId).HasColumnName("location_id");

                entity.Property(e => e.ProductId).HasColumnName("product_id");

                entity.Property(e => e.Quantity).HasColumnName("quantity");

                entity.HasOne(d => d.Location)
                    .WithMany(p => p.Items)
                    .HasForeignKey(d => d.LocationId)
                    .HasConstraintName("FK__item__location_i__1209AD79");

                entity.HasOne(d => d.Product)
                    .WithMany(p => p.Items)
                    .HasForeignKey(d => d.ProductId)
                    .HasConstraintName("FK__item__product_id__12FDD1B2");
            });

            modelBuilder.Entity<LocationTable>(entity =>
            {
                entity.ToTable("locationTable");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.LocationAddress)
                    .HasMaxLength(50)
                    .HasColumnName("locationAddress");

                entity.Property(e => e.LocationName)
                    .HasMaxLength(50)
                    .HasColumnName("locationName");
            });

            modelBuilder.Entity<Manager>(entity =>
            {
                entity.ToTable("managers");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.EmailAddress)
                    .IsRequired()
                    .HasMaxLength(100)
                    .HasColumnName("emailAddress");

                entity.Property(e => e.FirstName)
                    .IsRequired()
                    .HasMaxLength(100)
                    .HasColumnName("firstName");

                entity.Property(e => e.LastName)
                    .IsRequired()
                    .HasMaxLength(100)
                    .HasColumnName("lastName");
            });

            modelBuilder.Entity<OrderTable>(entity =>
            {
                entity.ToTable("orderTable");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.CustomerId).HasColumnName("customer_id");

                entity.Property(e => e.ItemId).HasColumnName("item_id");

                entity.Property(e => e.LocationId).HasColumnName("location_id");

                entity.Property(e => e.Quantity).HasColumnName("quantity");

                entity.Property(e => e.Total).HasColumnName("total");

                entity.HasOne(d => d.Customer)
                    .WithMany(p => p.OrderTables)
                    .HasForeignKey(d => d.CustomerId)
                    .HasConstraintName("FK__orderTabl__custo__1A9EF37A");

                entity.HasOne(d => d.Item)
                    .WithMany(p => p.OrderTables)
                    .HasForeignKey(d => d.ItemId)
                    .HasConstraintName("FK__orderTabl__item___1C873BEC");

                entity.HasOne(d => d.Location)
                    .WithMany(p => p.OrderTables)
                    .HasForeignKey(d => d.LocationId)
                    .HasConstraintName("FK__orderTabl__locat__1B9317B3");
            });

            modelBuilder.Entity<Product>(entity =>
            {
                entity.ToTable("product");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Category).HasColumnName("category");

                entity.Property(e => e.Price).HasColumnName("price");

                entity.Property(e => e.ProductName)
                    .HasMaxLength(50)
                    .HasColumnName("productName");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
