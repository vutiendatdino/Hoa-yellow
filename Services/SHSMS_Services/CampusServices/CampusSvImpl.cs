using SP23_G21_SHSMS.Models.SHSMS;
using SP23_G21_SHSMS.Repositories.SHSMS_Repository.CampusRepo;

namespace SP23_G21_SHSMS.Services.SHSMS_Services.CampusServices
{
    public class CampusSvImpl : ICampusSv
    {
        private DbShsmsContext _context;
        private ICampusRepo _campusRepository;
        public CampusSvImpl(DbShsmsContext context)
        {
            _context = context;
            _campusRepository = new CampusRepoImpl(context);
        }
        public IEnumerable<Campus> GetCampuses()
        {
            return _campusRepository.GetCampuses();
        }
    }
}
