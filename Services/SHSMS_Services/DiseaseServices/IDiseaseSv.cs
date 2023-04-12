using SP23_G21_SHSMS.Models.SHSMS;

namespace SP23_G21_SHSMS.Services.SHSMS_Services.DiseaseServices
{
    public interface IDiseaseSv
    {
        IEnumerable<Disease> GetDiseases();
    }
}
