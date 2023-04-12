using SP23_G21_SHSMS.Models.SHSMS;

namespace SP23_G21_SHSMS.Repositories.SHSMS_Repository.MedicineRepo
{
    public interface IMedicineRepo
    {
        IEnumerable<Medicine> GetMedicines();
        IEnumerable<Medicine> GetMedicinesByName(string medicineName);
        IEnumerable<Medicine> GetMedicinesByCategoryId(int categoryId);
        Medicine GetMedicineById(string medicineId);
        bool FindMedicineById(string medicineId);
        void AddMedicine(Medicine medicine);
        void UpdateMedicine(Medicine medicine);
        void DeleteMedicine(Medicine medicine);
    }
}
