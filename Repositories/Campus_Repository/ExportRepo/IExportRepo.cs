using SP23_G21_SHSMS.Models.Campuses;

namespace SP23_G21_SHSMS.Repositories.Campus_Repository.ExportRepo
{
    public interface IExportRepo
    {
        IEnumerable<Export> GetExports();
        IEnumerable<Export> GetExportsByMedicalRecordId(int medicalId);
        IEnumerable<Export> GetExportsByExportTypeId(int typeId);
        Export GetExportByExportId(int exportId);
        bool FindExportByExportId(int exportId);
        void SaveExport(Export export);
        void RemoveExport(Export export);
        void UpdateExport(Export export);
    }
}
