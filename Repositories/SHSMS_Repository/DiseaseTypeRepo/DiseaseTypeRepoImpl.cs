using SP23_G21_SHSMS.Models.SHSMS;

namespace SP23_G21_SHSMS.Repositories.SHSMS_Repository.DiseaseTypeRepo
{
	public class DiseaseTypeRepoImpl : IDiseaseTypeRepo
	{
		private readonly DbShsmsContext _context;
		public DiseaseTypeRepoImpl(DbShsmsContext context)
		{
			_context = context;
		}
		public DiseaseType GetDiseaseTypeById(int id)
		{
			return _context.DiseaseTypes.Find(id);
		}

		public IEnumerable<DiseaseType> GetDiseaseTypes()
		{
			return _context.DiseaseTypes.ToList();
		}
	}
}
