using System;
using System.Collections.Generic;

namespace SP23_G21_SHSMS.Models.Campuses;

public partial class MedicalHistory
{
    public int MedicalHistoryId { get; set; }

    public string? RollNumber { get; set; }

    public string? ParentIllness { get; set; }

    public string? CurrentIllness { get; set; }

    public string? MedicalInUse { get; set; }

    public string? DrugAllergies { get; set; }

    public virtual Student? RollNumberNavigation { get; set; }
}
