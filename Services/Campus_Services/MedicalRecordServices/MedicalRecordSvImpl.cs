using SP23_G21_SHSMS.Models.Campuses;
using SP23_G21_SHSMS.Repositories.Campus_Repository.MedicalRecordRepo;

namespace SP23_G21_SHSMS.Services.Campus_Services.MedicalRecordServices
{
    public class MedicalRecordSvImpl : IMedicalRecordSv
    {
        private DbCampusContext _context;
        private IMedicalRecordRepo _medicalRecordRepo;
        public MedicalRecordSvImpl(DbCampusContext context)
        {
            _context = context;
            _medicalRecordRepo = new MedicalRecordRepoImpl(_context);
        }

        public MedicalRecord GetMedicalRecordById(int id)
        {
            if (!IsMedicalRecordExist(id))
                return null;
            return _medicalRecordRepo.GetMedicalRecordById(id);
        }

        public IEnumerable<MedicalRecord> GetMedicalRecords()
        {
            var records = _medicalRecordRepo.GetMedicalRecords();
            if (records == null || records.Count() == 0) 
                return null;
            return records;
        }

        public IEnumerable<MedicalRecord> GetMedicalRecordsByRollNumber(string rollNumber)
        {
            if (string.IsNullOrEmpty(rollNumber))
            {
                return null;
            }
            var records = _medicalRecordRepo.GetMedicalRecordsByRollNumber(rollNumber);
            if (records == null || records.Count() == 0)
                return null;
            return records;
        }

        public bool IsMedicalRecordExist(int id)
        {
            if (_medicalRecordRepo.FindMedicalRecordById(id))
                return true;
            return false;
        }

        public bool RemoveMedicalRecord(MedicalRecord medicalRecord)
        {
            try
            {
                if (_context.MedicalRecords is null || medicalRecord == null || !IsMedicalRecordExist(medicalRecord.MedicalRecordsId))
                    return false;
                _medicalRecordRepo.RemoveMedicalRecord(medicalRecord);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public int SaveMedicalRecord(MedicalRecord medicalRecord)
        {
            try
            {
                return _medicalRecordRepo.SaveMedicalRecord(medicalRecord);
                
            }
            catch
            {
                return -1;
            }
        }

        public bool UpdateMedicalRecord(int id, MedicalRecord medicalRecord)
        {
            if(id != medicalRecord.MedicalRecordsId) return false;
            try
            {
                if (_context.MedicalRecords is null || medicalRecord == null || !IsMedicalRecordExist(id)) 
                    return false;
                _medicalRecordRepo.UpdateMedicalRecord(medicalRecord);
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
