using SP23_G21_SHSMS.Models.Campuses;
using SP23_G21_SHSMS.Repositories.Campus_Repository.ExportRepo;

namespace SP23_G21_SHSMS.Services.Campus_Services.ExportServices
{
    public class ExportSvImpl : IExportSv
    {
        private DbCampusContext _context;
        private IExportRepo _exportRepo;
        public ExportSvImpl(DbCampusContext context)
        {
            _context = context;
            _exportRepo = new ExportRepoImpl(_context);
        }

        public Export GetExportByExportId(int exportId)
        {
            if (!_exportRepo.FindExportByExportId(exportId))
                return null;
            return _exportRepo.GetExportByExportId(exportId);
        }

        public IEnumerable<Export> GetExports()
        {
            if (_exportRepo == null) return null;
            var exports = _exportRepo.GetExports();
            if (exports is null || !exports.Any() || exports.Count() == 0)
                return null;
            return exports;
        }

        public IEnumerable<Export> GetExportsByMedicalRecordId(int medicalRecordId)
        {
            if (_exportRepo == null) return null;
            var exports = _exportRepo.GetExportsByMedicalRecordId(medicalRecordId);
            if (exports is null || !exports.Any() || exports.Count() == 0)
                return null;
            return exports;
        }

        public bool IsExportExist(int exportId)
        {
            if (_exportRepo == null) return false;
            if (_exportRepo.FindExportByExportId(exportId))
                return true;
            return false;
        }

        public bool SaveExport(Export export)
        {
            try
            {
                _exportRepo.SaveExport(export);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool UpdateExport(Export export)
        {
            throw new NotImplementedException();
        }

        public bool DeleteExport(Export exportId)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Export> FindExportsByExportTypeId(int typeId)
        {
            try
            {
                return _exportRepo.GetExportsByExportTypeId(typeId);
            }
            catch
            {
                return null;
            }
        }
    }
}
