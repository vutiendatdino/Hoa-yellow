using SP23_G21_SHSMS.Models.Campuses;

namespace SP23_G21_SHSMS.Services.Campus_Services.ExportDetailServices
{
    public interface IExportDetailSv
    {
        IEnumerable<ExportDetail> GetExportDetailsByExportId(int exportId);
        bool SaveExportDetail(ExportDetail exportDeatail);
    }
}
