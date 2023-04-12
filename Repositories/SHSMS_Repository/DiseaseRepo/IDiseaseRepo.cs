using SP23_G21_SHSMS.Models.SHSMS;

namespace SP23_G21_SHSMS.Repositories.SHSMS_Repository.DiseaseRepo
{
    public interface IDiseaseRepo
    {
        IEnumerable<Disease> GetDiseases();
        IEnumerable<Disease> GetDiseasesByType(int diseaseTypeId);
    }
}
