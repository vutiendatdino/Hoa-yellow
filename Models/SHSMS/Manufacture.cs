using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace SP23_G21_SHSMS.Models.SHSMS;

public partial class Manufacture
{
    public int ManufactureId { get; set; }

    public string? ManufactureName { get; set; }

    public bool? Status { get; set; }
    [JsonIgnore]
    public virtual ICollection<Medicine> Medicines { get; } = new List<Medicine>();
}
