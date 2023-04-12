using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SP23_G21_SHSMS.Models.SHSMS;

public partial class DiseaseType
{
    public int TypeId { get; set; }
    [Display(Name = "Loại bệnh")]
    public string? TypeName { get; set; }
    [JsonIgnore]
    public virtual ICollection<Disease> Diseases { get; } = new List<Disease>();
}
