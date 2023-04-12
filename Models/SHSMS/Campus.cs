using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SP23_G21_SHSMS.Models.SHSMS;

public partial class Campus
{
    public int CampusId { get; set; }

    public int? EduId { get; set; }

    [Display(Name = "Tên cơ sở")]
    public string? CampusName { get; set; }

    public string? ConnectionString { get; set; }

    public bool? Status { get; set; }
    [JsonIgnore]
    public virtual ICollection<Contact> Contacts { get; } = new List<Contact>();

    public virtual Fe? Edu { get; set; }
    [JsonIgnore]
    public virtual ICollection<User> Users { get; } = new List<User>();
}
