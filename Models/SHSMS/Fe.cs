using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace SP23_G21_SHSMS.Models.SHSMS;

public partial class Fe
{
    public int EduId { get; set; }

    public string? EduName { get; set; }

    public bool? Status { get; set; }
    [JsonIgnore]
    public virtual ICollection<Campus> Campuses { get; } = new List<Campus>();
}
