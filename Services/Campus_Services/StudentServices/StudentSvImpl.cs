using SP23_G21_SHSMS.Models.Campuses;
using SP23_G21_SHSMS.Models.SHSMS;
using SP23_G21_SHSMS.Repositories.Campus_Repository.StudentRepo;

namespace SP23_G21_SHSMS.Services.Campus_Services.StudentServices
{
    public class StudentSvImpl : IStudentSv
    {
        private DbCampusContext _context;
        private IStudentRepo _studentRepo;
        public StudentSvImpl(DbCampusContext context)
        {
            _context = context;
            _studentRepo = new StudentRepoImpl(_context);
        }
        public Student FindStudentByRollNumber(string rollNumber)
        {
            if (string.IsNullOrEmpty(rollNumber)) return null;
            return _studentRepo.GetStudentByRollNumber(rollNumber);
        }

        public IEnumerable<Student> GetStudents()
        {
            //if (_studentRepo == null) yield break;
            if (_studentRepo == null) return null;
            var students = _studentRepo.GetStudents();
            if (students is null || students.Count() == 0)
                return null;
            return students;
        }
    }
}
