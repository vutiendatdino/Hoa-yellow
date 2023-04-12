using SP23_G21_SHSMS.Models.Campuses;

namespace SP23_G21_SHSMS.Repositories.Campus_Repository.ExportTypeRepo
{
	public class ExportTypeRepoImpl : IExportTypeRepo
	{
		private readonly DbCampusContext _context;
		public ExportTypeRepoImpl(DbCampusContext context)
		{
			_context = context;
		}
		public ExportType GetExportTypeById(int id)
		{
			return _context.ExportTypes.Find(id);
		}

		public IEnumerable<ExportType> GetExportTypes()
		{
			return _context.ExportTypes.ToList();
		}
	}
}
