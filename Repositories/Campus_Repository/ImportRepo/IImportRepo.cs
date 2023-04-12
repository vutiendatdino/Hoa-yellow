using SP23_G21_SHSMS.Models.Campuses;

namespace SP23_G21_SHSMS.Repositories.Campus_Repository.ImportRepo
{
    public interface IImportRepo
    {
        IEnumerable<Import> GetImports();
        Import GetImportById(int id);
        Import GetLatestImport();
        int GetNewestImportId();
        void SaveImport(Import import);
        void RemoveImport(Import import);
        void UpdateImport(Import import);
    }
}
