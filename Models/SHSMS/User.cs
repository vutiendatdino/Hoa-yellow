using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SP23_G21_SHSMS.Models.SHSMS;

public partial class User
{
    [Display(Name = "Mã nhân viên")]
    public int UserId { get; set; }
    [Display(Name="Tên nhân viên")]
    public string? UserName { get; set; }
    [Display(Name = "Email")]
    public string? Email { get; set; }
    [Display(Name = "SĐT")]
    public string? Phone { get; set; }
    [Display(Name = "Người tạo")]
    public int? Supervisor { get; set; }
    [Display(Name = "Role")]
    public int? RoleId { get; set; }
    [Display(Name = "Trạng thái")]
    public bool? Status { get; set; }

    public virtual Role? Role { get; set; }
    [JsonIgnore]
    public virtual ICollection<Campus> Campuses { get; } = new List<Campus>();
}
