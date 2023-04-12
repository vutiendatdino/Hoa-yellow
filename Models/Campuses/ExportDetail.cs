using System;
using System.Collections.Generic;

namespace SP23_G21_SHSMS.Models.Campuses;

public partial class ExportDetail
{
    public int ExportId { get; set; }

    public int StorageId { get; set; }

    public int? Quantity { get; set; }

    public float? Price { get; set; }

    public string? Direction { get; set; }

    public virtual Export Export { get; set; } = null!;

    public virtual Storage Storage { get; set; } = null!;
}
