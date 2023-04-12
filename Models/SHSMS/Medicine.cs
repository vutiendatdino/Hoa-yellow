using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SP23_G21_SHSMS.Models.SHSMS;

public partial class Medicine
{
    [Display(Name = "Mã thuốc")]
    public string MedicineId { get; set; } = null!;
    [Display(Name = "Tên thuốc")]
    public string? MedicineName { get; set; }
    [Display(Name = "Mã NSX")]
    public int? ManufactureId { get; set; }
    [Display(Name = "Mã đơn vị")]
    public int? UnitId { get; set; }
    [Display(Name = "Mã danh mục")]
    public int? CategoryId { get; set; }
    [Display(Name = "HDSD")]
    public string? Direction { get; set; }
    [Display(Name = "Miễn phí")]
    public bool? IsFree { get; set; }
    [Display(Name = "Trạng thái")]
    public bool? Status { get; set; }

    public virtual Category? Category { get; set; }

    public virtual Manufacture? Manufacture { get; set; }

    public virtual Unit? Unit { get; set; }
}
