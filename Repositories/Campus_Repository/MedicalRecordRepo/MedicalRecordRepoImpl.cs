using Microsoft.EntityFrameworkCore;
using SP23_G21_SHSMS.Models.Campuses;

namespace SP23_G21_SHSMS.Repositories.Campus_Repository.MedicalRecordRepo
{
    public class MedicalRecordRepoImpl : IMedicalRecordRepo
    {
        private readonly DbCampusContext _context;
        public MedicalRecordRepoImpl(DbCampusContext context)
        {
            _context = context;
        }

        public bool FindMedicalRecordById(int id)
        {
            return _context.MedicalRecords.Any(mr => mr.MedicalRecordsId == id);
        }

        public MedicalRecord GetMedicalRecordById(int id)
        {
            return _context.MedicalRecords
                .Include(mr => mr.Symptoms)
                .Include(mr => mr.Exports)
                .SingleOrDefault(mr => mr.MedicalRecordsId == id);
        }

        public IEnumerable<MedicalRecord> GetMedicalRecords()
        {
            return _context.MedicalRecords
                .Where(m => m.ExaminationTime >= (DateTime.Now.AddDays(-7))
                    && m.ExaminationTime <= DateTime.Now
                    && !m.RollNumber.Equals("Temp"))
                .OrderByDescending(m => m.ExaminationTime)
                .ToList();
        }

        public IEnumerable<MedicalRecord> GetMedicalRecordsByRollNumber(string rollNumber)
        {
            return _context.MedicalRecords
                .Include(mr => mr.Symptoms)
                .Include(mr => mr.Exports)
                .Where(mr => mr.ExaminationTime >= (DateTime.Now.AddDays(-7))
                    && mr.ExaminationTime <= DateTime.Now
                    && mr.RollNumber.ToLower().Equals(rollNumber.ToLower()))
                .OrderByDescending(m => m.ExaminationTime)
                .ToList();
        }

        public IEnumerable<MedicalRecord> GetMedicalRecordsByTreatment(int treatmentId)
        {
            return _context.MedicalRecords
                .Include(mr => mr.Symptoms)
                .Include(mr => mr.Exports)
                .Where(mr => mr.TreatmentId == treatmentId)
                .ToList();
        }

        public void RemoveMedicalRecord(MedicalRecord medicalRecord)
        {
            var recordTmp = _context.MedicalRecords.FirstOrDefault(r => r.MedicalRecordsId == medicalRecord.MedicalRecordsId);
            _context.MedicalRecords.Remove(recordTmp);
            _context.SaveChanges();
        }

        public int SaveMedicalRecord(MedicalRecord medicalRecord)
        {
            _context.MedicalRecords.Add(medicalRecord);
            _context.SaveChanges();
            return medicalRecord.MedicalRecordsId;
        }

        public void UpdateMedicalRecord(MedicalRecord medicalRecord)
        {
            _context.Entry<MedicalRecord>(medicalRecord).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            _context.SaveChanges();
        }
    }
}
