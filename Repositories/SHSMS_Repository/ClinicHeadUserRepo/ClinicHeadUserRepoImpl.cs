using Microsoft.EntityFrameworkCore;
using SP23_G21_SHSMS.Models.SHSMS;

namespace SP23_G21_SHSMS.Repositories.SHSMS_Repository.ClinicHeadUserRepo
{
    public class ClinicHeadUserRepoImpl : IClinicHeadUserRepo
    {
        private readonly DbShsmsContext _context;
        public ClinicHeadUserRepoImpl(DbShsmsContext context)
        {
            _context = context;
        }

        public void RemoveUser(User user)
        {
            var userTmp = _context.Users.SingleOrDefault(u => u.UserId == user.UserId);
            _context.Users.Remove(userTmp!);
            _context.SaveChanges();
        }

        public User GetUserByEmailAndCampusID(string email, int campusID)
        {
            //var user = _context.Users.Include(u => u.Campuses.Where(c => c.CampusId == campusID)).SingleOrDefault(user => user.Email.ToLower().Equals(email.ToLower()));
            //var user = _context.Users.SingleOrDefault(u => u.Email.ToLower().Equals(email.ToLower()) && u.Campuses.Any(c => c.CampusId == campusID));
            var user = _context.Users.Where(u => u.Email.ToLower().Equals(email.ToLower())).Include(c => c.Campuses.Where(c => c.CampusId == campusID)).FirstOrDefault();
            return user;
        }

        public User GetUserByUserID(int userID)
        {
            return _context.Users.SingleOrDefault(user => user.UserId == userID)!;
        }

        public List<User> GetUsers()
        {
            return _context.Users.ToList();
        }

        public List<User> GetUsersByCampusID(int campusID)
        {
            return _context.Users.Include(user => user.Campuses.Where(campus => campus.CampusId == campusID)).ToList();
        }

        public void SaveUser(User user)
        {
            _context.Users.Add(user);
            _context.SaveChanges();
        }

        public void UpdateUser(User user)
        {
            _context.Entry<User>(user).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            _context.SaveChanges();
        }

        public User GetUserByEmail(string email)
        {
            var user = _context.Users.Where(u => u.Email.ToLower().Equals(email.ToLower()))
                .Include(r => r.Role)
                    .ThenInclude(p => p.Permissions)
                .FirstOrDefault();
            return user;
            //return _context.Users.SingleOrDefault(user => user.Email!.ToLower().Equals(email.ToLower()));
        }
    }
}
