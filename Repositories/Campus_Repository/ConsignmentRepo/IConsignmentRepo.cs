using SP23_G21_SHSMS.Models.Campuses;

namespace SP23_G21_SHSMS.Repositories.Campus_Repository.ConsignmentRepo
{
    public interface IConsignmentRepo
    {
        IEnumerable<Consignment> GetConsignments();
        IEnumerable<Consignment> GetCanUseConsignments();
        IEnumerable<Consignment> GetOutOfDateConsignments();
        IEnumerable<Consignment> GetCanUseConsignmentsByMedicineId(string medId);
        /*IEnumerable<Storage> GetCanUseStorageByMedicineId(string medId);*/
        void SaveConsignment(Consignment consignment);
        void RemoveConsigment(Consignment consignment);
        void UpdateConsigment(Consignment consignment);
        int GetNewestId();
        Consignment GetConsignmentById(int id);
        Consignment GetLastestConsignment();
    }
}
