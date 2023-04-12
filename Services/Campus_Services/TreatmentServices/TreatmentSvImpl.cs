using SP23_G21_SHSMS.Models.Campuses;
using SP23_G21_SHSMS.Repositories.Campus_Repository.SymptomRepo;
using SP23_G21_SHSMS.Repositories.Campus_Repository.TreatmentRepo;

namespace SP23_G21_SHSMS.Services.Campus_Services.TreatmentServices
{
    public class TreatmentSvImpl : ITreatmentSv
    {
        private DbCampusContext _context;
        private ITreatmentRepo _treatmentRepo;
        public TreatmentSvImpl(DbCampusContext context)
        {
            _context = context;
            _treatmentRepo = new TreatmentRepoImpl(_context);
        }
        public Treatment FindTreatmentById(int treatmentId)
        {
            try
            {
                if (_context.Treatments == null) return null;
                return _treatmentRepo.GetTreatmentById(treatmentId);
            }
            catch
            {
                return null;
            }
        }

        public IEnumerable<Treatment> GetTreatments()
        {
            try
            {
                if (_context.Treatments == null) return null;
                var treatments =  _treatmentRepo.GetTreatments();
                if (treatments is null || !treatments.Any() || treatments.Count() == 0) return null;
                return treatments;
            }
            catch
            {
                return null;
            }
        }
    }
}
