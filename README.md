## Requisition module (for other modules)
- RequisitionService.MarkPendingFulfillment(rfId): purchasing calls this after sending the PO.
- Shortage per item: RequisitionItems.QtyShortage (only for Approved RFs).
- RF statuses: Submitted > Approved > PendingFulfillment > Fulfilled (exits: ReturnedForRevision, Cancelled).
