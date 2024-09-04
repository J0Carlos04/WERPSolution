using DAL;
using System;
using System.Activities;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using U = Utility;

public class StockOutReturUsed
{
    #region Properties
    static string Qry = "Select a.*, b.Code WorkOrderCode, e.Code ItemCode, e.Name ItemName, g.Name Warehouse, h.Name Rack from StockOutReturUsed a Left Join WorkOrder b on b.id = a.WorkOrderId Left Join StockOut c on c.Id = a.StockOutId Left Join StockOutItem d On d.Id = a.StockOutItemId Left Join Item e on e.Id = d.ItemId Left Join Users f On f.Id = a.ReceiverUserId Left Join Warehouse g on g.Id = a.WarehouseId Left Join Rack h on h.Id = a.RackId";
    #region Fields
    [Column(Field = "Code", SearchName = "a.Code", SortName = "a.Code")]
    public string Code { get; set; }   
    [Column(Field = "Reason", SearchName = "a.Reason", SortName = "a.Reason")]
    public string Reason { get; set; }
    [Column(Field = "ReturDate", SearchName = "a.ReturDate", SortName = "a.ReturDate")]
    public DateTime ReturDate { get; set; }
    [Column(Field = "Qty", SearchName = "a.Qty", SortName = "a.Qty")]
    public int Qty { get; set; }    
    #endregion

    #region Key
    [Column(Field = "Id")]
    public int Id { get; set; }
    [Column(Field = "WorkOrderId")]
    public int WorkOrderId { get; set; }
    [Column(Field = "WorkOrderWorkUpdateId")]
    public int WorkOrderWorkUpdateId { get; set; }
    [Column(Field = "WorkOrderWorkUpdateItemId")]
    public int WorkOrderWorkUpdateItemId { get; set; }
    [Column(Field = "StockReceivedItemId")]
    public int StockReceivedItemId { get; set; }
    [Column(Field = "StockOutId")]
    public int StockOutId { get; set; }
    [Column(Field = "StockOutItemId")]
    public int StockOutItemId { get; set; }
    [Column(Field = "ReceiverUserId")]
    public int ReceiverUserId { get; set; }
    [Column(Field = "WarehouseId")]
    public int WarehouseId { get; set; }
    [Column(Field = "RackId")]
    public int RackId { get; set; }
    [Column(Field = "ItemId")]
    public int ItemId { get; set; }
    #endregion

    #region Additional
    [Column(Field = "WorkOrderCode", Title = "Work Order Code", SearchName = "b.code", SortName = "b.code")]
    public string WorkOrderCode { get; set; }
    [Column(Field = "WorkUpdateDate", Title = "Work Update Date", SearchName = "c.Name", SortName = "c.Name")]
    public string WorkUpdateDate { get; set; }
    [Column(Field = "ItemCode", Title = "Item Code", SearchName = "d.Code", SortName = "d.Code")]
    public string ItemCode { get; set; }
    [Column(Field = "ItemName", Title = "Item Name", SearchName = "d.Name", SortName = "d.Name")]
    public string ItemName { get; set; }
    [Column(Field = "Warehouse", Title = "Warehouse", SearchName = "f.Name", SortName = "f.Name")]
    public string Warehouse { get; set; }
    [Column(Field = "Rack", Title = "Rack", SearchName = "g.Name", SortName = "g.Name")]
    public string Rack { get; set; }
    [Column(Field = "Receiver", SearchName = "e.Name", SortName = "e.Name")]
    public string Receiver { get; set; }
    #endregion

    #region Default    
    [Column(Field = "CreatedBy")]
    public string CreatedBy { get; set; }
    [Column(Field = "Created")]
    public DateTime Created { get; set; }
    [Column(Field = "ModifiedBy")]
    public string ModifiedBy { get; set; }
    [Column(Field = "Modified")]
    public DateTime Modified { get; set; }
    [Column(Field = "No")]
    public double No { get; set; }
    public bool IsChecked { get; set; }
    public string Mode { get; set; }
    #endregion

    #endregion

