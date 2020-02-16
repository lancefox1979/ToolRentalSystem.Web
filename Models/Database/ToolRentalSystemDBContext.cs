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

        public ToolRentalSystemDBContext(DbContextOptions<ToolRentalSystemDBContext> options)
            : base(options)
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

                entity.ToTable("Login", "ToolRentalSystemDB");

                entity.HasIndex(e => e.UserId)
                    .HasName("IDX_User_ID");

                entity.Property(e => e.UserId)
                    .HasColumnName("User_ID")
                    .HasColumnType("int(11)")
                    .ValueGeneratedNever();

                entity.Property(e => e.Password)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Username)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.HasOne(d => d.User)
                    .WithOne(p => p.Login)
                    .HasForeignKey<Login>(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("Login_ibfk_1");
            });

            modelBuilder.Entity<Rented>(entity =>
            {
                entity.HasKey(e => new { e.UserId, e.ToolId });

                entity.ToTable("Rented", "ToolRentalSystemDB");

                entity.HasIndex(e => e.ToolId)
                    .HasName("Tool_ID");

                entity.HasIndex(e => new { e.UserId, e.ToolId })
                    .HasName("IDX_User_ID");

                entity.Property(e => e.UserId)
                    .HasColumnName("User_ID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.ToolId)
                    .HasColumnName("Tool_ID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.DueDate)
                    .HasColumnName("Due_date")
                    .HasColumnType("date");

                entity.Property(e => e.RentalStatus)
                    .HasColumnName("Rental_status")
                    .HasColumnType("int(11)");

                entity.Property(e => e.StartDate)
                    .HasColumnName("Start_date")
                    .HasColumnType("date");

                entity.HasOne(d => d.Tool)
                    .WithMany(p => p.Rented)
                    .HasForeignKey(d => d.ToolId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("Rented_ibfk_2");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Rented)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("Rented_ibfk_1");
            });

            modelBuilder.Entity<Tool>(entity =>
            {
                entity.ToTable("Tool", "ToolRentalSystemDB");

                entity.HasIndex(e => e.ToolConditionId)
                    .HasName("Tool_condition_ID");

                entity.HasIndex(e => e.ToolDetailId)
                    .HasName("Tool_detail_ID");

                entity.HasIndex(e => e.ToolId)
                    .HasName("IDX_Tool_ID");

                entity.Property(e => e.ToolId)
                    .HasColumnName("Tool_ID")
                    .HasColumnType("int(11)")
                    .ValueGeneratedNever();

                entity.Property(e => e.ToolConditionId)
                    .HasColumnName("Tool_condition_ID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.ToolDetailId)
                    .HasColumnName("Tool_detail_ID")
                    .HasColumnType("int(11)");

                entity.HasOne(d => d.ToolCondition)
                    .WithMany(p => p.Tool)
                    .HasForeignKey(d => d.ToolConditionId)
                    .HasConstraintName("Tool_ibfk_2");

                entity.HasOne(d => d.ToolDetail)
                    .WithMany(p => p.Tool)
                    .HasForeignKey(d => d.ToolDetailId)
                    .HasConstraintName("Tool_ibfk_1");
            });

            modelBuilder.Entity<ToolClassification>(entity =>
            {
                entity.ToTable("Tool_classification", "ToolRentalSystemDB");

                entity.HasIndex(e => e.ToolClassificationId)
                    .HasName("IDX_Tool_classification_ID");

                entity.Property(e => e.ToolClassificationId)
                    .HasColumnName("Tool_classification_ID")
                    .HasColumnType("int(11)")
                    .ValueGeneratedNever();

                entity.Property(e => e.ToolClassification1)
                    .HasColumnName("Tool_classification")
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<ToolCondition>(entity =>
            {
                entity.ToTable("Tool_condition", "ToolRentalSystemDB");

                entity.HasIndex(e => e.ToolConditionId)
                    .HasName("IDX_Tool_condition_ID");

                entity.Property(e => e.ToolConditionId)
                    .HasColumnName("Tool_condition_ID")
                    .HasColumnType("int(11)")
                    .ValueGeneratedNever();

                entity.Property(e => e.ToolCondition1)
                    .HasColumnName("Tool_condition")
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<ToolDetail>(entity =>
            {
                entity.ToTable("Tool_detail", "ToolRentalSystemDB");

                entity.HasIndex(e => e.ToolDetailId)
                    .HasName("IDX_Tool_detail_ID");

                entity.Property(e => e.ToolDetailId)
                    .HasColumnName("Tool_detail_ID")
                    .HasColumnType("int(11)")
                    .ValueGeneratedNever();

                entity.Property(e => e.ToolBrand)
                    .HasColumnName("Tool_brand")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.ToolClassificationId)
                    .HasColumnName("Tool_classification_ID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.TradeName)
                    .HasColumnName("Trade_name")
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("User", "ToolRentalSystemDB");

                entity.HasIndex(e => e.UserId)
                    .HasName("IDX_User_ID");

                entity.HasIndex(e => e.UserTypeId)
                    .HasName("User_type_ID");

                entity.Property(e => e.UserId)
                    .HasColumnName("User_ID")
                    .HasColumnType("int(11)")
                    .ValueGeneratedNever();

                entity.Property(e => e.Address)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Email)
                    .HasColumnName("email")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.FirstName)
                    .HasColumnName("First_name")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.LastName)
                    .HasColumnName("Last_name")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.PhoneNumber)
                    .HasColumnName("Phone_number")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.UserTypeId)
                    .HasColumnName("User_type_ID")
                    .HasColumnType("int(11)");

                entity.HasOne(d => d.UserType)
                    .WithMany(p => p.User)
                    .HasForeignKey(d => d.UserTypeId)
                    .HasConstraintName("User_ibfk_1");
            });

            modelBuilder.Entity<UserType>(entity =>
            {
                entity.ToTable("User_type", "ToolRentalSystemDB");

                entity.HasIndex(e => e.UserTypeId)
                    .HasName("IDX_User_type_ID");

                entity.Property(e => e.UserTypeId)
                    .HasColumnName("User_type_ID")
                    .HasColumnType("int(11)")
                    .ValueGeneratedNever();

                entity.Property(e => e.UserTypeP)
                    .HasColumnName("User_type_P")
                    .HasColumnType("int(11)");
            });
        }
    }
}
