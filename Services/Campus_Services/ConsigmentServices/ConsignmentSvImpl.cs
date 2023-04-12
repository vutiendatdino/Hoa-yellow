using Microsoft.CodeAnalysis.Differencing;
using SP23_G21_SHSMS.Models.Campuses;
using SP23_G21_SHSMS.Repositories.Campus_Repository.ConsignmentRepo;
using SP23_G21_SHSMS.Repositories.Campus_Repository.ImportRepo;

namespace SP23_G21_SHSMS.Services.Campus_Services.ConsigmentServices
{//TO DO: sua sau khi code lai theo logic cua db moi
    public class ConsignmentSvImpl : IConsignmentSv
    {
        private DbCampusContext _context;
        private IConsignmentRepo _consigmentRepo;

        public ConsignmentSvImpl(DbCampusContext context)
        {
            _context = context;
            _consigmentRepo = new ConsignmentRepoImpl(_context);
        }

        public IEnumerable<Consignment> GetCanUseConsignments()
        {
            try
            {
                return _consigmentRepo.GetCanUseConsignments();
            }
            catch (Exception ex)
            {
                // Log the exception or handle it in some other way
                throw new Exception("An error occurred while getting consignments.", ex);
            }
        }

        public IEnumerable<Consignment> GetCanUseConsignmentsByMedicineId(string medId)
        {            
            try
            {
                return _consigmentRepo.GetCanUseConsignmentsByMedicineId(medId);
            }
            catch (Exception ex)
            {
                // Log the exception or handle it in some other way
                throw new Exception("An error occurred while getting consignments.", ex);
            }
        }

        /*public IEnumerable<Storage> GetCanUseStorageByMedicineId(string medId)
        {
            try
            {
                return _consigmentRepo.GetCanUseStorageByMedicineId(medId);
            }
            catch (Exception ex)
            {
                // Log the exception or handle it in some other way
                throw new Exception("An error occurred while getting consignments.", ex);
            }
        }*/

        public Consignment GetConsignmentById(int id)
        {
            try
            {
                return _consigmentRepo.GetConsignmentById(id);
            }
            catch (Exception ex)
            {
                // Log the exception or handle it in some other way
                throw new Exception("An error occurred while getting consignments.", ex);
            }

        }

        public Consignment GetLastestConsignment()
        {
            try
            {
                return _consigmentRepo.GetLastestConsignment();
            }
            catch (Exception ex)
            {
                // Log the exception or handle it in some other way
                throw new Exception("An error occurred while getting consignments.", ex);
            }
        }

        public int GetNewestId()
        {
            return _consigmentRepo.GetNewestId();
        }

        public IEnumerable<Consignment> GetOutOfDateConsignments()
        {            
            try
            {
                return _consigmentRepo.GetOutOfDateConsignments();
            }
            catch (Exception ex)
            {
                // Log the exception or handle it in some other way
                throw new Exception("An error occurred while getting consignments.", ex);
            }
        }

        public bool SaveConsignment(Consignment consignment)
        {
            try
            {
                if (consignment is null || string.IsNullOrEmpty(consignment.ConsignmentNumber))
                {
                    return false;
                }
                _consigmentRepo.SaveConsignment(consignment);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public void UpdateConsigment(Consignment consignment)
        {
            _consigmentRepo.UpdateConsigment(consignment);
        }
    }
}
