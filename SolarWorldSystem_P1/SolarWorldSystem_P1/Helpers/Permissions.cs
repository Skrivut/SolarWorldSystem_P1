using System;

namespace SolarWorldERP.Helpers
{
    /// <summary>
    /// The ONE place that says what each role may open and do in the screens (menus, buttons).
    /// The rules match the guards that already exist in the services:
    ///   - PurchasingService.Approve / Reject  -> Owner only
    ///   - everything else in purchasing       -> Owner and Staff
    /// Roles come from Session.Role: "Owner", "Staff" or "Driver".
    /// </summary>
    public static class Permissions
    {
        private static bool IsOwner { get { return Session.Role == "Owner"; } }
        private static bool IsStaff { get { return Session.Role == "Staff"; } }

        /// <summary>
        /// false (default) = staff PREPARE purchase orders (create a draft, submit it, manage suppliers, cancel)
        ///                   and the owner DECIDES (approve or reject). This is how the system works today.
        /// true            = staff can only LOOK at purchasing and follow up suppliers. Turning this on also needs the
        ///                   small edits to PurchasingService and frmPurchasing listed in the chat.
        /// </summary>
        public static bool StaffPurchasingViewOnly = false;

        // ----- Which menu items a role sees -----
        public static bool CanOpenInbox { get { return IsOwner || IsStaff; } }
        public static bool CanOpenRequests { get { return IsOwner || IsStaff; } }
        public static bool CanOpenPurchasing { get { return IsOwner || IsStaff; } }
        public static bool CanOpenDelivery { get { return true; } }   // owner, staff and driver
        public static bool CanOpenInventory { get { return IsOwner || IsStaff; } }

        // ----- What a role may DO elsewhere -----
        /// <summary>Shows the "+ New request" button in the inbox. Staff create requests; the owner approves them.</summary>
        public static bool CanCreateRequisition { get { return IsStaff; } }

        // ----- What a role may DO inside Purchasing -----
        /// <summary>Create a draft PO, submit it for approval, add or edit suppliers, cancel an order.</summary>
        public static bool CanPreparePurchaseOrders { get { return IsOwner || (IsStaff && !StaffPurchasingViewOnly); } }

        /// <summary>Approve or reject a PO. The owner only.</summary>
        public static bool CanApprovePurchaseOrders { get { return IsOwner; } }

        /// <summary>Send the driver again for a balance or a replacement.</summary>
        public static bool CanFollowUpSupplier { get { return IsOwner || IsStaff; } }

        /// <summary>True when the logged-in user sees Purchasing without its preparing buttons.</summary>
        public static bool PurchasingIsViewOnly { get { return IsStaff && StaffPurchasingViewOnly; } }

        /// <summary>
        /// Optional guard for services that do not have their own role check yet.
        /// (PurchasingService, DeliveryService, RequisitionService and InventoryService already have RequireRole.)
        /// </summary>
        public static void Require(bool allowed, string action)
        {
            if (!allowed)
                throw new UnauthorizedAccessException("Your role (" + Session.Role + ") is not allowed to " + action + ".");
        }
    }
}