    #region Methods
    #region Get Data
    public static StockOutReturUsed GetById(object Id)
    {
        return (StockOutReturUsed)DataAccess.GetSingleRowByQuery(0, $"{Qry} where a.Id = '{Id}'", new StockOutReturUsed());
    }
    public static StockOutReturUsed GetByWorkOrderId(object WorkOrderId)
    {
        return (StockOutReturUsed)DataAccess.GetSingleRowByQuery(0, $"{Qry} where a.WorkOrderId = '{WorkOrderId}'", new StockOutReturUsed());
    }
    public static StockOutReturUsed GetByWorkOrderWorkUpdateId(object WorkOrderWorkUpdateId)
    {
        return (StockOutReturUsed)DataAccess.GetSingleRowByQuery(0, $"{Qry} where a.WorkOrderWorkUpdateId = '{WorkOrderWorkUpdateId}' ", new StockOutReturUsed());
    }
    public static StockOutReturUsed GetByWorkOrderWorkUpdateItemId(object WorkOrderWorkUpdateId, object WorkOrderWorkUpdateItemId)
    {
        return (StockOutReturUsed)DataAccess.GetSingleRowByQuery(0, $"{Qry} where a.WorkOrderWorkUpdateId = '{WorkOrderWorkUpdateId}' and a.WorkOrderWorkUpdateItemId = '{WorkOrderWorkUpdateItemId}' ", new StockOutReturUsed());
    }
    public static StockOutReturUsed GetByWorkOrderWorkUpdateItemId(object WorkOrderWorkUpdateItemId)
    {
        return (StockOutReturUsed)DataAccess.GetSingleRowByQuery(0, $"{Qry} where a.WorkOrderWorkUpdateItemId = '{WorkOrderWorkUpdateItemId}'", new StockOutReturUsed());
    }
    public static List<object> GetALL()
    {
        return DataAccess.GetDataByQuery(0, "Select * from StockOutReturUsed", new StockOutReturUsed());
    }
    public static List<object> GetByCriteria(List<clsParameter> pList, out double TotalRow)
    {
        return DataAccess.GetDataBySPPaging(0, "StockOutReturUsedGetByCriteria", pList, out TotalRow, new StockOutReturUsed());
    }
    public static List<object> GetByWorkOrderAndItem(object WorkOrderId, object ItemId)
    {
        return DataAccess.GetDataByQuery(0, $"{Qry} where a.WorkOrderId = '{WorkOrderId}' and e.Id = '{ItemId}' ", new StockOutReturUsed());
    }
    public static int GetTotalQtyByWorkOrderItem(object WorkOrderId, object ItemId)
    {
        return DataAccess.GetSingleValueByQuery(0, $"Select SUM(a.Qty) from StockOutReturUsed a Left Join WorkOrder b On b.Id = a.WorkOrderId Left Join StockOutItem d On d.Id = a.StockOutItemId Left Join Item e on e.Id = d.ItemId where a.WorkOrderId = '{WorkOrderId}' and e.Id = '{ItemId}' ").ToInt();
    }
    public static int GetQty(object StockOutId)
    {
        return DataAccess.GetSingleValueByQuery(0, $"Select SUM(Qty) from StockOutReturUsed where StockOutId = '{StockOutId}' ").ToInt();
    }
    #endregion
    #region ChangeData
    public string Insert()
    {
        List<clsParameter> pList = new List<clsParameter>();
        pList.Add(new clsParameter("WorkOrderId", WorkOrderId));
        pList.Add(new clsParameter("WorkOrderWorkUpdateId", WorkOrderWorkUpdateId));
        pList.Add(new clsParameter("WorkOrderWorkUpdateItemId", WorkOrderWorkUpdateItemId));
        pList.Add(new clsParameter("StockReceivedItemId", StockReceivedItemId));
        pList.Add(new clsParameter("StockOutId", StockOutId));
        pList.Add(new clsParameter("StockOutItemId", StockOutItemId));
        pList.Add(new clsParameter("ReceiverUserId", ReceiverUserId));
        pList.Add(new clsParameter("WarehouseId", WarehouseId));
        pList.Add(new clsParameter("RackId", RackId));
        pList.Add(new clsParameter("Code", Code));
        pList.Add(new clsParameter("Reason", Reason));
        pList.Add(new clsParameter("ReturDate", ReturDate));
        pList.Add(new clsParameter("ItemId", ItemId));
        pList.Add(new clsParameter("Qty", Qty));
        pList.Add(new clsParameter("Author", U.GetUsername()));
        return DataAccess.Save(0, "StockOutReturUsedInsert", pList);
    }
    public string Update()
    {
        List<clsParameter> pList = new List<clsParameter>();
        pList.Add(new clsParameter("Id", Id));
        pList.Add(new clsParameter("WorkOrderId", WorkOrderId));
        pList.Add(new clsParameter("WorkOrderWorkUpdateId", WorkOrderWorkUpdateId));
        pList.Add(new clsParameter("WorkOrderWorkUpdateItemId", WorkOrderWorkUpdateItemId));
        pList.Add(new clsParameter("StockReceivedItemId", StockReceivedItemId));
        pList.Add(new clsParameter("StockOutId", StockOutId));
        pList.Add(new clsParameter("StockOutItemId", StockOutItemId));
        pList.Add(new clsParameter("ReceiverUserId", ReceiverUserId));
        pList.Add(new clsParameter("WarehouseId", WarehouseId));
        pList.Add(new clsParameter("RackId", RackId));
        pList.Add(new clsParameter("Code", Code));
        pList.Add(new clsParameter("Reason", Reason));
        pList.Add(new clsParameter("ReturDate", ReturDate));
        pList.Add(new clsParameter("ItemId", ItemId));
        pList.Add(new clsParameter("Qty", Qty));
        pList.Add(new clsParameter("Author", U.GetUsername()));
        return DataAccess.Save(0, "StockOutReturUsedUpdate", pList);
    }
    public static string Delete(object Id)
    {
        return DataAccess.Delete(0, Id, "Id", "StockOutReturUsed");
    }
    #endregion
    #endregion
}