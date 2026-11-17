using System.Configuration;
using System.Data.Linq;

namespace SolarWorldERP.Data
{
    public class ErpDataContext : DataContext
    {
        public ErpDataContext()
            : base(ConfigurationManager.ConnectionStrings["ErpDb"].ConnectionString) { }

        public Table<User> Users { get { return GetTable<User>(); } }
        public Table<Item> Items { get { return GetTable<Item>(); } }
        public Table<Requisition> Requisitions { get { return GetTable<Requisition>(); } }
        public Table<RequisitionItem> RequisitionItems { get { return GetTable<RequisitionItem>(); } }
    }
}
