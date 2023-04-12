using SP23_G21_SHSMS.Models.SHSMS;

namespace SP23_G21_SHSMS.Services.SHSMS_Services.ClinicHeadUserServices
{
    public interface IClinicHeadUserSv
    {
        bool Login(string email, int campusID);
        User? GetUserByEmail(string email);
        IEnumerable<User> GetUsers();
    }
}
