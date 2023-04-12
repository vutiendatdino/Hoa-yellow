using SP23_G21_SHSMS.Models.SHSMS;

namespace SP23_G21_SHSMS.Services.SHSMS_Services.UserServices
{
    public interface IUserSv
    {
        bool Login(string email, int campusID);
        User? GetUserByEmail(string email);
        IEnumerable<User> GetUsers();
        //User GetUserFromSession(string sessionName);
        bool SaveUser(User user);
        bool RemoveUser(User user);
        bool UpdateUser(int id, User user);
        bool IsUserExist(int id);
        User GetUserById(int id);
        bool IsEmailExist(string email);
    }
}
