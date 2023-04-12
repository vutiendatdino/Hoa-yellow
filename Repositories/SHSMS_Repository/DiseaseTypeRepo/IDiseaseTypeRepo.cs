using SP23_G21_SHSMS.Models.SHSMS;

namespace SP23_G21_SHSMS.Repositories.SHSMS_Repository.DiseaseTypeRepo
{
	public interface IDiseaseTypeRepo
	{
		IEnumerable<DiseaseType> GetDiseaseTypes();
		DiseaseType GetDiseaseTypeById(int id);
	}
}
