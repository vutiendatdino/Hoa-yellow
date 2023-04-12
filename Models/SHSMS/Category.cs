using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace SP23_G21_SHSMS.Models.SHSMS;

public partial class Category
{
    public int CategoryId { get; set; }

    public string? CategoryName { get; set; }
    [JsonIgnore]
    public virtual ICollection<Medicine> Medicines { get; } = new List<Medicine>();
}
