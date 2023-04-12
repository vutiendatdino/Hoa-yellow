using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace SP23_G21_SHSMS.Models.Campuses;

public partial class Treatment
{
    public int TreatmentId { get; set; }

    public string? TreatmentName { get; set; }

    [JsonIgnore]
    public virtual ICollection<MedicalRecord> MedicalRecords { get; } = new List<MedicalRecord>();
}
