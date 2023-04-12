using SP23_G21_SHSMS.Models.Campuses;

namespace SP23_G21_SHSMS.Repositories.Campus_Repository.SymptomRepo
{
	public interface ISymptomRepo
	{
		IEnumerable<Symptom> GetSymptoms();
		IEnumerable<Symptom> GetSymptomsByName(string name);
		bool IsSymptomExist(int id);
		Symptom GetSymptomById(int id);
		void SaveSymptom(Symptom symptom);
		void RemoveSymptom(Symptom symptom);
		void UpdateSymptom(Symptom symptom);
	}
}
