using SP23_G21_SHSMS.Models.Campuses;

namespace SP23_G21_SHSMS.Services.Campus_Services.ImportServices
{
    //TO DO: sua sau khi code lai theo logic cua db moi
    public interface IImportSv
    {
        bool SaveImport(Import import);
        int GetNewestImportID();
        Import GetLastestImport();
        Import GetImportById(int id);
        IEnumerable<Import> GetImportList();
        bool UpdateImport(Import import);
    }
}
