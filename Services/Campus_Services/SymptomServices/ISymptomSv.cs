using SP23_G21_SHSMS.Models.Campuses;

namespace SP23_G21_SHSMS.Services.Campus_Services.SymptomServices
{
    public interface ISymptomSv
    {
        IEnumerable<Symptom> GetSymptoms();
        IEnumerable<Symptom> FindSymptomsByName(string name);
        Symptom FindSymptomsById(int symptomId);
        bool SaveSymptom(Symptom symptom);
        bool RemoveSymptom(Symptom symptom);
        bool UpdateSymptom(int id, Symptom symptom);
        bool IsSymptomExist(int id);
    }
}
