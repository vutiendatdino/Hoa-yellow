using Microsoft.EntityFrameworkCore;
using SP23_G21_SHSMS.Models.Campuses;
using SP23_G21_SHSMS.Models.SHSMS;

namespace SP23_G21_SHSMS.Repositories.Campus_Repository.StudentRepo
{
    public class StudentRepoImpl : IStudentRepo
    {
        private DbCampusContext _context;
        public StudentRepoImpl(DbCampusContext context)
        {
            _context = context;
        }
        public Student GetStudentByRollNumber(string rollNumber)
        {
            return _context.Students
                .Include(s => s.MedicalRecords)
                .SingleOrDefault(s => s.RollNumber.ToLower().Equals(rollNumber.ToLower()));
        }

        public IEnumerable<Student> GetStudents()
        {
            return _context.Students.ToList();
        }
    }
}
