using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace SP23_G21_SHSMS.Models.SHSMS;

public partial class Permission
{
    public int PermissionsId { get; set; }

    public string? PermissionName { get; set; }
    [JsonIgnore]
    public virtual ICollection<Role> Roles { get; } = new List<Role>();
}
