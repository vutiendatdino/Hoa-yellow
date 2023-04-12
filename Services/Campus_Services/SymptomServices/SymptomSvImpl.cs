using SP23_G21_SHSMS.Models.Campuses;
using SP23_G21_SHSMS.Repositories.Campus_Repository.SymptomRepo;

namespace SP23_G21_SHSMS.Services.Campus_Services.SymptomServices
{
    public class SymptomSvImpl : ISymptomSv
    {
        private DbCampusContext _context;
        private ISymptomRepo _symptomRepo;
        public SymptomSvImpl(DbCampusContext context)
        {
            _context = context;
            _symptomRepo = new SymptomRepoImpl(_context);
        }
        public Symptom FindSymptomsById(int symptomId)
        {
            try
            {
                if (_context.Symptoms == null || !IsSymptomExist(symptomId)) return null;
                return _symptomRepo.GetSymptomById(symptomId);
            }
            catch
            {
                return null;
            }
        }

        public IEnumerable<Symptom> FindSymptomsByName(string name)
        {
            try
            {
                if (string.IsNullOrEmpty(name) || _context.Symptoms == null) return null;
                var symptoms = _symptomRepo.GetSymptomsByName(name);
                if (symptoms is null || !symptoms.Any() || symptoms.Count() == 0) return null;
                return symptoms;
            }
            catch
            {
                return null;
            }
        }

        public IEnumerable<Symptom> GetSymptoms()
        {
            try
            {
                if (_context.Symptoms == null) return null;
                var symptoms = _symptomRepo.GetSymptoms();
                if (symptoms is null || !symptoms.Any() || symptoms.Count() == 0) return null;
                return symptoms;
            }
            catch
            {
                return null;
            }
        }

        public bool IsSymptomExist(int id)
        {
            try
            {
                return _symptomRepo.IsSymptomExist(id);
            }
            catch
            {
                return false;
            }
        }

        public bool RemoveSymptom(Symptom symptom)
        {
            try
            {
                if (_context.Symptoms == null || symptom is null || !_symptomRepo.IsSymptomExist(symptom.SymptomId)) return false;
                _symptomRepo.RemoveSymptom(symptom);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool SaveSymptom(Symptom symptom)
        {
            try
            {
                if (_context.Symptoms == null || symptom is null || string.IsNullOrEmpty(symptom.SymptomName)) return false;
                _symptomRepo.SaveSymptom(symptom);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool UpdateSymptom(int id, Symptom symptom)
        {
            if(id != symptom.SymptomId) return false;
            try
            {
                if (_context.Symptoms == null || symptom is null || !_symptomRepo.IsSymptomExist(symptom.SymptomId)) return false;
                _symptomRepo.UpdateSymptom(symptom);
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
