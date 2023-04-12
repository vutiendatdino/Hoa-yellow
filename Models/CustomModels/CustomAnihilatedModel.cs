using SP23_G21_SHSMS.Models.Campuses;
using SP23_G21_SHSMS.Models.SHSMS;

namespace SP23_G21_SHSMS.Models.CustomModels
{
    public class CustomAnihilatedModel
    {
        public Consignment Consignment { get; set; }
        public Medicine Medicine { get; set; }
        public Export Export { get; set; }
        public ExportDetail ExportDetail { get; set; }
    }
}
