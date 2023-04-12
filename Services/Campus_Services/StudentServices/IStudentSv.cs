using SP23_G21_SHSMS.Models.Campuses;
using SP23_G21_SHSMS.Models.SHSMS;

namespace SP23_G21_SHSMS.Services.Campus_Services.StudentServices
{
    public interface IStudentSv
    {
        IEnumerable<Student> GetStudents();
        Student FindStudentByRollNumber(string rollNumber);
    }
}
