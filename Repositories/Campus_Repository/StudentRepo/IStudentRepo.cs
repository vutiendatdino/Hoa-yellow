using SP23_G21_SHSMS.Models.Campuses;
using SP23_G21_SHSMS.Models.SHSMS;

namespace SP23_G21_SHSMS.Repositories.Campus_Repository.StudentRepo
{
    public interface IStudentRepo
    {
        IEnumerable<Student> GetStudents();
        Student GetStudentByRollNumber(string rollNumber);
    }
}
