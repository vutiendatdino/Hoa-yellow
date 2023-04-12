using SP23_G21_SHSMS.Models.Campuses;
using SP23_G21_SHSMS.Models.SHSMS;

namespace SP23_G21_SHSMS.Models.CustomModels
{//TO DO: sua sau khi code lai theo logic cua db moi
    public class CustomViewStorageModel
    {
        public Consignment Consignment { get; set; }
        public Storage Storage { get; set; }
        public Medicine Medicine { get; set; }
    }
}
