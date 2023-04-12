using Microsoft.CodeAnalysis.Differencing;
using Microsoft.EntityFrameworkCore;
using SP23_G21_SHSMS.Models.Campuses;
using SP23_G21_SHSMS.Models.SHSMS;

namespace SP23_G21_SHSMS.Repositories.Campus_Repository.StorageRepo
{
    public class StorageRepoImpl : IStorageRepo
    {
        private DbCampusContext _context;
        public StorageRepoImpl(DbCampusContext context)
        {
            _context = context;
        }

        public IEnumerable<Storage> GetInStockStorages()
        {
            return _context.Storages.Where(x => x.Quantity > 0).Include(x => x.Consignment).ToList();
        }

        public Storage GetStorageById(int id)
        {
            return _context.Storages.Find(id);
        }

        /*public Storage GetStorageByImportAndMedicine(int impId, string medId)
        {            
            return _context.Storages.Where(x => x.ImportId == impId && x.MedicineId.Equals(medId)).FirstOrDefault();
        }*/

        public IEnumerable<Storage> GetUsingStorages()
        {
            var storages = _context.Storages.Include(x => x.Consignment)
                .Where(x => x.Quantity > 0 && x.Consignment.ExpiredDate > DateTime.Now)
                .OrderBy(x => x.Consignment.ExpiredDate).ToList();
            return storages;
        }

        public IEnumerable<Storage> GetUsingStorageByMedicineId(string medId)
        {
            var storages = _context.Storages.Include(x => x.Consignment)
                .Where(x => x.MedicineId.ToLower().Equals(medId.ToLower()) && x.Quantity > 0 && x.Consignment.ExpiredDate > DateTime.Now)
                .OrderBy(x => x.Consignment.ExpiredDate).ToList();
            return storages;
        }

        public void RemoveStorage(Storage storage)
        {
            _context.Storages.Remove(storage);
            _context.SaveChanges();
        }

        public void SaveStorage(Storage storage)
        {
            _context.Storages.Add(storage);
            _context.SaveChanges();
        }

        public void UpdateStorage(Storage storage)
        {
            _context.Storages.Update(storage);
            _context.SaveChanges();
        }
    }
}
