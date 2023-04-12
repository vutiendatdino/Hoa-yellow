using Microsoft.EntityFrameworkCore;
using SP23_G21_SHSMS.Models.SHSMS;

namespace SP23_G21_SHSMS.Repositories.SHSMS_Repository.CategoryRepo
{
    public class CategoryRepoImpl : ICategoryRepo
    {
        private DbShsmsContext _context;
        public CategoryRepoImpl(DbShsmsContext context)
        {
            _context = context;
        }

        public bool FindCategory(int categoryId)
        {
            return _context.Categories
                .Any(c => c.CategoryId == categoryId);
        }

        public IEnumerable<Category> GetCategories()
        {
            return _context.Categories
                .Include(c => c.Medicines)
                .ThenInclude(m => m.Unit)
                .ToList();
        }

        public Category GetCategoryByCategoryId(int categoryId)
        {
            return _context.Categories
                .Include(c => c.Medicines)
                .ThenInclude(m => m.Unit)
                .SingleOrDefault(c => c.CategoryId == categoryId);
        }
    }
}
