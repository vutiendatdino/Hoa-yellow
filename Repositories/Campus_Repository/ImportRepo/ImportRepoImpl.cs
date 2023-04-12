using Microsoft.EntityFrameworkCore;
using SP23_G21_SHSMS.Models.Campuses;

namespace SP23_G21_SHSMS.Repositories.Campus_Repository.ImportRepo
{
    public class ImportRepoImpl : IImportRepo
    {
        private readonly DbCampusContext _context;
        public ImportRepoImpl(DbCampusContext context)
        {
            _context = context;
        }

        public Import GetImportById(int id)
        {
            return _context.Imports.Where(x => x.ImportId == id).Include(x => x.Consignments).FirstOrDefault();
        }

        public IEnumerable<Import> GetImports()
        {
            return _context.Imports.Include(x => x.Consignments).ToList();
        }

        public Import GetLatestImport()
        {
            return _context.Imports.OrderByDescending(x => x.ImportId).FirstOrDefault();
        }

        public int GetNewestImportId()
        {
            return _context.Imports.OrderByDescending(x => x.ImportId).Select(x => x.ImportId).FirstOrDefault();
        }

        public void RemoveImport(Import import)
        {
            throw new NotImplementedException();
        }

        public void SaveImport(Import import)
        {
            _context.Imports.Add(import);
            _context.SaveChanges();
        }

        public void UpdateImport(Import import)
        {
            _context.Imports.Update(import);
            _context.SaveChanges();
        }
    }
}
