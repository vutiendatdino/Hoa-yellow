using SP23_G21_SHSMS.Models.SHSMS;

namespace SP23_G21_SHSMS.Services.SHSMS_Services.MedicineServices
{
    public interface IMedicineSv
    {
        IEnumerable<Medicine> GetMedicines();
        IEnumerable<Medicine> GetMedicinesByName(string medicineName);
        IEnumerable<Medicine> GetMedicinesByCategory(int categoryId);
        Medicine GetMedicineById(string medicineId);
        bool IsMedicineExist(string medicineId);
        bool SaveMedicine(Medicine medicine);
        bool UpdateMedicine(Medicine medicine);
        bool RemoveMedicine(Medicine medicine);
    }
}
