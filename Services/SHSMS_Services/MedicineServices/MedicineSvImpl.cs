using SP23_G21_SHSMS.Models.SHSMS;
using SP23_G21_SHSMS.Repositories.SHSMS_Repository.MedicineRepo;

namespace SP23_G21_SHSMS.Services.SHSMS_Services.MedicineServices
{
    public class MedicineSvImpl : IMedicineSv
    {
        private DbShsmsContext _context;
        private IMedicineRepo _medicineRepo;
        public MedicineSvImpl(DbShsmsContext context)
        {
            _context = context;
            _medicineRepo = new MedicineRepoImpl(_context);
        }
        public Medicine GetMedicineById(string medicineId)
        {
            if (string.IsNullOrEmpty(medicineId) || _context.Medicines is null) return null;
            return _medicineRepo.GetMedicineById(medicineId);
        }

        public IEnumerable<Medicine> GetMedicines()
        {
            if (_context.Medicines is null) return null;
            try
            {
                return _medicineRepo.GetMedicines();
            }
            catch
            {
                return null;
            }
        }

        public IEnumerable<Medicine> GetMedicinesByCategory(int categoryId)
        {
            if (_context.Medicines is null) return null;
            try
            {
                return _medicineRepo.GetMedicinesByCategoryId(categoryId);
            }
            catch
            {
                return null;
            }
        }

        public IEnumerable<Medicine> GetMedicinesByName(string medicineName)
        {
            if (_context.Medicines is null || string.IsNullOrEmpty(medicineName)) return null;
            try
            {
                return _medicineRepo.GetMedicinesByName(medicineName);
            }
            catch
            {
                return null;
            }
        }

        public bool IsMedicineExist(string medicineId)
        {
            if (string.IsNullOrEmpty(medicineId) || _context.Medicines is null) return false;
            try
            {
                return _medicineRepo.FindMedicineById(medicineId);
            }
            catch
            {
                return false;
            }
        }

        public bool RemoveMedicine(Medicine medicine)
        {
            if (medicine is null || _context.Medicines is null) return false;
            try
            {
                if (!IsMedicineExist(medicine.MedicineId)) return false;
                _medicineRepo.DeleteMedicine(medicine);
            }
            catch
            {
                return false;
            }
            return true;
        }

        public bool SaveMedicine(Medicine medicine)
        {
            if (medicine is null || _context.Medicines is null) return false;
            try
            {
                if (IsMedicineExist(medicine.MedicineId)) return false;
                _medicineRepo.AddMedicine(medicine);
            }
            catch
            {
                return false;
            }
            return true;
        }

        public bool UpdateMedicine(Medicine medicine)
        {
            if (medicine is null || _context.Medicines is null) return false;
            try
            {
                if (!IsMedicineExist(medicine.MedicineId)) return false;
                _medicineRepo.UpdateMedicine(medicine);
            }
            catch
            {
                return false;
            }
            return true;
        }
    }
}
