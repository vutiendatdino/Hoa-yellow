using Microsoft.CodeAnalysis.Differencing;
using Microsoft.EntityFrameworkCore;
using SP23_G21_SHSMS.Models.Campuses;
using SP23_G21_SHSMS.Models.CustomModels;

namespace SP23_G21_SHSMS.Repositories.Campus_Repository.ConsignmentRepo
{
    public class ConsignmentRepoImpl : IConsignmentRepo
    {
        private readonly DbCampusContext _context;
        public ConsignmentRepoImpl(DbCampusContext context)
        {
            _context = context;
        }

        public IEnumerable<Consignment> GetCanUseConsignments()
        {
            try
            {
                return _context.Consignments.Where(x => x.ExpiredDate > DateTime.Now)
                    .Include(x => x.Imports).Include(x => x.Storages).ToList();
            }
            catch (Exception ex)
            {
                // Log the exception or handle it in some other way
                throw new Exception("An error occurred while getting consignments.", ex);
            }
        }

        public IEnumerable<Consignment> GetCanUseConsignmentsByMedicineId(string medId)
        {
            //TO DO: fix cai nay
            try
            {
                return _context.Consignments
                .Where(x => x.ExpiredDate > DateTime.Now && x.MedicineId.ToLower().Trim().Equals(medId.ToLower().Trim()))
                .Include(x => x.Imports).Include(x => x.Storages)
                .OrderBy(x => x.ExpiredDate)
                .ToList();
            }
            catch (Exception ex)
            {
                // Log the exception or handle it in some other way
                throw new Exception("An error occurred while getting consignments.", ex);
            }
        }

        /*public IEnumerable<Storage> GetCanUseStorageByMedicineId(string medId)
        {
            var cons = GetCanUseConsignmentsByMedicineId(medId);
            var storages = new List<Storage>();
            foreach (var item in cons)
            {
                foreach (var imp in item.Imports.ToList())
                {
                    //TO DO: fix cai nay
                    *//*foreach (var s in imp.Storages.ToList())
                    {
                        storages.Add(s);
                    }*//*
                }
            }
            return storages;
        }*/

        public Consignment GetConsignmentById(int id)
        {
            try
            {
                return _context.Consignments.Where(x => x.ConsignmentId == id).Include(x => x.Imports).Include(x => x.Storages).FirstOrDefault();
            }
            catch (Exception ex)
            {
                // Log the exception or handle it in some other way
                throw new Exception("An error occurred while getting consignments.", ex);
            }
        }

        public IEnumerable<Consignment> GetConsignments()
        {
            return _context.Consignments.Include(x => x.Imports).Include(x => x.Imports).Include(x => x.Storages).ToList();
        }

        public Consignment GetLastestConsignment()
        {
            try
            {
                return _context.Consignments.OrderByDescending(x => x.ConsignmentId).Include(x => x.Imports).Include(x => x.Storages).FirstOrDefault();
            }
            catch (Exception ex)
            {
                // Log the exception or handle it in some other way
                throw new Exception("An error occurred while getting consignments.", ex);
            }
        }

        public int GetNewestId()
        {
            return _context.Consignments.OrderByDescending(x => x.ConsignmentId).Select(x => x.ConsignmentId).FirstOrDefault();
        }

        public IEnumerable<Consignment> GetOutOfDateConsignments()
        {
            try
            {
                return _context.Consignments.Where(x => x.ExpiredDate < DateTime.Now).Include(x => x.Imports).Include(x => x.Storages).ToList();
            }
            catch (Exception ex)
            {
                // Log the exception or handle it in some other way
                throw new Exception("An error occurred while getting consignments.", ex);
            }
        }

        public void RemoveConsigment(Consignment consignment)
        {
            try
            {
                _context.Consignments.Remove(consignment);
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while delete the consignment.", ex);
            }
        }

        public void SaveConsignment(Consignment consignment)
        {
            try
            {
                // Save the consignment
                _context.Consignments.Add(consignment);
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                // Log the exception or handle it in some other way
                throw new Exception("An error occurred while saving the consignment.", ex);
            }
        }

        public void UpdateConsigment(Consignment consignment)
        {
            try
            {
                _context.Consignments.Update(consignment);
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while updating the consignment.", ex);
            }
        }
    }
}
