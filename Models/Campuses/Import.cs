using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SP23_G21_SHSMS.Models.Campuses;

public partial class Import
{
    public int ImportId { get; set; }

    public int? UserId { get; set; }
    [Display(Name = "Ngày nhập")]
    [DataType(DataType.Date)]
    public DateTime? ImportDate { get; set; }

    [JsonIgnore]
    public virtual ICollection<Consignment> Consignments { get; } = new List<Consignment>();
}
