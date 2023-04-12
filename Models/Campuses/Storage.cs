using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace SP23_G21_SHSMS.Models.Campuses;

public partial class Storage
{
    public int StorageId { get; set; }

    public int? ConsignmentId { get; set; }

    public string? MedicineId { get; set; }

    public int? Quantity { get; set; }

    public float? Price { get; set; }

    public virtual Consignment? Consignment { get; set; }
    [JsonIgnore]
    public virtual ICollection<ExportDetail> ExportDetails { get; } = new List<ExportDetail>();
}
