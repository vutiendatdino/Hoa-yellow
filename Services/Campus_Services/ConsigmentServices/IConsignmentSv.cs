using SP23_G21_SHSMS.Models.Campuses;

namespace SP23_G21_SHSMS.Services.Campus_Services.ConsigmentServices
{
    public interface IConsignmentSv
    {
        bool SaveConsignment(Consignment consignment);
        int GetNewestId();
        IEnumerable<Consignment> GetCanUseConsignments();
        IEnumerable<Consignment> GetOutOfDateConsignments();
        IEnumerable<Consignment> GetCanUseConsignmentsByMedicineId(string medId);
        //IEnumerable<Storage> GetCanUseStorageByMedicineId(string medId);
        Consignment GetConsignmentById(int id);
        Consignment GetLastestConsignment();
        void UpdateConsigment(Consignment consignment);
    }
}
