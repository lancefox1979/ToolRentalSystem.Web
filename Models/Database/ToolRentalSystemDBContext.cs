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

        public virtual DbSet<AspNetRoleClaims> AspNetRoleClaims { get; set; }
        public virtual DbSet<AspNetRoles> AspNetRoles { get; set; }
        public virtual DbSet<AspNetUserClaims> AspNetUserClaims { get; set; }
        public virtual DbSet<AspNetUserLogins> AspNetUserLogins { get; set; }
        public virtual DbSet<AspNetUserRoles> AspNetUserRoles { get; set; }
        public virtual DbSet<AspNetUsers> AspNetUsers { get; set; }
        public virtual DbSet<AspNetUserTokens> AspNetUserTokens { get; set; }
        public virtual DbSet<EfmigrationsHistory> EfmigrationsHistory { get; set; }
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
                optionsBuilder.UseMySql("server=localhost;user=SA_ToolRentalSystem;password=admin;database=ToolRentalSystemDB");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AspNetRoleClaims>(entity =>
            {
                entity.HasIndex(e => e.RoleId);

                entity.Property(e => e.Id).HasColumnType("int(11)");

                entity.Property(e => e.ClaimType).HasColumnType("longtext");

                entity.Property(e => e.ClaimValue).HasColumnType("longtext");

                entity.Property(e => e.RoleId)
                    .IsRequired()
                    .HasColumnType("varchar(255)");

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.AspNetRoleClaims)
                    .HasForeignKey(d => d.RoleId);
            });

            modelBuilder.Entity<AspNetRoles>(entity =>
            {
                entity.HasIndex(e => e.NormalizedName)
                    .HasName("RoleNameIndex")
                    .IsUnique();

                entity.Property(e => e.Id).HasColumnType("varchar(255)");

                entity.Property(e => e.ConcurrencyStamp).HasColumnType("longtext");

                entity.Property(e => e.Name).HasColumnType("varchar(256)");

                entity.Property(e => e.NormalizedName).HasColumnType("varchar(256)");
            });

            modelBuilder.Entity<AspNetUserClaims>(entity =>
            {
                entity.HasIndex(e => e.UserId);

                entity.Property(e => e.Id).HasColumnType("int(11)");

                entity.Property(e => e.ClaimType).HasColumnType("longtext");

                entity.Property(e => e.ClaimValue).HasColumnType("longtext");

                entity.Property(e => e.UserId)
                    .IsRequired()
                    .HasColumnType("varchar(255)");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.AspNetUserClaims)
                    .HasForeignKey(d => d.UserId);
            });

            modelBuilder.Entity<AspNetUserLogins>(entity =>
            {
                entity.HasKey(e => new { e.LoginProvider, e.ProviderKey });

                entity.HasIndex(e => e.UserId);

                entity.Property(e => e.LoginProvider).HasColumnType("varchar(128)");

                entity.Property(e => e.ProviderKey).HasColumnType("varchar(128)");

                entity.Property(e => e.ProviderDisplayName).HasColumnType("longtext");

                entity.Property(e => e.UserId)
                    .IsRequired()
                    .HasColumnType("varchar(255)");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.AspNetUserLogins)
                    .HasForeignKey(d => d.UserId);
            });

            modelBuilder.Entity<AspNetUserRoles>(entity =>
            {
                entity.HasKey(e => new { e.UserId, e.RoleId });

                entity.HasIndex(e => e.RoleId);

                entity.Property(e => e.UserId).HasColumnType("varchar(255)");

                entity.Property(e => e.RoleId).HasColumnType("varchar(255)");

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.AspNetUserRoles)
                    .HasForeignKey(d => d.RoleId);

                entity.HasOne(d => d.User)
                    .WithMany(p => p.AspNetUserRoles)
                    .HasForeignKey(d => d.UserId);
            });

            modelBuilder.Entity<AspNetUsers>(entity =>
            {
                entity.HasIndex(e => e.NormalizedEmail)
                    .HasName("EmailIndex");

                entity.HasIndex(e => e.NormalizedUserName)
                    .HasName("UserNameIndex")
                    .IsUnique();

                entity.Property(e => e.Id).HasColumnType("varchar(255)");

                entity.Property(e => e.AccessFailedCount).HasColumnType("int(11)");

                entity.Property(e => e.ConcurrencyStamp).HasColumnType("longtext");

                entity.Property(e => e.Email).HasColumnType("varchar(256)");

                entity.Property(e => e.EmailConfirmed).HasColumnType("bit(1)");

                entity.Property(e => e.LockoutEnabled).HasColumnType("bit(1)");

                entity.Property(e => e.NormalizedEmail).HasColumnType("varchar(256)");

                entity.Property(e => e.NormalizedUserName).HasColumnType("varchar(256)");

                entity.Property(e => e.PasswordHash).HasColumnType("longtext");

                entity.Property(e => e.PhoneNumber).HasColumnType("longtext");

                entity.Property(e => e.PhoneNumberConfirmed).HasColumnType("bit(1)");

                entity.Property(e => e.SecurityStamp).HasColumnType("longtext");

                entity.Property(e => e.TwoFactorEnabled).HasColumnType("bit(1)");

                entity.Property(e => e.UserName).HasColumnType("varchar(256)");
            });

            modelBuilder.Entity<AspNetUserTokens>(entity =>
            {
                entity.HasKey(e => new { e.UserId, e.LoginProvider, e.Name });

                entity.Property(e => e.UserId).HasColumnType("varchar(255)");

                entity.Property(e => e.LoginProvider).HasColumnType("varchar(128)");

                entity.Property(e => e.Name).HasColumnType("varchar(128)");

                entity.Property(e => e.Value).HasColumnType("longtext");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.AspNetUserTokens)
                    .HasForeignKey(d => d.UserId);
            });

            modelBuilder.Entity<EfmigrationsHistory>(entity =>
            {
                entity.HasKey(e => e.MigrationId);

                entity.ToTable("__EFMigrationsHistory");

                entity.Property(e => e.MigrationId).HasColumnType("varchar(95)");

                entity.Property(e => e.ProductVersion)
                    .IsRequired()
                    .HasColumnType("varchar(32)");
            });

            modelBuilder.Entity<Login>(entity =>
            {
                entity.HasKey(e => e.UserId);

                entity.ToTable("login");

                entity.HasIndex(e => e.UserId)
                    .HasName("IDX_user_id");

                entity.Property(e => e.UserId)
                    .HasColumnName("user_id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Password)
                    .HasColumnName("password")
                    .HasColumnType("varchar(50)");

                entity.Property(e => e.Username)
                    .HasColumnName("username")
                    .HasColumnType("varchar(50)");

                entity.HasOne(d => d.User)
                    .WithOne(p => p.Login)
                    .HasForeignKey<Login>(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("login_ibfk_1");
            });

            modelBuilder.Entity<Rented>(entity =>
            {
                entity.HasKey(e => new { e.UserId, e.ToolId });

                entity.ToTable("rented");

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
                entity.ToTable("tool");

                entity.HasIndex(e => e.ToolConditionId)
                    .HasName("tool_condition_id");

                entity.HasIndex(e => e.ToolDetailId)
                    .HasName("tool_detail_id");

                entity.HasIndex(e => e.ToolId)
                    .HasName("IDX_tool_id");

                entity.Property(e => e.ToolId)
                    .HasColumnName("tool_id")
                    .HasColumnType("int(11)");

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
                entity.ToTable("tool_classification");

                entity.HasIndex(e => e.ToolClassificationId)
                    .HasName("IDX_tool_classification_id");

                entity.Property(e => e.ToolClassificationId)
                    .HasColumnName("tool_classification_id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.ToolClassification1)
                    .HasColumnName("tool_classification")
                    .HasColumnType("varchar(50)");
            });

            modelBuilder.Entity<ToolCondition>(entity =>
            {
                entity.ToTable("tool_condition");

                entity.HasIndex(e => e.ToolConditionId)
                    .HasName("IDX_tool_condition_id");

                entity.Property(e => e.ToolConditionId)
                    .HasColumnName("tool_condition_id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.ToolCondition1)
                    .HasColumnName("tool_condition")
                    .HasColumnType("varchar(50)");
            });

            modelBuilder.Entity<ToolDetail>(entity =>
            {
                entity.ToTable("tool_detail");

                entity.HasIndex(e => e.ToolClassificationId)
                    .HasName("tool_classification_id");

                entity.HasIndex(e => e.ToolDetailId)
                    .HasName("IDX_tool_detail_id");

                entity.Property(e => e.ToolDetailId)
                    .HasColumnName("tool_detail_id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.ToolBrand)
                    .HasColumnName("tool_brand")
                    .HasColumnType("varchar(50)");

                entity.Property(e => e.ToolClassificationId)
                    .HasColumnName("tool_classification_id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.TradeName)
                    .HasColumnName("trade_name")
                    .HasColumnType("varchar(50)");

                entity.HasOne(d => d.ToolClassification)
                    .WithMany(p => p.ToolDetail)
                    .HasForeignKey(d => d.ToolClassificationId)
                    .HasConstraintName("tool_detail_ibfk_1");
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("user");

                entity.HasIndex(e => e.UserId)
                    .HasName("IDX_user_id");

                entity.HasIndex(e => e.UserTypeId)
                    .HasName("user_type_id");

                entity.Property(e => e.UserId)
                    .HasColumnName("user_id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Address)
                    .HasColumnName("address")
                    .HasColumnType("varchar(50)");

                entity.Property(e => e.Email)
                    .HasColumnName("email")
                    .HasColumnType("varchar(50)");

                entity.Property(e => e.FirstName)
                    .HasColumnName("first_name")
                    .HasColumnType("varchar(50)");

                entity.Property(e => e.LastName)
                    .HasColumnName("last_name")
                    .HasColumnType("varchar(50)");

                entity.Property(e => e.PhoneNumber)
                    .HasColumnName("phone_number")
                    .HasColumnType("varchar(50)");

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
                entity.ToTable("user_type");

                entity.HasIndex(e => e.UserTypeId)
                    .HasName("IDX_user_type_id");

                entity.Property(e => e.UserTypeId)
                    .HasColumnName("user_type_id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.UserType1)
                    .HasColumnName("user_type")
                    .HasColumnType("varchar(50)");
            });
        }
    }
}
