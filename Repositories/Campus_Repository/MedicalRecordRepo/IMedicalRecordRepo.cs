using SP23_G21_SHSMS.Models.Campuses;

namespace SP23_G21_SHSMS.Repositories.Campus_Repository.MedicalRecordRepo
{
	public interface IMedicalRecordRepo
	{
		IEnumerable<MedicalRecord> GetMedicalRecords();
		IEnumerable<MedicalRecord> GetMedicalRecordsByRollNumber(string rollNumber);
		IEnumerable<MedicalRecord> GetMedicalRecordsByTreatment(int treatmentId);
		MedicalRecord GetMedicalRecordById(int id);
		bool FindMedicalRecordById(int id);
		int SaveMedicalRecord(MedicalRecord medicalRecord);
		void RemoveMedicalRecord(MedicalRecord medicalRecord);
		void UpdateMedicalRecord(MedicalRecord medicalRecord);
	}
}
