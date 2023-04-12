using Microsoft.EntityFrameworkCore;
using SP23_G21_SHSMS.Models.Campuses;

namespace SP23_G21_SHSMS.Repositories.Campus_Repository.ExportRepo
{
    public class ExportRepoImpl : IExportRepo
    {
        private readonly DbCampusContext _context;
        public ExportRepoImpl(DbCampusContext context)
        {
            _context = context;
        }
        public IEnumerable<Export> GetExports()
        {
            return _context.Exports
                .Include(e => e.ExportDetails)
                //.Include(e => e.MedicalRecords)
                .ToList();
        }

        public IEnumerable<Export> GetExportsByMedicalRecordId(int medicalId)
        {
            return _context.Exports
                .Include(e => e.ExportDetails)
                .Where(e => e.MedicalRecordsId == medicalId).ToList();
        }

        public void SaveExport(Export export)
        {
            _context.Exports.Add(export);
            _context.SaveChanges();
        }

        public void RemoveExport(Export export)
        {
            throw new NotImplementedException();
        }

        public void UpdateExport(Export export)
        {
            throw new NotImplementedException();
        }

        public Export GetExportByExportId(int exportId)
        {
            return _context.Exports.Include(e => e.ExportDetails).SingleOrDefault(e => e.ExportId == exportId);
        }

        public bool FindExportByExportId(int exportId)
        {
            return _context.Exports.Any(e => e.ExportId == exportId);
        }

        public IEnumerable<Export> GetExportsByExportTypeId(int typeId)
        {
            return _context.Exports.Where(e => e.ExportTypeId == typeId).ToList();
        }
    }
}
