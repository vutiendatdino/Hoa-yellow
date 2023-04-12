using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SP23_G21_SHSMS.Models.SHSMS;

public partial class Unit
{
    [Display(Name = "Mã đơn vị")]
    public int UnitId { get; set; }
    [Display(Name = "Tên đơn vị")]
    public string? UnitName { get; set; }
    [JsonIgnore]
    public virtual ICollection<Medicine> Medicines { get; } = new List<Medicine>();
}
