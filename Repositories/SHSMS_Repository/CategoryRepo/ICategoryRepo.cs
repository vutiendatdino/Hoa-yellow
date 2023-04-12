using SP23_G21_SHSMS.Models.SHSMS;

namespace SP23_G21_SHSMS.Repositories.SHSMS_Repository.CategoryRepo
{
    public interface ICategoryRepo
    {
        IEnumerable<Category> GetCategories();
        Category GetCategoryByCategoryId(int categoryId);
        bool FindCategory(int categoryId);
    }
}
