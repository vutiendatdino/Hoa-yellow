using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SP23_G21_SHSMS.Models.SHSMS;

public partial class Disease
{
    public int DiseasesId { get; set; }

    public int? TypeId { get; set; }
    [Display(Name = "Bệnh")]
    public string? DiseasesName { get; set; }

    public virtual DiseaseType? Type { get; set; }
}
