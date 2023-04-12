using SP23_G21_SHSMS.Models.SHSMS;
using SP23_G21_SHSMS.Repositories.SHSMS_Repository.UserRepo;

namespace SP23_G21_SHSMS.Services.SHSMS_Services.UserServices
{
    public class UserSvImpl : IUserSv
    {
        private DbShsmsContext _context;
        private IUserRepo _userRepository;
        public UserSvImpl(DbShsmsContext context)
        {
            _context = context;
            _userRepository = new UserRepoImpl(_context);
        }
        public User? GetUserByEmail(string email)
        {
            if (string.IsNullOrEmpty(email)) return null;

            return _userRepository.GetUserByEmail(email);
        }

        public User GetUserById(int id)
        {
            try
            {
                if (_context.Users == null || !IsUserExist(id)) return null;
                return _userRepository.GetUserByUserID(id);
            }
            catch
            {
                return null;
            }
        }

        public IEnumerable<User> GetUsers()
        {
            return _userRepository.GetUsers();
        }

        public bool IsEmailExist(string email)
        {
            try
            {
                return _userRepository.IsEmailExist(email);
            }
            catch
            {
                return false;
            }
        }

        public bool IsUserExist(int id)
        {
            try
            {
                return _userRepository.IsUserExist(id);
            }
            catch
            {
                return false;
            }
        }

        //public User GetUserFromSession(string sessionName)
        //{
        //    ISession session = new ISession();
        //    //return HttpContext.Session.GetString("User");
        //}

        public bool Login(string email, int campusID)
        {
            if (string.IsNullOrEmpty(email)) return false;

            var user = _userRepository.GetUserByEmailAndCampusID(email, campusID);
            if (user == null)
                return false;
            return true;
        }

        public bool RemoveUser(User user)
        {
            try
            {
                if (_context.Users == null || user is null || !_userRepository.IsUserExist(user.UserId)) return false;
                _userRepository.RemoveUser(user);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool SaveUser(User user)
        {
            try
            {
                if (user == null) return false;
                _userRepository.SaveUser(user);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool UpdateUser(int id, User user)
        {
            if (id != user.UserId) return false;
            try
            {
                if (user is null) return false;
                _userRepository.UpdateUser(user);
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
