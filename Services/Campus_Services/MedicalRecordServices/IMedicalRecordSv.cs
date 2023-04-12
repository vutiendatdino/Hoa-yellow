using SP23_G21_SHSMS.Models.Campuses;

namespace SP23_G21_SHSMS.Services.Campus_Services.MedicalRecordServices
{
    public interface IMedicalRecordSv
    {
        IEnumerable<MedicalRecord> GetMedicalRecords();
        IEnumerable<MedicalRecord> GetMedicalRecordsByRollNumber(string rollNumber);
        MedicalRecord GetMedicalRecordById(int id);
        bool IsMedicalRecordExist(int id);
        int SaveMedicalRecord(MedicalRecord medicalRecord);
        bool RemoveMedicalRecord(MedicalRecord medicalRecord);
        bool UpdateMedicalRecord(int id, MedicalRecord medicalRecord);
    }
}
