using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace SP23_G21_SHSMS.Models.Campuses;

public partial class Student
{
    public string RollNumber { get; set; } = null!;

    public int? CampusId { get; set; }

    public string? StudentEmail { get; set; }

    public string? StudentName { get; set; }

    public DateTime? Dob { get; set; }

    public string? HealthInsurance { get; set; }

    public string? Phone { get; set; }

    public string? StudentAddress { get; set; }

    public string? ParentPhone { get; set; }

    public string? Identification { get; set; }

    [JsonIgnore]
    public virtual ICollection<MedicalHistory> MedicalHistories { get; } = new List<MedicalHistory>();

    [JsonIgnore]
    public virtual ICollection<MedicalRecord> MedicalRecords { get; } = new List<MedicalRecord>();
}
