using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace SP23_G21_SHSMS.Models.Campuses;

public partial class MedicalRecord
{
    public int MedicalRecordsId { get; set; }

    public int? UserId { get; set; }

    public int? DiseaseId { get; set; }

    public string? RollNumber { get; set; }

    public int? TreatmentId { get; set; }

    public DateTime? ExaminationTime { get; set; }

    public int? Sys { get; set; }

    public int? Dia { get; set; }

    public double? BodyTemperature { get; set; }

    public int? HeartRate { get; set; }

    public int? Spo2 { get; set; }

    public string? SymptomNote { get; set; }

    public string? TreatmentNote { get; set; }

    public string? DiseaseNote { get; set; }

    public string? Note { get; set; }

    public bool? Status { get; set; }

    [JsonIgnore]
    public virtual ICollection<Export> Exports { get; } = new List<Export>();

    public virtual Student? RollNumberNavigation { get; set; }

    public virtual Treatment? Treatment { get; set; }

    [JsonIgnore]
    public virtual ICollection<Symptom> Symptoms { get; } = new List<Symptom>();
}
