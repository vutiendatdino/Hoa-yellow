using SP23_G21_SHSMS.Models.Campuses;

namespace SP23_G21_SHSMS.Repositories.Campus_Repository.TreatmentRepo
{
	public interface ITreatmentRepo
	{
		IEnumerable<Treatment> GetTreatments();
		Treatment GetTreatmentById(int treatmentId);
		void SaveTreatment(Treatment treatment);
		void UpdateTreatment(Treatment treatment);
		void DeleteTreatment(Treatment treatment);
	}
}
