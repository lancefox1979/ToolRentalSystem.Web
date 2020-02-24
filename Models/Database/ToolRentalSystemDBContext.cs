using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace ToolRentalSystem.Web.Models.Database
{
    public partial class ToolRentalSystemDBContext : DbContext
    {
        public ToolRentalSystemDBContext()
        {
        }

        public ToolRentalSystemDBContext(DbContextOptions<ToolRentalSystemDBContext> options) : base(options)
        {
        }

        public virtual DbSet<Login> Login { get; set; }
        public virtual DbSet<Rented> Rented { get; set; }
        public virtual DbSet<Tool> Tool { get; set; }
        public virtual DbSet<ToolClassification> ToolClassification { get; set; }
        public virtual DbSet<ToolCondition> ToolCondition { get; set; }
        public virtual DbSet<ToolDetail> ToolDetail { get; set; }
        public virtual DbSet<User> User { get; set; }
        public virtual DbSet<UserType> UserType { get; set; }
        
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseMySQL("server=localhost;user=SA_ToolRentalSystem;password=admin;database=ToolRentalSystemDB");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Login>(entity =>
            {
                entity.HasKey(e => e.UserId);

                entity.ToTable("login", "ToolRentalSystemDB");

                entity.HasIndex(e => e.UserId)
                    .HasName("IDX_user_id");

                entity.Property(e => e.UserId)
                    .HasColumnName("user_id")
                    .HasColumnType("int(11)")
                    .ValueGeneratedNever();

                entity.Property(e => e.Password)
                    .HasColumnName("password")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Username)
                    .HasColumnName("username")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.HasOne(d => d.User)
                    .WithOne(p => p.Login)
                    .HasForeignKey<Login>(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("login_ibfk_1");
            });

            modelBuilder.Entity<Rented>(entity =>
            {
                entity.HasKey(e => new { e.UserId, e.ToolId });

                entity.ToTable("rented", "ToolRentalSystemDB");

                entity.HasIndex(e => e.ToolId)
                    .HasName("tool_id");

                entity.HasIndex(e => new { e.UserId, e.ToolId })
                    .HasName("IDX_user_id");

                entity.Property(e => e.UserId)
                    .HasColumnName("user_id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.ToolId)
                    .HasColumnName("tool_id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.DueDate)
                    .HasColumnName("due_date")
                    .HasColumnType("date");

                entity.Property(e => e.RentalStatus)
                    .HasColumnName("rental_status")
                    .HasColumnType("int(11)");

                entity.Property(e => e.StartDate)
                    .HasColumnName("start_date")
                    .HasColumnType("date");

                entity.HasOne(d => d.Tool)
                    .WithMany(p => p.Rented)
                    .HasForeignKey(d => d.ToolId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("rented_ibfk_2");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Rented)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("rented_ibfk_1");
            });

            modelBuilder.Entity<Tool>(entity =>
            {
                entity.ToTable("tool", "ToolRentalSystemDB");

                entity.HasIndex(e => e.ToolConditionId)
                    .HasName("tool_condition_id");

                entity.HasIndex(e => e.ToolDetailId)
                    .HasName("tool_detail_id");

                entity.HasIndex(e => e.ToolId)
                    .HasName("IDX_tool_id");

                entity.Property(e => e.ToolId)
                    .HasColumnName("tool_id")
                    .HasColumnType("int(11)")
                    .ValueGeneratedNever();

                entity.Property(e => e.ToolConditionId)
                    .HasColumnName("tool_condition_id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.ToolDetailId)
                    .HasColumnName("tool_detail_id")
                    .HasColumnType("int(11)");

                entity.HasOne(d => d.ToolCondition)
                    .WithMany(p => p.Tool)
                    .HasForeignKey(d => d.ToolConditionId)
                    .HasConstraintName("tool_ibfk_2");

                entity.HasOne(d => d.ToolDetail)
                    .WithMany(p => p.Tool)
                    .HasForeignKey(d => d.ToolDetailId)
                    .HasConstraintName("tool_ibfk_1");
            });

            modelBuilder.Entity<ToolClassification>(entity =>
            {
                entity.ToTable("tool_classification", "ToolRentalSystemDB");

                entity.HasIndex(e => e.ToolClassificationId)
                    .HasName("IDX_tool_classification_id");

                entity.Property(e => e.ToolClassificationId)
                    .HasColumnName("tool_classification_id")
                    .HasColumnType("int(11)")
                    .ValueGeneratedNever();

                entity.Property(e => e.ToolClassification1)
                    .HasColumnName("tool_classification")
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<ToolCondition>(entity =>
            {
                entity.ToTable("tool_condition", "ToolRentalSystemDB");

                entity.HasIndex(e => e.ToolConditionId)
                    .HasName("IDX_tool_condition_id");

                entity.Property(e => e.ToolConditionId)
                    .HasColumnName("tool_condition_id")
                    .HasColumnType("int(11)")
                    .ValueGeneratedNever();

                entity.Property(e => e.ToolCondition1)
                    .HasColumnName("tool_condition")
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<ToolDetail>(entity =>
            {
                entity.ToTable("tool_detail", "ToolRentalSystemDB");

                entity.HasIndex(e => e.ToolClassificationId)
                    .HasName("tool_classification_id");

                entity.HasIndex(e => e.ToolDetailId)
                    .HasName("IDX_tool_detail_id");

                entity.Property(e => e.ToolDetailId)
                    .HasColumnName("tool_detail_id")
                    .HasColumnType("int(11)")
                    .ValueGeneratedNever();

                entity.Property(e => e.ToolBrand)
                    .HasColumnName("tool_brand")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.ToolClassificationId)
                    .HasColumnName("tool_classification_id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.TradeName)
                    .HasColumnName("trade_name")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.HasOne(d => d.ToolClassification)
                    .WithMany(p => p.ToolDetail)
                    .HasForeignKey(d => d.ToolClassificationId)
                    .HasConstraintName("tool_detail_ibfk_1");
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("user", "ToolRentalSystemDB");

                entity.HasIndex(e => e.UserId)
                    .HasName("IDX_user_id");

                entity.HasIndex(e => e.UserTypeId)
                    .HasName("user_type_id");

                entity.Property(e => e.UserId)
                    .HasColumnName("user_id")
                    .HasColumnType("int(11)")
                    .ValueGeneratedNever();

                entity.Property(e => e.Address)
                    .HasColumnName("address")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Email)
                    .HasColumnName("email")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.FirstName)
                    .HasColumnName("first_name")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.LastName)
                    .HasColumnName("last_name")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.PhoneNumber)
                    .HasColumnName("phone_number")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.UserTypeId)
                    .HasColumnName("user_type_id")
                    .HasColumnType("int(11)");

                entity.HasOne(d => d.UserType)
                    .WithMany(p => p.User)
                    .HasForeignKey(d => d.UserTypeId)
                    .HasConstraintName("user_ibfk_1");
            });

            modelBuilder.Entity<UserType>(entity =>
            {
                entity.ToTable("user_type", "ToolRentalSystemDB");

                entity.HasIndex(e => e.UserTypeId)
                    .HasName("IDX_user_type_id");

                entity.Property(e => e.UserTypeId)
                    .HasColumnName("user_type_id")
                    .HasColumnType("int(11)")
                    .ValueGeneratedNever();

                entity.Property(e => e.UserType1)
                    .HasColumnName("user_type")
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });
        }
    }
}
