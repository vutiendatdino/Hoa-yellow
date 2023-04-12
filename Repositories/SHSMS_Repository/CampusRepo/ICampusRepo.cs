using SP23_G21_SHSMS.Models.SHSMS;

namespace SP23_G21_SHSMS.Repositories.SHSMS_Repository.CampusRepo
{
    public interface ICampusRepo
    {
        void SaveCampus(Campus campus);
        void UpdateCampus(Campus campus);
        void RemoveCampus(Campus campus);
        IEnumerable<Campus> GetCampuses();
    }
}
