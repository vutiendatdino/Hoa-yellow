using SP23_G21_SHSMS.Models.SHSMS;

namespace SP23_G21_SHSMS.Repositories.SHSMS_Repository.UserRepo
{
    public interface IUserRepo
    {
        void SaveUser(User user);
        void UpdateUser(User user);
        void RemoveUser(User user);
        List<User> GetUsers();
        List<User> GetUsersByCampusID(int campusID);
        User GetUserByEmailAndCampusID(string email, int campusID);
        User GetUserByEmail(string email);
        User GetUserByUserID(int userID);
        bool IsUserExist(int id);
        bool IsEmailExist(string email);
    }
}
