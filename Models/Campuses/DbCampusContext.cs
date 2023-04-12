using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace SP23_G21_SHSMS.Models.Campuses;

public partial class DbCampusContext : DbContext
{
    public DbCampusContext()
    {
    }

    public DbCampusContext(DbContextOptions<DbCampusContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Consignment> Consignments { get; set; }

    public virtual DbSet<Export> Exports { get; set; }

    public virtual DbSet<ExportDetail> ExportDetails { get; set; }

    public virtual DbSet<ExportType> ExportTypes { get; set; }

    public virtual DbSet<Import> Imports { get; set; }

    public virtual DbSet<MedicalHistory> MedicalHistories { get; set; }

    public virtual DbSet<MedicalRecord> MedicalRecords { get; set; }

    public virtual DbSet<Storage> Storages { get; set; }

    public virtual DbSet<Student> Students { get; set; }

    public virtual DbSet<Symptom> Symptoms { get; set; }

    public virtual DbSet<Treatment> Treatments { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("server =(local); database = DB_Campus;uid=sa;pwd=123456;TrustServerCertificate=True");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Consignment>(entity =>
        {
            entity.HasKey(e => e.ConsignmentId).HasName("PK__consignm__A2E2B547604939FF");

            entity.ToTable("consignments");

            entity.Property(e => e.ConsignmentId).HasColumnName("consignmentID");
            entity.Property(e => e.ConsignmentNumber)
                .HasMaxLength(120)
                .HasColumnName("consignmentNumber");
            entity.Property(e => e.ExpiredDate)
                .HasColumnType("datetime")
                .HasColumnName("expiredDate");
            entity.Property(e => e.ManufactureDate)
                .HasColumnType("datetime")
                .HasColumnName("manufactureDate");
            entity.Property(e => e.MedicineId)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("medicineID");
            entity.Property(e => e.Price).HasColumnName("price");
            entity.Property(e => e.Quantity).HasColumnName("quantity");
        });

        modelBuilder.Entity<Export>(entity =>
        {
            entity.HasKey(e => e.ExportId).HasName("PK__exports__4D92A951E5C06C40");

            entity.ToTable("exports");

            entity.Property(e => e.ExportId).HasColumnName("exportID");
            entity.Property(e => e.Amount).HasColumnName("amount");
            entity.Property(e => e.ExportDate)
                .HasColumnType("datetime")
                .HasColumnName("exportDate");
            entity.Property(e => e.ExportTypeId).HasColumnName("exportTypeID");
            entity.Property(e => e.MedicalRecordsId).HasColumnName("medicalRecordsID");
            entity.Property(e => e.Note)
                .HasColumnType("ntext")
                .HasColumnName("note");
            entity.Property(e => e.Status).HasColumnName("status");
            entity.Property(e => e.UserId).HasColumnName("userID");

            entity.HasOne(d => d.ExportType).WithMany(p => p.Exports)
                .HasForeignKey(d => d.ExportTypeId)
                .HasConstraintName("FK__exports__exportT__3F466844");

            entity.HasOne(d => d.MedicalRecords).WithMany(p => p.Exports)
                .HasForeignKey(d => d.MedicalRecordsId)
                .HasConstraintName("FK__exports__medical__3E52440B");
        });

        modelBuilder.Entity<ExportDetail>(entity =>
        {
            entity.HasKey(e => new { e.ExportId, e.StorageId }).HasName("PK__exportDe__F0869E99BC375E99");

            entity.ToTable("exportDetails");

            entity.Property(e => e.ExportId).HasColumnName("exportID");
            entity.Property(e => e.StorageId).HasColumnName("storageID");
            entity.Property(e => e.Direction)
                .HasColumnType("ntext")
                .HasColumnName("direction");
            entity.Property(e => e.Price).HasColumnName("price");
            entity.Property(e => e.Quantity).HasColumnName("quantity");

            entity.HasOne(d => d.Export).WithMany(p => p.ExportDetails)
                .HasForeignKey(d => d.ExportId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__exportDet__expor__4222D4EF");

            entity.HasOne(d => d.Storage).WithMany(p => p.ExportDetails)
                .HasForeignKey(d => d.StorageId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__exportDet__stora__4316F928");
        });

        modelBuilder.Entity<ExportType>(entity =>
        {
            entity.HasKey(e => e.ExportTypeId).HasName("PK__exportTy__AC4E2ABD0D386DC3");

            entity.ToTable("exportType");

            entity.Property(e => e.ExportTypeId).HasColumnName("exportTypeID");
            entity.Property(e => e.ExportTypeName)
                .HasMaxLength(200)
                .HasColumnName("exportTypeName");
        });

        modelBuilder.Entity<Import>(entity =>
        {
            entity.HasKey(e => e.ImportId).HasName("PK__imports__2CC5AB073D0D710E");

            entity.ToTable("imports");

            entity.Property(e => e.ImportId).HasColumnName("importID");
            entity.Property(e => e.ImportDate)
                .HasColumnType("datetime")
                .HasColumnName("importDate");
            entity.Property(e => e.UserId).HasColumnName("userID");

            entity.HasMany(d => d.Consignments).WithMany(p => p.Imports)
                .UsingEntity<Dictionary<string, object>>(
                    "ImportDetail",
                    r => r.HasOne<Consignment>().WithMany()
                        .HasForeignKey("ConsignmentId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK__importDet__consi__30F848ED"),
                    l => l.HasOne<Import>().WithMany()
                        .HasForeignKey("ImportId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK__importDet__impor__300424B4"),
                    j =>
                    {
                        j.HasKey("ImportId", "ConsignmentId").HasName("PK__importDe__46EB8053F7E31A51");
                        j.ToTable("importDetails");
                        j.IndexerProperty<int>("ImportId").HasColumnName("importID");
                        j.IndexerProperty<int>("ConsignmentId").HasColumnName("consignmentID");
                    });
        });

        modelBuilder.Entity<MedicalHistory>(entity =>
        {
            entity.HasKey(e => e.MedicalHistoryId).HasName("PK__medicalH__39392EE05A692ECC");

            entity.ToTable("medicalHistory");

            entity.Property(e => e.MedicalHistoryId).HasColumnName("medicalHistoryID");
            entity.Property(e => e.CurrentIllness)
                .HasMaxLength(500)
                .HasColumnName("currentIllness");
            entity.Property(e => e.DrugAllergies)
                .HasMaxLength(500)
                .HasColumnName("drugAllergies");
            entity.Property(e => e.MedicalInUse)
                .HasMaxLength(500)
                .HasColumnName("medicalInUse");
            entity.Property(e => e.ParentIllness)
                .HasMaxLength(500)
                .HasColumnName("parentIllness");
            entity.Property(e => e.RollNumber)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("rollNumber");

            entity.HasOne(d => d.RollNumberNavigation).WithMany(p => p.MedicalHistories)
                .HasForeignKey(d => d.RollNumber)
                .HasConstraintName("FK__medicalHi__rollN__45F365D3");
        });

        modelBuilder.Entity<MedicalRecord>(entity =>
        {
            entity.HasKey(e => e.MedicalRecordsId).HasName("PK__medicalR__CAFFE7F144868B3B");

            entity.ToTable("medicalRecords");

            entity.Property(e => e.MedicalRecordsId).HasColumnName("medicalRecordsID");
            entity.Property(e => e.BodyTemperature).HasColumnName("bodyTemperature");
            entity.Property(e => e.Dia).HasColumnName("dia");
            entity.Property(e => e.DiseaseId).HasColumnName("diseaseID");
            entity.Property(e => e.DiseaseNote)
                .HasMaxLength(500)
                .HasColumnName("diseaseNote");
            entity.Property(e => e.ExaminationTime)
                .HasColumnType("datetime")
                .HasColumnName("examinationTime");
            entity.Property(e => e.HeartRate).HasColumnName("heartRate");
            entity.Property(e => e.Note)
                .HasColumnType("ntext")
                .HasColumnName("note");
            entity.Property(e => e.RollNumber)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("rollNumber");
            entity.Property(e => e.Spo2).HasColumnName("spo2");
            entity.Property(e => e.Status).HasColumnName("status");
            entity.Property(e => e.SymptomNote)
                .HasMaxLength(500)
                .HasColumnName("symptomNote");
            entity.Property(e => e.Sys).HasColumnName("sys");
            entity.Property(e => e.TreatmentId).HasColumnName("treatmentID");
            entity.Property(e => e.TreatmentNote)
                .HasMaxLength(500)
                .HasColumnName("treatmentNote");
            entity.Property(e => e.UserId).HasColumnName("userID");

            entity.HasOne(d => d.RollNumberNavigation).WithMany(p => p.MedicalRecords)
                .HasForeignKey(d => d.RollNumber)
                .HasConstraintName("FK__medicalRe__rollN__36B12243");

            entity.HasOne(d => d.Treatment).WithMany(p => p.MedicalRecords)
                .HasForeignKey(d => d.TreatmentId)
                .HasConstraintName("FK__medicalRe__treat__37A5467C");
        });

        modelBuilder.Entity<Storage>(entity =>
        {
            entity.HasKey(e => e.StorageId).HasName("PK__storages__D1437C8ABDAF7064");

            entity.ToTable("storages");

            entity.Property(e => e.StorageId).HasColumnName("storageID");
            entity.Property(e => e.ConsignmentId).HasColumnName("consignmentID");
            entity.Property(e => e.MedicineId)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("medicineID");
            entity.Property(e => e.Price).HasColumnName("price");
            entity.Property(e => e.Quantity).HasColumnName("quantity");

            entity.HasOne(d => d.Consignment).WithMany(p => p.Storages)
                .HasForeignKey(d => d.ConsignmentId)
                .HasConstraintName("FK__storages__consig__33D4B598");
        });

        modelBuilder.Entity<Student>(entity =>
        {
            entity.HasKey(e => e.RollNumber).HasName("PK__students__251EFDE08BF10684");

            entity.ToTable("students");

            entity.Property(e => e.RollNumber)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("rollNumber");
            entity.Property(e => e.CampusId).HasColumnName("campusID");
            entity.Property(e => e.Dob)
                .HasColumnType("date")
                .HasColumnName("dob");
            entity.Property(e => e.HealthInsurance)
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasColumnName("healthInsurance");
            entity.Property(e => e.Identification)
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasColumnName("identification");
            entity.Property(e => e.ParentPhone)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("parentPhone");
            entity.Property(e => e.Phone)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("phone");
            entity.Property(e => e.StudentAddress)
                .HasMaxLength(500)
                .HasColumnName("studentAddress");
            entity.Property(e => e.StudentEmail)
                .HasMaxLength(150)
                .HasColumnName("studentEmail");
            entity.Property(e => e.StudentName)
                .HasMaxLength(150)
                .HasColumnName("studentName");
        });

        modelBuilder.Entity<Symptom>(entity =>
        {
            entity.HasKey(e => e.SymptomId).HasName("PK__symptoms__9A6E1DC31D8BC039");

            entity.ToTable("symptoms");

            entity.Property(e => e.SymptomId).HasColumnName("symptomID");
            entity.Property(e => e.SymptomName)
                .HasMaxLength(200)
                .HasColumnName("symptomName");

            entity.HasMany(d => d.MedicalRecords).WithMany(p => p.Symptoms)
                .UsingEntity<Dictionary<string, object>>(
                    "SymptomsMedicalRecord",
                    r => r.HasOne<MedicalRecord>().WithMany()
                        .HasForeignKey("MedicalRecordsId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK__symptoms___medic__3B75D760"),
                    l => l.HasOne<Symptom>().WithMany()
                        .HasForeignKey("SymptomId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK__symptoms___sympt__3A81B327"),
                    j =>
                    {
                        j.HasKey("SymptomId", "MedicalRecordsId").HasName("PK__symptoms__96C1E3BC3EF4EA2C");
                        j.ToTable("symptoms_medicalRecords");
                        j.IndexerProperty<int>("SymptomId").HasColumnName("symptomID");
                        j.IndexerProperty<int>("MedicalRecordsId").HasColumnName("medicalRecordsID");
                    });
        });

        modelBuilder.Entity<Treatment>(entity =>
        {
            entity.HasKey(e => e.TreatmentId).HasName("PK__treatmen__D7AA588800996FB7");

            entity.ToTable("treatments");

            entity.Property(e => e.TreatmentId).HasColumnName("treatmentID");
            entity.Property(e => e.TreatmentName)
                .HasMaxLength(200)
                .HasColumnName("treatmentName");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
