using Microsoft.EntityFrameworkCore;
using SP23_G21_SHSMS.Models.SHSMS;

namespace SP23_G21_SHSMS.Repositories.SHSMS_Repository.MedicineRepo
{
    public class MedicineRepoImpl : IMedicineRepo
    {
        private DbShsmsContext _context;
        public MedicineRepoImpl(DbShsmsContext context)
        {
            _context = context;
        }

        public bool FindMedicineById(string medicineId)
        {
            return _context.Medicines
                .Include(m => m.Category)
                .Include(m => m.Manufacture)
                .Include(m => m.Unit)
                .Any(m => m.MedicineId.Trim().ToLower().Equals(medicineId.ToLower()));
        }

        public Medicine GetMedicineById(string medicineId)
        {
            return _context.Medicines
                .Include(m => m.Category)
                .Include(m => m.Manufacture)
                .Include(m => m.Unit)
                .SingleOrDefault(m => m.MedicineId.Trim().ToLower().Equals(medicineId.ToLower()));
        }

        public IEnumerable<Medicine> GetMedicines()
        {
            return _context.Medicines
                .Include(m => m.Category)
                .Include(m => m.Manufacture)
                .Include(m => m.Unit)
                .ToList();
        }

        public IEnumerable<Medicine> GetMedicinesByName(string medicineName)
        {
            return _context.Medicines
                .Include(m => m.Category)
                .Include(m => m.Manufacture)
                .Include(m => m.Unit)
                .Where(m => m.MedicineName.Trim().ToLower().Contains(medicineName.ToLower())).ToList();
        }

        public void AddMedicine(Medicine medicine)
        {
            _context.Medicines.Add(medicine);
            _context.SaveChanges();
        }

        public void DeleteMedicine(Medicine medicine)
        {
            var medicineTmp = GetMedicineById(medicine.MedicineId);
            _context.Medicines.Remove(medicineTmp);
            _context.SaveChanges();
        }
        
        public void UpdateMedicine(Medicine medicine)
        {
            _context.Entry<Medicine>(medicine).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            _context.SaveChanges();
        }

        public IEnumerable<Medicine> GetMedicinesByCategoryId(int categoryId)
        {
            return _context.Medicines
                .Include(m => m.Category)
                .Include(m => m.Manufacture)
                .Include(m => m.Unit)
                .Where(m => m.CategoryId == categoryId)
                .ToList();
        }
    }
}
