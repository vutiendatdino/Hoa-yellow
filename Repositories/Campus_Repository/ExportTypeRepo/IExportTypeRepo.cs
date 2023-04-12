using SP23_G21_SHSMS.Models.Campuses;

namespace SP23_G21_SHSMS.Repositories.Campus_Repository.ExportTypeRepo
{
	public interface IExportTypeRepo
	{
		IEnumerable<ExportType> GetExportTypes();
		ExportType GetExportTypeById(int id);
	}
}
