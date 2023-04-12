using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SP23_G21_SHSMS.Models.SHSMS;

public partial class Contact
{
    public int ContactId { get; set; }

    [Display(Name = "Tên cơ sở")]
    public int? CampusId { get; set; }

    [Display(Name = "Số điện thoại")]
    [RegularExpression(@"^(03|05|07|08|09|01[2|6|8|9]|1900)\s*\d{7,8}\s*$", ErrorMessage = "Hãy nhập số điện thoại hợp lệ.")]
    public string? Phone { get; set; }

    public virtual Campus? Campus { get; set; }
}
