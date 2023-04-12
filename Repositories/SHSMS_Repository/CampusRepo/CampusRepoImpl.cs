using SP23_G21_SHSMS.Models.SHSMS;

namespace SP23_G21_SHSMS.Repositories.SHSMS_Repository.CampusRepo
{
    public class CampusRepoImpl : ICampusRepo
    {
        private readonly DbShsmsContext _context;
        public CampusRepoImpl(DbShsmsContext context)
        {
            _context = context;
        }
        public IEnumerable<Campus> GetCampuses()
        {
            return _context.Campuses.ToList();
        }

        public void RemoveCampus(Campus campus)
        {
            var campusTmp = _context.Campuses.SingleOrDefault(c => c.CampusId == campus.CampusId);
            _context.Campuses.Remove(campusTmp!);
            _context.SaveChanges();
        }

        public void SaveCampus(Campus campus)
        {
            _context.Campuses.Add(campus);
            _context.SaveChanges();
        }

        public void UpdateCampus(Campus campus)
        {
            _context.Entry<Campus>(campus).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            _context.SaveChanges();
        }
    }
}
