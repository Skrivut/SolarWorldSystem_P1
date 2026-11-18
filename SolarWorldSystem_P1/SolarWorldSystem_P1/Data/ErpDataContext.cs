using System.Configuration;
using System.Data.Linq;

namespace SolarWorldERP.Data
{
    /// <summary>
    /// The one door to the database. Create it inside a using block, run your queries,
    /// call SubmitChanges() to save, and let the using block close the connection.
    /// </summary>
    public class ErpDataContext : DataContext
    {
        /// <summary>Opens the database using the connection string named "ErpDb" in App.config.</summary>
        public ErpDataContext()
            : base(ConfigurationManager.ConnectionStrings["ErpDb"].ConnectionString) { }

        /// <summary>The Users table.</summary>
        public Table<User> Users { get { return GetTable<User>(); } }

        /// <summary>The Items (stock) table.</summary>
        public Table<Item> Items { get { return GetTable<Item>(); } }

        /// <summary>The Requisitions (RF header) table.</summary>
        public Table<Requisition> Requisitions { get { return GetTable<Requisition>(); } }

        /// <summary>The RequisitionItems (RF lines) table.</summary>
        public Table<RequisitionItem> RequisitionItems { get { return GetTable<RequisitionItem>(); } }

        /// <summary>The PurchaseOrders table.</summary>
        public Table<PurchaseOrder> PurchaseOrders { get { return GetTable<PurchaseOrder>(); } }

        /// <summary>The PurchaseOrderLines table.</summary>
        public Table<PurchaseOrderLine> PurchaseOrderLines { get { return GetTable<PurchaseOrderLine>(); } }

        /// <summary>The Deliveries table.</summary>
        public Table<Delivery> Deliveries { get { return GetTable<Delivery>(); } }

        /// <summary>The DeliveryLines table.</summary>
        public Table<DeliveryLine> DeliveryLines { get { return GetTable<DeliveryLine>(); } }
    }
}
