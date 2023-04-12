using SP23_G21_SHSMS.Models.Campuses;
using SP23_G21_SHSMS.Repositories.Campus_Repository.ImportRepo;

namespace SP23_G21_SHSMS.Services.Campus_Services.ImportServices
{//TO DO: sua sau khi code lai theo logic cua db moi
    public class ImportSvImpl : IImportSv
    {
        private DbCampusContext _context;
        private IImportRepo _importRepo;

        public ImportSvImpl(DbCampusContext context)
        {
            _context = context;
            _importRepo = new ImportRepoImpl(_context);
        }

        public Import GetImportById(int id)
        {
            return _importRepo.GetImportById(id);
        }

        public IEnumerable<Import> GetImportList()
        {
            return _importRepo.GetImports();
        }

        public Import GetLastestImport()
        {
            return _importRepo.GetLatestImport();
        }

        public int GetNewestImportID()
        {
            return _importRepo.GetNewestImportId();
        }

        public bool SaveImport(Import import)
        {
            try
            {
                if(import is null) 
                    return false;
                _importRepo.SaveImport(import);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool UpdateImport(Import import)
        {
            try
            {
                if (import is null)
                    return false;
                _importRepo.UpdateImport(import);
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
