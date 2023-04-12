using SP23_G21_SHSMS.Models.Campuses;
using SP23_G21_SHSMS.Repositories.Campus_Repository.ConsignmentRepo;
using SP23_G21_SHSMS.Repositories.Campus_Repository.ExportDetailRepo;
using SP23_G21_SHSMS.Repositories.Campus_Repository.ExportRepo;
using System.Composition;

namespace SP23_G21_SHSMS.Services.Campus_Services.ExportDetailServices
{
    public class ExportDetailSvImpl : IExportDetailSv
    {
        private DbCampusContext _context;
        private IExportDetailRepo _exportDetailRepo;
        public ExportDetailSvImpl(DbCampusContext context)
        {
            _context = context;
            _exportDetailRepo = new ExportDetailRepoImpl(_context);
        }
        public IEnumerable<ExportDetail> GetExportDetailsByExportId(int exportId)
        {
            return _exportDetailRepo.GetExportDetailsByExportId(exportId);
        }

        public bool SaveExportDetail(ExportDetail exportDeatail)
        {
            try
            {
                _exportDetailRepo.SaveExportDetail(exportDeatail);
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
