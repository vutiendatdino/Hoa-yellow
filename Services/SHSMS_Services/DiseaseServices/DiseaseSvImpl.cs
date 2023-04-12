using SP23_G21_SHSMS.Models.SHSMS;
using SP23_G21_SHSMS.Repositories.SHSMS_Repository.DiseaseRepo;

namespace SP23_G21_SHSMS.Services.SHSMS_Services.DiseaseServices
{
    public class DiseaseSvImpl : IDiseaseSv
    {
        private DbShsmsContext _context;
        private IDiseaseRepo _diseaseRepo;
        public DiseaseSvImpl(DbShsmsContext context)
        {
            _context = context;
            _diseaseRepo = new DiseaseRepoImpl(_context);
        }
        public IEnumerable<Disease> GetDiseases()
        {
            try
            {
                if (_context.Diseases == null) return null;
                var diseases = _diseaseRepo.GetDiseases();
                if (diseases is null || diseases.Count() == 0)
                    return null;
                return diseases;
            }
            catch
            {
                return null;
            }
        }
    }
}
