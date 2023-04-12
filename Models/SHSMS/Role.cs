using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace SP23_G21_SHSMS.Models.SHSMS;

public partial class Role
{
    public int RoleId { get; set; }

    public string? RoleName { get; set; }
    [JsonIgnore]
    public virtual ICollection<User> Users { get; } = new List<User>();
    [JsonIgnore]
    public virtual ICollection<Permission> Permissions { get; } = new List<Permission>();
}
