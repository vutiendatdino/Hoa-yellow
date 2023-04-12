using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace SP23_G21_SHSMS.Models.Campuses;

public partial class ExportType
{
    public int ExportTypeId { get; set; }

    public string? ExportTypeName { get; set; }

    [JsonIgnore]
    public virtual ICollection<Export> Exports { get; } = new List<Export>();
}
