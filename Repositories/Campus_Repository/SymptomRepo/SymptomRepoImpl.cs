using SP23_G21_SHSMS.Models.Campuses;

namespace SP23_G21_SHSMS.Repositories.Campus_Repository.SymptomRepo
{
	public class SymptomRepoImpl : ISymptomRepo
	{
		private readonly DbCampusContext _context;
		public SymptomRepoImpl(DbCampusContext context)
		{
			_context = context;
		}
		public IEnumerable<Symptom> GetSymptoms()
		{
			return _context.Symptoms.ToList();
		}
		public void SaveSymptom(Symptom symptom)
		{
			_context.Symptoms.Add(symptom);
			_context.SaveChanges();
		}
		public void RemoveSymptom(Symptom symptom)
		{
			var symptomTmp = _context.Symptoms.SingleOrDefault(s => s.SymptomId == symptom.SymptomId);
			_context.Symptoms.Remove(symptomTmp!);
			_context.SaveChanges();
		}
		public void UpdateSymptom(Symptom symptom)
		{
			_context.Entry<Symptom>(symptom).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
			_context.SaveChanges();
		}

        public IEnumerable<Symptom> GetSymptomsByName(string name)
        {
			return _context.Symptoms.Where(s => s.SymptomName.ToLower().Contains(name.ToLower())).ToList();
        }

        public Symptom GetSymptomById(int id)
        {
			return _context.Symptoms.Find(id);
        }

        public bool IsSymptomExist(int id)
        {
			return _context.Symptoms.Any(s => s.SymptomId == id);
        }
    }
}
