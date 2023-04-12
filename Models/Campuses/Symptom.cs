using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace SP23_G21_SHSMS.Models.Campuses;

public partial class Symptom
{
    public int SymptomId { get; set; }

    public string? SymptomName { get; set; }

    [JsonIgnore]
    public virtual ICollection<MedicalRecord> MedicalRecords { get; } = new List<MedicalRecord>();
}
