using SP23_G21_SHSMS.Models.Campuses;

namespace SP23_G21_SHSMS.Services.Campus_Services.TreatmentServices
{
    public interface ITreatmentSv
    {
        IEnumerable<Treatment> GetTreatments();
        Treatment FindTreatmentById(int treatmentId);
    }
}
