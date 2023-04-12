using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace SP23_G21_SHSMS.Models.SHSMS;

public partial class DbShsmsContext : DbContext
{
    public DbShsmsContext()
    {
    }

    public DbShsmsContext(DbContextOptions<DbShsmsContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Campus> Campuses { get; set; }

    public virtual DbSet<Category> Categories { get; set; }

    public virtual DbSet<Contact> Contacts { get; set; }

    public virtual DbSet<Disease> Diseases { get; set; }

    public virtual DbSet<DiseaseType> DiseaseTypes { get; set; }

    public virtual DbSet<Fe> Fes { get; set; }

    public virtual DbSet<Manufacture> Manufactures { get; set; }

    public virtual DbSet<Medicine> Medicines { get; set; }

    public virtual DbSet<Permission> Permissions { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<Unit> Units { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("server = (local); database = DB_SHSMS;uid=sa;pwd=123456;TrustServerCertificate=True");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Campus>(entity =>
        {
            entity.ToTable("campuses");

            entity.Property(e => e.CampusId).HasColumnName("campusID");
            entity.Property(e => e.CampusName)
                .HasMaxLength(150)
                .HasColumnName("campusName");
            entity.Property(e => e.ConnectionString)
                .HasMaxLength(4000)
                .HasColumnName("connectionString");
            entity.Property(e => e.EduId).HasColumnName("eduID");
            entity.Property(e => e.Status).HasColumnName("status");

            entity.HasOne(d => d.Edu).WithMany(p => p.Campuses)
                .HasForeignKey(d => d.EduId)
                .HasConstraintName("FK_campuses_fes");

            entity.HasMany(d => d.Users).WithMany(p => p.Campuses)
                .UsingEntity<Dictionary<string, object>>(
                    "UsersCampus",
                    r => r.HasOne<User>().WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK_users_campuses_users"),
                    l => l.HasOne<Campus>().WithMany()
                        .HasForeignKey("CampusId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK_users_campuses_campuses"),
                    j =>
                    {
                        j.HasKey("CampusId", "UserId").HasName("PK__users_ca__3C83130D2BDD4D39");
                        j.ToTable("users_campuses");
                        j.IndexerProperty<int>("CampusId").HasColumnName("campusID");
                        j.IndexerProperty<int>("UserId").HasColumnName("userID");
                    });
        });

        modelBuilder.Entity<Category>(entity =>
        {
            entity.ToTable("categories");

            entity.Property(e => e.CategoryId).HasColumnName("categoryID");
            entity.Property(e => e.CategoryName)
                .HasMaxLength(120)
                .HasColumnName("categoryName");
        });

        modelBuilder.Entity<Contact>(entity =>
        {
            entity.HasKey(e => e.ContactId).HasName("PK_Contacts");

            entity.ToTable("contacts");

            entity.Property(e => e.ContactId).HasColumnName("contactID");
            entity.Property(e => e.CampusId).HasColumnName("campusID");
            entity.Property(e => e.Phone)
                .HasMaxLength(50)
                .IsFixedLength()
                .HasColumnName("phone");

            entity.HasOne(d => d.Campus).WithMany(p => p.Contacts)
                .HasForeignKey(d => d.CampusId)
                .HasConstraintName("FK_Contacts_campuses");
        });

        modelBuilder.Entity<Disease>(entity =>
        {
            entity.HasKey(e => e.DiseasesId);

            entity.ToTable("diseases");

            entity.Property(e => e.DiseasesId).HasColumnName("diseasesID");
            entity.Property(e => e.DiseasesName)
                .HasMaxLength(200)
                .HasColumnName("diseasesName");
            entity.Property(e => e.TypeId).HasColumnName("typeID");

            entity.HasOne(d => d.Type).WithMany(p => p.Diseases)
                .HasForeignKey(d => d.TypeId)
                .HasConstraintName("FK_diseases_diseaseTypes");
        });

        modelBuilder.Entity<DiseaseType>(entity =>
        {
            entity.HasKey(e => e.TypeId);

            entity.ToTable("diseaseTypes");

            entity.Property(e => e.TypeId).HasColumnName("typeID");
            entity.Property(e => e.TypeName)
                .HasMaxLength(200)
                .HasColumnName("typeName");
        });

        modelBuilder.Entity<Fe>(entity =>
        {
            entity.HasKey(e => e.EduId);

            entity.ToTable("fes");

            entity.Property(e => e.EduId).HasColumnName("eduID");
            entity.Property(e => e.EduName)
                .HasMaxLength(150)
                .HasColumnName("eduName");
            entity.Property(e => e.Status).HasColumnName("status");
        });

        modelBuilder.Entity<Manufacture>(entity =>
        {
            entity.ToTable("manufactures");

            entity.Property(e => e.ManufactureId).HasColumnName("manufactureID");
            entity.Property(e => e.ManufactureName)
                .HasMaxLength(120)
                .HasColumnName("manufactureName");
            entity.Property(e => e.Status).HasColumnName("status");
        });

        modelBuilder.Entity<Medicine>(entity =>
        {
            entity.ToTable("medicines");

            entity.Property(e => e.MedicineId)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("medicineID");
            entity.Property(e => e.CategoryId).HasColumnName("categoryID");
            entity.Property(e => e.Direction)
                .HasColumnType("ntext")
                .HasColumnName("direction");
            entity.Property(e => e.IsFree).HasColumnName("isFree");
            entity.Property(e => e.ManufactureId).HasColumnName("manufactureID");
            entity.Property(e => e.MedicineName)
                .HasMaxLength(200)
                .HasColumnName("medicineName");
            entity.Property(e => e.Status).HasColumnName("status");
            entity.Property(e => e.UnitId).HasColumnName("unitID");

            entity.HasOne(d => d.Category).WithMany(p => p.Medicines)
                .HasForeignKey(d => d.CategoryId)
                .HasConstraintName("FK_medicines_categories");

            entity.HasOne(d => d.Manufacture).WithMany(p => p.Medicines)
                .HasForeignKey(d => d.ManufactureId)
                .HasConstraintName("FK_medicines_manufactures");

            entity.HasOne(d => d.Unit).WithMany(p => p.Medicines)
                .HasForeignKey(d => d.UnitId)
                .HasConstraintName("FK_medicines_units");
        });

        modelBuilder.Entity<Permission>(entity =>
        {
            entity.HasKey(e => e.PermissionsId);

            entity.ToTable("permissions");

            entity.Property(e => e.PermissionsId).HasColumnName("permissionsID");
            entity.Property(e => e.PermissionName)
                .HasMaxLength(120)
                .HasColumnName("permissionName");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.ToTable("roles");

            entity.Property(e => e.RoleId).HasColumnName("roleID");
            entity.Property(e => e.RoleName)
                .HasMaxLength(120)
                .HasColumnName("roleName");

            entity.HasMany(d => d.Permissions).WithMany(p => p.Roles)
                .UsingEntity<Dictionary<string, object>>(
                    "RolesPermission",
                    r => r.HasOne<Permission>().WithMany()
                        .HasForeignKey("PermissionId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK_users_permissions_permissions"),
                    l => l.HasOne<Role>().WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK_roles_permissions_roles"),
                    j =>
                    {
                        j.HasKey("RoleId", "PermissionId").HasName("PK__roles_pe__101A551D7D5CD6DB");
                        j.ToTable("roles_permissions");
                        j.IndexerProperty<int>("RoleId").HasColumnName("roleID");
                        j.IndexerProperty<int>("PermissionId").HasColumnName("permissionID");
                    });
        });

        modelBuilder.Entity<Unit>(entity =>
        {
            entity.ToTable("units");

            entity.Property(e => e.UnitId).HasColumnName("unitID");
            entity.Property(e => e.UnitName)
                .HasMaxLength(120)
                .HasColumnName("unitName");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.ToTable("users");

            entity.Property(e => e.UserId).HasColumnName("userID");
            entity.Property(e => e.Email)
                .HasMaxLength(150)
                .HasColumnName("email");
            entity.Property(e => e.Phone)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("phone");
            entity.Property(e => e.RoleId).HasColumnName("roleID");
            entity.Property(e => e.Status).HasColumnName("status");
            entity.Property(e => e.Supervisor).HasColumnName("supervisor");
            entity.Property(e => e.UserName)
                .HasMaxLength(150)
                .HasColumnName("userName");

            entity.HasOne(d => d.Role).WithMany(p => p.Users)
                .HasForeignKey(d => d.RoleId)
                .HasConstraintName("FK_users_roles");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
