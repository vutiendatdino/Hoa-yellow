using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SP23_G21_SHSMS.Models.Campuses;

public partial class Consignment
{
    public int ConsignmentId { get; set; }

    [Display(Name = "Số lô")]
    [Required(ErrorMessage = "Không được để trống")]
    public string? ConsignmentNumber { get; set; }

    [Display(Name = "Thuốc")]
    [Required(ErrorMessage = "Không được để trống")]
    public string? MedicineId { get; set; }

    [Display(Name = "Ngày sản xuất")]
    [DataType(DataType.Date)]
    [Required(ErrorMessage = "Không được để trống")]
    public DateTime? ManufactureDate { get; set; }

    [Display(Name = "Ngày sản xuất")]
    [DataType(DataType.Date)]
    [Required(ErrorMessage = "Không được để trống")]
    public DateTime? ExpiredDate { get; set; }

    [Display(Name = "Số lượng")]
    [Required(ErrorMessage = "Không được để trống")]
    public int? Quantity { get; set; }

    [Display(Name = "Giá nhập (VND)")]
    [Required(ErrorMessage = "Không được để trống")]
    [DisplayFormat(DataFormatString = "{0:0,0}", ApplyFormatInEditMode = true)]
    public float? Price { get; set; }

    [JsonIgnore]
    public virtual ICollection<Storage> Storages { get; } = new List<Storage>();
    [JsonIgnore]
    public virtual ICollection<Import> Imports { get; } = new List<Import>();
}
