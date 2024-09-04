using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

public static class CNT
{
    public const string DataNotAvailable = "Data not available";
    public const string Unauthorized = "Unauthorized";
    public const string Username = "WERPUsername";
    public const string Password = "WERPPassword";
    public const string User = "WERPIsUser";
    public const string SuperAdmin = "SuperAdmin";
    public const string Admin = "Admin";
    public const string Approval = "Approval";
    public const string Operator = "Operator";
    public const string Text = "Text";
    public const string NavigateUrl = "NavigateUrl";
    public const string NonInventory = "NonInventory";
    public const string Installation = "Installation";
    public const string Retur = "Retur";
    public const string Disposal = "Disposal";

    public static class DV
    {
        public static class ReceivedItem
        {
            public const string PO = "dvPOReceivedItem";           
        }
        public static class WorkOrder
        {
            public const string Customer = "dvCustomerWorkOrder";
            public const string MeterCondition = "dvMeterCondition";
        }
        public static class Helpdesk
        {
            public const string ConductedBy = "dvHelpdeskConductedBy";
            public const string ConductedByCust = "dvHelpdeskConductedByCust";
        }
        public static class StockOut
        {
            public const string Code = "dvCode";
            public const string CodeLookup = "dvCodeLookup";
            
        }
    }

    public static class Menu
    {
        public const string SuperAdmin = "Super Admin";
        public const string Admin = "Admin";
        public const string Inventory = "Inventory";
        public const string StockOrder = "Stock Order";
        public const string StockReceived = "Stock Received";
        public const string StockReceivedPending = "Pending - Stock Received";
        public const string StockOut = "Stock Out";
        public const string StockOutRetur = "Retur - Stock Out";
        public const string WorkOrder = "Work Order";
    }
    public static class VS
    {
        public const string UsersId = "UsersId";
        public const string Username = "Username";
        public const string StartNo = "StartNo";
        public const string EndNo = "EndNo";
        public const string TotalRow = "TotalRow";
        public const string TotalPage = "TotalPage";
        public const string Criteria = "Criteria";
        public const string SortBy = "SortBy";
        public const string SearchData = "SearchData";
        public const string SortData = "SortData";
        public const string Data = "Data";
        public const string Table = "Table";
        public const string SelectedData = "SelectedData";
        public const string PageName = "PageName";
        public const string ItemId = "ItemId";
        public const string StockOutId = "StockOutId";
        public const string StockOutItemId = "StockOutItemId";
        public const string StockReceivedItemId = "StockReceivedItemId";
        public const string UseItem = "UseItem";
        public const string UsePhoto = "UsePhoto";
        public const string MeterCondition = "MeterCondition";
        public const string IsUsed = "IsUsed";        
        public const string WorkOrderId = "WorkOrderId";
        public const string WorkType = "WorkType";
        public const string ItemExist = "ItemExist";
        public const string PreviousStatus = "PreviousStatus";
    }
    public static class Session
    {
        public const string WorkUpdate = "WorkUpdateList";
        public const string WorkUpdateItem = "WorkUpdateItemList";
        public const string ErrorList = "ErrorList";
        public const string Wrapping = "Wrapping";
        public const string Customer = "Customer";
        public const string Criteria = "Criteria";
    }
    public static class Role
    {
        public const string Admin = "Admin";
        public const string Approver = "Approver";
        public const string SuperAdmin = "SuperAdmin";
        public const string Operator = "Operator";
    }
    public static class Access
    {
        public const string C = "Create";
        public const string R = "Read";
        public const string U = "Update";
        public const string D = "Delete";
        public const string V = "ViewAllData";
        public const string DEV = "Deviation";
    }
    public static class Status
    {
        public const string Assignment = "Assignment";
        public const string Preparation = "Preparation";
        public const string StockOut = "StockOut";
        public const string ItemReceived = "ItemReceived";
        public const string Started = "Started";
        public const string Inprogress = "InProgress";
        public const string Completed = "Completed";
        public const string Pending = "Pending";
        public const string Cancel = "Cancel";
    }
    public static class WorkOrder
    {
        public static class WorkType
        {
            public const string NonInventory = "NonInventory";
            public const string Installation = "Installation";
            public const string Disposal = "Disposal";
            public const string Retur = "Retur";
        }
    }
    public static class CL
    {
        public const string RevenueLoss = "RevenueLoss";
        public const string VolumeLoss = "VolumeLoss";
    }
}
