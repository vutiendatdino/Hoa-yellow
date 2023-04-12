using SP23_G21_SHSMS.Models.Campuses;
using SP23_G21_SHSMS.Repositories.Campus_Repository.StorageRepo;

namespace SP23_G21_SHSMS.Services.Campus_Services.StorageServices
{
    public class StorageSvImpl : IStorageSv
    {
        private DbCampusContext _context;
        private IStorageRepo _storageRepo;

        public StorageSvImpl(DbCampusContext context)
        {
            _context = context;
            _storageRepo = new StorageRepoImpl(_context);
        }

        public Storage FindStorageById(int id)
        {
            try
            {
                return _storageRepo.GetStorageById(id);
            }
            catch
            {
                return null;
            }
        }

        public IEnumerable<Storage> GetInStockStorages()
        {
            try
            {
                return _storageRepo.GetInStockStorages();
            }
            catch
            {
                return null;
            }
        }

        //public Storage GetStorageByImportAndMedicine(int impId, string medId)
        //{
        //    try
        //    {
        //        return _storageRepo.GetStorageByImportAndMedicine(impId, medId);
        //    }
        //    catch
        //    {
        //        return null;
        //    }
        //}

        public IEnumerable<Storage> GetStorages()
        {
            try
            {
                return _storageRepo.GetUsingStorages();
            }
            catch
            {
                return null;
            }
        }

        public IEnumerable<Storage> GetUsingStorageByMedicineId(string medId)
        {
            try
            {
                return _storageRepo.GetUsingStorageByMedicineId(medId);
            }
            catch
            {
                return null;
            }
        }

        public bool SaveStorage(Storage storage)
        {
            try
            {
                if (storage is null)
                    return false;
                _storageRepo.SaveStorage(storage);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool UpdateStorage(Storage storage)
        {
            try
            {
                if (storage is null)
                {
                    return false;
                }
                else
                {
                    _storageRepo.UpdateStorage(storage);
                    return true;
                }
            }
            catch { return false; }
        }
    }
}
