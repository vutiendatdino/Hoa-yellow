using SP23_G21_SHSMS.Models.Campuses;

namespace SP23_G21_SHSMS.Repositories.Campus_Repository.StorageRepo
{
    public interface IStorageRepo
    {
        IEnumerable<Storage> GetUsingStorages();
        IEnumerable<Storage> GetInStockStorages();
        IEnumerable<Storage> GetUsingStorageByMedicineId(string medId);
        Storage GetStorageById(int id);
        //Storage GetStorageByImportAndMedicine(int impId, string medId);
        void SaveStorage(Storage storage);
        void RemoveStorage(Storage storage);
        void UpdateStorage(Storage storage);
    }
}
