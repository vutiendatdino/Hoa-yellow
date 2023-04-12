using SP23_G21_SHSMS.Models.SHSMS;

namespace SP23_G21_SHSMS.Services.SHSMS_Services.CampusServices
{
    public interface ICampusSv
    {
        IEnumerable<Campus> GetCampuses();
    }
}
