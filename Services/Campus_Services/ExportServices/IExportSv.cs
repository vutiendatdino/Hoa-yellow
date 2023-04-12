using SP23_G21_SHSMS.Models.Campuses;

namespace SP23_G21_SHSMS.Services.Campus_Services.ExportServices
{
    public interface IExportSv
    {
        IEnumerable<Export> GetExports();
        IEnumerable<Export> GetExportsByMedicalRecordId(int medicalRecordId);
        IEnumerable<Export> FindExportsByExportTypeId(int typeId);
        Export GetExportByExportId(int exportId);
        bool IsExportExist(int exportId);
        bool SaveExport(Export export);
        bool UpdateExport(Export export);
        bool DeleteExport(Export exportId);
    }
}
