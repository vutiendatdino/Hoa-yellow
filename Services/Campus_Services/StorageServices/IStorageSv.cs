using SP23_G21_SHSMS.Models.Campuses;

namespace SP23_G21_SHSMS.Services.Campus_Services.StorageServices
{
    public interface IStorageSv
    {
        IEnumerable<Storage> GetStorages();
        IEnumerable<Storage> GetInStockStorages();
        IEnumerable<Storage> GetUsingStorageByMedicineId(string medId);
        Storage FindStorageById(int id);
        /*Storage GetStorageByImportAndMedicine(int impId, string medId);*/
        bool SaveStorage(Storage storage);
        bool UpdateStorage(Storage storage);
    }
}
