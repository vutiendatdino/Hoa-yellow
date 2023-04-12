using SP23_G21_SHSMS.Models.SHSMS;

namespace SP23_G21_SHSMS.Services.SHSMS_Services.CategoryServices
{
    public interface ICategorySv
    {
        IEnumerable<Category> GetCategories();
        Category GetCategoryByCategoryId(int categoryId);
        bool IsCategoryExist(int categoryId);
    }
}
