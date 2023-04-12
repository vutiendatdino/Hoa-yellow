using SP23_G21_SHSMS.Models.SHSMS;
using SP23_G21_SHSMS.Repositories.SHSMS_Repository.CategoryRepo;

namespace SP23_G21_SHSMS.Services.SHSMS_Services.CategoryServices
{
    public class CategorySvImpl : ICategorySv
    {
        private DbShsmsContext _context;
        private ICategoryRepo _categoryRepo;
        public CategorySvImpl(DbShsmsContext context)
        {
            _context = context;
            _categoryRepo = new CategoryRepoImpl(_context);
        }
        public IEnumerable<Category> GetCategories()
        {
            if (_context.Categories == null) return null;
            try
            {
                return _categoryRepo.GetCategories();
            }
            catch
            {
                return null;
            }
        }

        public Category GetCategoryByCategoryId(int categoryId)
        {
            if (_context.Categories == null || categoryId <=0) return null;
            try
            {
                return _categoryRepo.GetCategoryByCategoryId(categoryId);
            }
            catch
            {
                return null;
            }
        }

        public bool IsCategoryExist(int categoryId)
        {
            if (_context.Categories == null) return false;
            try
            {
                return _categoryRepo.FindCategory(categoryId);
            }
            catch
            {
                return false;
            }
        }
    }
}
