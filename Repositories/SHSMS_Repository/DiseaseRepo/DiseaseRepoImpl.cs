using SP23_G21_SHSMS.Models.SHSMS;

namespace SP23_G21_SHSMS.Repositories.SHSMS_Repository.DiseaseRepo
{
    public class DiseaseRepoImpl : IDiseaseRepo
    {
        private DbShsmsContext _context;
        public DiseaseRepoImpl(DbShsmsContext context)
        {
            _context = context;
        }
        public IEnumerable<Disease> GetDiseases()
        {
            return _context.Diseases.ToList();
        }

		public IEnumerable<Disease> GetDiseasesByType(int diseaseTypeId)
		{
            return _context.Diseases.Where(d => d.TypeId == diseaseTypeId).ToList();
		}
	}
}
