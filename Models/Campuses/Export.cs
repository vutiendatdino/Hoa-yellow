using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace SP23_G21_SHSMS.Models.Campuses;

public partial class Export
{
    public int ExportId { get; set; }

    public int? UserId { get; set; }

    public int? MedicalRecordsId { get; set; }

    public float? Amount { get; set; }

    public DateTime? ExportDate { get; set; }

    public int? ExportTypeId { get; set; }

    public string? Note { get; set; }

    public bool? Status { get; set; }

    [JsonIgnore]
    public virtual ICollection<ExportDetail> ExportDetails { get; } = new List<ExportDetail>();

    public virtual ExportType? ExportType { get; set; }

    public virtual MedicalRecord? MedicalRecords { get; set; }
}
