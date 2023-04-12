using SP23_G21_SHSMS.Models.Campuses;

namespace SP23_G21_SHSMS.Repositories.Campus_Repository.TreatmentRepo
{
	public class TreatmentRepoImpl : ITreatmentRepo
	{
		private readonly DbCampusContext _context;
		public TreatmentRepoImpl(DbCampusContext context)
		{
			_context = context;
		}

		public Treatment GetTreatmentById(int treatmentId)
		{
			return _context.Treatments.Find(treatmentId);
		}

		public IEnumerable<Treatment> GetTreatments()
		{
			return _context.Treatments.ToList();
		}

		public void SaveTreatment(Treatment treatment)
		{
			_context.Treatments.Add(treatment);
			_context.SaveChanges();
		}

		public void UpdateTreatment(Treatment treatment)
		{
			_context.Entry<Treatment>(treatment).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
			_context.SaveChanges();
		}

		public void DeleteTreatment(Treatment treatment)
		{
			var treatmentTmp = _context.Treatments.FirstOrDefault(t => t.TreatmentId ==  treatment.TreatmentId);
			_context.Treatments.Remove(treatmentTmp);
			_context.SaveChanges();
		}
	}
}
