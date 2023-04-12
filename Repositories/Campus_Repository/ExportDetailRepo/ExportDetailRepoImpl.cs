using SP23_G21_SHSMS.Models.Campuses;
using System.Composition;

namespace SP23_G21_SHSMS.Repositories.Campus_Repository.ExportDetailRepo
{
    public class ExportDetailRepoImpl : IExportDetailRepo
    {
        private readonly DbCampusContext _context;
        public ExportDetailRepoImpl(DbCampusContext context)
        {
            _context = context;
        }

        public bool FindExportDetailsByExportId(int exportId)
        {
            return _context.ExportDetails.Any(ed => ed.ExportId == exportId);
        }

        public bool FindExportDetailsByMedicineId(string medicineId)
        {
            //TODO: Sửa lại anhpt
            //return _context.ExportDetails.Any(ed => ed.MedicineId.ToLower().Equals(medicineId.ToLower()));
            return false;
        }

        public IEnumerable<ExportDetail> GetExportDetails()
        {
            return _context.ExportDetails.ToList();
        }

        public IEnumerable<ExportDetail> GetExportDetailsByExportId(int exportId)
        {
            return _context.ExportDetails.Where(ed => ed.ExportId == exportId).ToList();
        }

        public ExportDetail GetExportDetailsByExportIdAndMedicineId(int exportId, string medicineId)
        {
            //TODO: Sửa lại anhpt
            //return _context.ExportDetails.SingleOrDefault(ed => ed.ExportId == exportId && ed.MedicineId.ToLower().Equals(medicineId.ToLower()));
            return null;
        }

        public IEnumerable<ExportDetail> GetExportDetailsByMedicineId(string medicineId)
        {
            //TODO: Sửa lại anhpt
            //return _context.ExportDetails.Where(ed => ed.MedicineId.ToLower().Equals(medicineId.ToLower())).ToList();
            return null;
        }

        public void SaveExportDetail(ExportDetail exportDetail)
        {
            _context.ExportDetails.Add(exportDetail);
            _context.SaveChanges();
        }

        public void UpdateExportDetail(ExportDetail exportDetail)
        {
            throw new NotImplementedException();
        }

        public void RemoveExportDetail(ExportDetail exportDetail)
        {
            throw new NotImplementedException();
        }
    }
}
