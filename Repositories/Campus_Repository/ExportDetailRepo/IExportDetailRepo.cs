using SP23_G21_SHSMS.Models.Campuses;

namespace SP23_G21_SHSMS.Repositories.Campus_Repository.ExportDetailRepo
{
    public interface IExportDetailRepo
    {
        IEnumerable<ExportDetail> GetExportDetails();
        IEnumerable<ExportDetail> GetExportDetailsByExportId(int exportId);
        IEnumerable<ExportDetail> GetExportDetailsByMedicineId(string medicineId);
        ExportDetail GetExportDetailsByExportIdAndMedicineId(int exportId, string medicineId);
        bool FindExportDetailsByExportId(int exportId);
        bool FindExportDetailsByMedicineId(string medicineId);
        void SaveExportDetail(ExportDetail exportDetail);
        void RemoveExportDetail(ExportDetail exportDetail);
        void UpdateExportDetail(ExportDetail exportDetail);
    }
}
